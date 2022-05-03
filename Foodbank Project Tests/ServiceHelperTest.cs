#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Foodbank_Project.Util;
using Xunit;

#endregion

namespace Foodbank_Project_Tests;

public class ServiceHelperTests
{
    private class TimeoutTaskTimoutSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 800 };
            yield return new object[] { 2, 50 };
            yield return new object[] { 4, 40 };
            yield return new object[] { 8, 70 };
            yield return new object[] { 16, 3000 };
            yield return new object[] { 32, 80 };
            yield return new object[] { 64, 4000 };
            yield return new object[] { 128, 9000 };
            yield return new object[] { 4256, 90000 };
            yield return new object[] { 6512, 90000 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    
    [Theory]
    [ClassData(typeof(TimeoutTaskTimoutSet))]
    public async void TimeoutTask(int timeout, int taskDuration)
    {
        var result = await ServiceHelpers.TimeoutTask(timeout, CancellationToken.None, async (token) =>
        {
            await Task.Delay(taskDuration, token);
            return true;
        });
        Assert.Equal(new ServiceHelpers.ResultWrapper<bool>
        {
            ResultCode = ServiceHelpers.ResultWrapper<bool>.Code.Timeout,
            Result = false
        }, result);
    }


    [Fact]
    public async void ErroredTask_Exception()
    {
        var result = await ServiceHelpers.TimeoutTask(1000, CancellationToken.None, async (token) =>
        {
            throw new Exception("Some error occured");
#pragma warning disable CS0162
            return true;
#pragma warning restore CS0162
        });
        Assert.Equal(ServiceHelpers.ResultWrapper<bool>.Code.Errored, result.ResultCode);
    }
    
    [Fact]
    public async void ErroredTask_Result()
    {
        var result = await ServiceHelpers.TimeoutTask(1000, CancellationToken.None, async (token) =>
        {
            throw new Exception("Some error occured");
            return true;
        });
        Assert.False(result.Result);
    }
}