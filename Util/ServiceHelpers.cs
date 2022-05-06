#region

using System.Text;

#endregion

namespace Foodbank_Project.Util;

public static class ServiceHelpers
{
    public struct ResultWrapper<T>
    {
        public ResultWrapper()
        {
            Result = default;
            ResultCode = Code.NotRun;
            Ex = null;
        }

        public Code ResultCode;
        public T? Result;
        public Exception? Ex;

        public enum Code
        {
            Success,
            Timeout,
            Errored,
            NotRun,
            NoErrorNoRun,
            Cancelled
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("ResultWrapper<");
            sb.Append(typeof(T));
            sb.AppendLine(">:");
            sb.Append("    Result: ");
            sb.AppendLine(Result?.ToString() ?? "");
            sb.Append("    Code: ");
            sb.AppendLine(ResultCode.ToString());
            if (Ex != null) sb.AppendLine(Ex.ToString());
            return sb.ToString();
        }
    }

    public static async Task<ResultWrapper<T>> TimeoutTask<T>(int timeoutMs,
        Func<CancellationToken, Task<T>> target, CancellationToken cancellationToken)
    {
        var cancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var task = Task.Run(() => target(cancellation.Token), cancellation.Token);
        var timeoutTask = Task.Delay(timeoutMs, cancellation.Token);

        try
        {
            await await Task.WhenAny(timeoutTask, task);
            if (task.IsCompletedSuccessfully)
            {
                AttemptCancel(cancellation);
                return new ResultWrapper<T>
                {
                    ResultCode = ResultWrapper<T>.Code.Success,
                    Result = task.Result
                };
            }

            if (timeoutTask.IsCompletedSuccessfully)
            {
                AttemptCancel(cancellation);
                return new ResultWrapper<T>
                {
                    ResultCode = ResultWrapper<T>.Code.Timeout
                };
            }

            AttemptCancel(cancellation);
            return new ResultWrapper<T>
            {
                ResultCode = ResultWrapper<T>.Code.NoErrorNoRun
            };
        }
        catch (TaskCanceledException)
        {
            return new ResultWrapper<T>
            {
                ResultCode = ResultWrapper<T>.Code.Cancelled
            };
        }
        catch (Exception ex)
        {
            try
            {
                AttemptCancel(cancellation);
                return new ResultWrapper<T>
                {
                    Ex = ex,
                    ResultCode = ResultWrapper<T>.Code.Errored
                };
            }
            catch (TaskCanceledException)
            {
                return new ResultWrapper<T>
                {
                    Ex = ex,
                    ResultCode = ResultWrapper<T>.Code.Cancelled
                };
            }
            catch (Exception ex2)
            {
                return new ResultWrapper<T>
                {
                    ResultCode = ResultWrapper<T>.Code.Errored,
                    Ex = new AggregateException(ex, ex2)
                };
            }
        }
    }

    private static void AttemptCancel(CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            cancellationTokenSource.Cancel();
        }
        catch (TaskCanceledException)
        {
            // silently kill error
        }
        // rethrow exception to preserve stack
    }
}