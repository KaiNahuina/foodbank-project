#region

using Foodbank_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#endregion

namespace Foodbank_Project.Data;

public static class SeedData
{
    public static async Task SeedRecipesAsync(ApplicationContext ctx)
    {
        var meatCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 1) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 1,
            Name = "Meat"
        }).Entity ;
        var vegetarianCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 2) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 2,
            Name = "Vegetarian"
        }).Entity;
        var fishCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 3) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 3,
            Name = "Fish"
        }).Entity;
        var soupsCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 4) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 4,
            Name = "Soups"
        }).Entity;
        var specialCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 5) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 5,
            Name = "Special"
        }).Entity;
        var snacksCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 6) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 6,
            Name = "Snacks"
        }).Entity;
        var sideDishesCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 7) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 7,
            Name = "Side Dishes"
        }).Entity;
        var DessertsCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 8) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 8,
            Name = "Desserts"
        }).Entity;


        await using (var transaction = await ctx.Database.BeginTransactionAsync())
        {
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[RecipeCategories] ON");
            await ctx.SaveChangesAsync();
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[RecipeCategories] OFF");
            await transaction.CommitAsync();
        };
        
        



        var _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 1) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Omelette",
            RecipeId = 1,
            Ingredients = "4 eggs \n 1/8 teaspoon of pepper \n 4 tablespoons of water \n 1 tablespoon of butter \n 1/2 teaspoon of salt \n a little parsley",
            Status = Status.Approved,
            Method = "Beat the yolks until thick and lemon colored\nAdd hot water (one tablespoonfull to an egg), salt and pepper\n" +
            "Beat the whites till stiff and dry\nCut and fold into first mixture\nHeat the omelette pan, add the butter, turn the pan so that the" +
            "melted butter covers teh sides and the bottom of the pan.\nTurn in mixture, spread evenly, turn down the fire and allow the omelette to cook slowly\n" +
            "Turn the pan so that the omelette will brown evenly",
            Categories = new List<RecipeCategory> { vegetarianCategory, },
            Notes = "",
            Serves = "9 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/omelette.jpg")
        }).Entity;
      

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 2) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Berry Pie",
            RecipeId = 2,
            Ingredients = "1 1/2 cups of berries (black or blue)\n2 tablespoons flour\n1/8 teaspoons salt\n1/2 cups sugar\n1 tablespoon of lemon juice",
            Status = Status.Approved,
            Method = "Wash the fruit, mix with the sugar, flour, salt and lemon juice.\nLine a deep pie tin with a plain pie paste and sprinkle" +
            " one tablespoon of sugar over the bottom crust\nAdd the berry mixture\nWet the lower crust slightly\nRoll out the upper crust and make " +
            "slits in the middle to allow the steam to escape\nPlace on the lower crust pinching the edges together\nBake in a moderately hot oven 40 min",
            Categories = new List<RecipeCategory> { DessertsCategory },
            Notes = "",
            Serves = "4 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Berry.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 3) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Creamed Beef",
            RecipeId = 3,
            Ingredients = "1/4 pound diced beef, thinly slice\n2 tablespoon flour\n2 tablespoon butter\n1 cup of milk",
            Status = Status.Approved,
            Method = "Place the butter in a frying pan, and when the pan is hot and the butter is melted, add the beef\nAllow it to frizzle\n" +
            "Add the flour, mix thoroughly with beef and butter, allowing the flour to brown a little\nAdd the milk slowly, cooking until thick and smooth\n" +
            "Pour over rounds of toast\nGarnish with parsley",
            Categories = new List<RecipeCategory> { meatCategory, snacksCategory },
            Notes = "",
            Serves = "4 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Creamb.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 4) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Veal Loaf",
            RecipeId = 4,
            Ingredients = "2 pounds of lean veal\n4 tespoons of onion salt\n1/2 pound of salt pork\n1 tablespoon of salt\n6 large crackers\n1/2 teaspoon pepper/n" +
            "2 tablespoon lemon juice\n4 tablespoon cream",
            Status = Status.Approved,
            Method = "Put 2 crackers in meat grinder, add bits of meat and pork and the rest of the crackers\nThe crackers first and last prevent " +
            "the pork and meat from sticking to the grinder\nAdd another ingredient in order named\nPack in a well-buttered bread-pan\nSmooth evenly on top, brush " +
            "with white of an egg and bake 1 hour in a moderate oven\nBaste frequently\nThe meat may be cooked in a fireless cooker between two stones\n" +
            "It is perfectly satisfactory cooked this way, and requires no basting",
            Categories = new List<RecipeCategory> { meatCategory, specialCategory },
            Notes = "",
            Serves = "6-8 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Veal.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 5) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Stuffed Tomatoes Bettina",
            RecipeId = 5,
            Ingredients = "2 firm good sized tomatoes\n3 tablespoon fresh-baked bread crumbs\n2 tablespoons of mixed vegs\n1 tablespoon chopped cooked " +
            "ham or cooked bacon\n1/8 tablespoon paprika\n1 tablespoon egg\n1 tablespoon melted butter\n1/2 teaspoon salt",
            Status = Status.Approved,
            Method = "Wash tomatoes thoroughly and cut a slice 1 inch in diameter from the blossom end, reserving it for future use\n" +
            "Carefully scoop out the pulp, being careful to leave the shell firm\nAdd bread crumbs, tomato pulp, vegs, chopped meat, egg, melted butter, " +
            "salt and paprika\nCook the mixture four minutes over the fire\nPut the slices back on the tomatoes\nPlace in a small pan and bake twenty minutes in a hot oven\nServe",
            Categories = new List<RecipeCategory> { meatCategory },
            Notes = "",
            Serves = "2 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Tomato.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 6) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Dormers",
            RecipeId = 6,
            Ingredients = "1 pound cold mutton\n4 ounce beef suet\npepper\nsalt\n1/2 pound boiled rice\n1 egg\nbread crumbs\ncanned gravy or soup",
            Status = Status.Approved,
            Method = "Chop meat, suet and rice fine, and mix with pepper and salt\nRoll like sausages in egg and bread crumbs\nfry in dripping til brown\n" +
            "Serve in a dish with mashed potatoes round them, and pour on some hot gravy/soup",
            Categories = new List<RecipeCategory> { meatCategory, soupsCategory },
            Notes = "",
            Serves = "4 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Dormer.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 7) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Dutch country Meat Loaf",
            RecipeId = 7,
            Ingredients = "2 pound ground beef\n3 eggs\n1/2 pounds of ground pork\n2 cup bread crumbs\n1/2 pound ground veal\n1 medium onion\n2 cups of milk\n" +
            "salt\npepper",
            Status = Status.Approved,
            Method = "Soak breadcrumbs in milk for a few minutes\nCombine the meat, bread crumbs, eggs, onions and seasoning\nMix thoroughly and shape into a " +
            "loaf\nPlace  in a roasting pan, add 1/2 inch of water and bake in a medium oven(375f/190c) for 1 and a half to 2 hours",
            Categories = new List<RecipeCategory> { meatCategory },
            Notes = "",
            Serves = "6-8 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Dutch.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 8) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Goulash",
            RecipeId = 8,
            Ingredients = "1/4 pound of pork\n1 1/4 pound of beef\n7 cups water\n1 tablespoon of finely chopped celery/celery salt\n1 teaspoon chilipowder\n" +
            "1 teaspoon parsley\n1/4 cup onion\n 1/8 pepper\n1/4 teaspoon paprika\n6 tablespoons flour\n1 1/2 cups water\n8 gingersnaps\n1/2 cup water\n" +
            "2 cup ketchup\n1 tablespoon brown sugar\n1/2 cup water/n2 tablespoon vinegar\n1/2 can kidney beans",
            Status = Status.Approved,
            Method = "Cut meat into bite-sized pieces\nPlace meat in pressure cooker with water or boil in pot\nAdd: celery salt, chili powder, parsley " +
            "onion, pepper, paprika\nBoil 50 minutes under 10 pounds pressure or until tender\nWhile cooking place 2 tbsp grease in medium-sized skillet " +
            "on medium heat\nBlend in flour\nBrown stirring frequently and let cood\nAdd 1 1/2 cups water gradually\nStir until smooth then set aside\n" +
            "Place gingersnaps in a bowl\nPour 1/2 cup water over the gingersnaps\nSet aside until meat is done\nThen add gingersnaps and browned flour mix\n" +
            "Add: ketchup, brown sugar, vinegar, 1/2 cup water, kidney beans\nBest when let it set for 3 to 4 hours\nAdd 1/2 if it is too thick",
            Categories = new List<RecipeCategory> { meatCategory, soupsCategory },
            Notes = "",
            Serves = "6 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Goulash.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 9) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Chile verde",
            RecipeId = 9,
            Ingredients = "1 large yellow onion, dice\n1 tablespoon olive oil\n3 to 4 garlic cloves, minced\n2 1/2 cup diced roasted green chile\n4 " +
            "pound pork, roast\n8 cups chicken stock\n2 tablespoons dried oregano\nsalt\ndiced tomatoes",
            Status = Status.Approved,
            Method = "Heat olive oil in a saucepan over medium high heat\nAdd onion, garlics and sautee until soft\nAdd green chile and sautee for 3-4 minutes " +
            "more until chile is heated through\nRemove from heat\nTrim excess fat from pork roast and season with salt and pepper\nPlace pork in a large " +
            "slow cooker (5-6 quarts)\nCover roast with onion mixture\nAdd chicken stock to roast\nAdd oregano and tomatoes\nCover pot and cook for " +
            "6-8 hours on medium low heat\nRemove roast from cooker and let cool for a few minutes\nRoast should break into manageable chunks\nTrim fat " +
            "and shred meat into bite size pieces using 2 forks\nSkin excess fat from cooked broth\nAdd shredded meat back to cooker\nHeat on medium until " +
            "meat is warmed through\nSeason with salt and pepper to taste\nServe in bowls with warm corn tortillas and avocado slices",
            Categories = new List<RecipeCategory> { meatCategory, soupsCategory },
            Notes = "",
            Serves = "4 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Chile.jpg")
        }).Entity;


        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 10) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Devilled Eggs",
            RecipeId = 10,
            Ingredients = "1 teaspoon melted butter\n 6 hard-cooked eggs\n 1 teaspoon vinegar\n 1/4 teaspoon chopped parsley\n 1/4 teaspoon mustard\n 1/4 teaspoon salt",
            Status = Status.Approved,
            Method = "Shell the eggs, cut lengthwise in half, remove yolks, mash them and add vinegar, mustard, melted butter, parsley and salt.\nRefill the whites and put pairs together.\nWrap in tissue paper with frilled edges to represent torpedoes.",
            Categories = new List<RecipeCategory> { sideDishesCategory, vegetarianCategory },
            Notes = "Boil and mash potatoes.\n Sift flour, salt and baking powder together.\n Add potatoes and cream in the lard.\n Mix to a light dough with egg and milk.\n Roll out rather thin and bake in hot oven until brown.\n Serve hot.",
            Serves = "6 eggs",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Devilled Eggs.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 11) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Potato Biscuit",
            RecipeId = 11,
            Ingredients = "2 large potatoes\n 1/2 teaspoon salt\n 3 cup flour\n 1 / 4 cup lard\n 3 teaspoon baking powder\n 1 egg\n 1 cup milk",
            Status = Status.Approved,
            Method = "Cook Chips\nServe",
            Categories = new List<RecipeCategory> { vegetarianCategory, sideDishesCategory },
            Notes = "",
            Serves = "3 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Potato Biscuit.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 12) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Sweet sour Beans",
            RecipeId = 12,
            Ingredients = "1 can green beans\n1/4 cup gingersnap, crumbs\n3/4 teaspoon salt\n1/4 teaspoon pepper\n1/4 cup vinegar\n1/4 cup liquid of beans\n1/2 cup of milk",
            Status = Status.Approved,
            Method = "Heat green beans\nDrain and save liquid\n" +
           "Drain and add measured water and small onion\nCook gently for 5 or 6 hours until liquid is reduced one half\nCombine crumbs, seasonings, vinegar " +
           "and liquid of beans\nBoil one minute, string constantly\nAdd a mixture to hot milk and pour over beans",
            Categories = new List<RecipeCategory> { vegetarianCategory, sideDishesCategory },
            Notes = "",
            Serves = "4-6 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Sweet.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 13) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Nut Cake",
            RecipeId = 13,
            Ingredients = "1 1/2 cup sugar\n 1 cup nut meats, chopped\n 1 / 2 cup butter\n 2 teaspoon baking powder\n 3 eggs, separated\n 3 / 4 cup milk\n 2 1 / 2 cup flour\n a little salt\n",
            Status = Status.Approved,
            Method = "Rub butter and sugar to a light, white cream.\n Add egg yolks and beat until smooth\n Sift flour, salt and baking powder and add, together with milk, a little at a time, beating well.\n Fold in chopped nuts and stiffly beaten egg whites.\n Pour into 2 nine inch cake pans or 1 loaf pan.\n Bake in medium oven (350-f) for 30 minutes for layer cake or 1 hour for loaf cake.\n Use hickory nuts, black walnuts or shellbarks.",
            Categories = new List<RecipeCategory> { vegetarianCategory, DessertsCategory, specialCategory },
            Notes = "",
            Serves = "5 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Nut Cake.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 14) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Vegetable Pasta",
            RecipeId = 14,
            Ingredients = "1 butternut squash\n 1 onion\n 1 stick goat cheese\n small sized pasta, (bowties, spirals)\n fresh basil\n candied pecans or walnuts",
            Status = Status.Approved,
            Method = "Dice butternut squash, fennel and brown onion.\n Add to baking dish with generous amount of olive oil and mixed herbs mix ingredients.\n Place in oven at 425f for 60 minutes.\n Let stick of goat cheese sit at room temperature (or warm slightly on oven top).\n Cook pasta.\n Add goat cheese to bottom of mixing bowl, add cooked fresh pasta.\n The heat will help melt cheese.\n Add basil and pecans and mix.\n Add cooked vegetables and mix.",
            Categories = new List<RecipeCategory> { vegetarianCategory },
            Notes = "",
            Serves = "4 servings",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/NoPorkQL.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 15) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Oatmeal Porridge",
            RecipeId = 15,
            Ingredients = "oatmeal\nwater\nmilk\nsalt\ngolden syrup",
            Status = Status.Approved,
            Method = "Boil some water with a little salt in it, sprinkle oatmeal in slowly, stirring gently for 15 or 20 minutes until it is thick enough.\nPour it at once on to the plates from which it is to be eaten, and serve with cold milk or a little syrup or your favourite fruit.",
            Categories = new List<RecipeCategory> { DessertsCategory },
            Notes = "",
            Serves = "1",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Oatmeal Porridge.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 16) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Potatoes Anna",
            RecipeId = 16,
            Ingredients = "1 cup cooked diced potatoes\n1/2 teaspoon celery salt\n2 hard-cooked eggs\n1/4 teaspoon onion salt\n1 cup thin white sauce",
            Status = Status.Approved,
            Method = "Place alternate layers of diced cooked potatoes and sliced hard-cooked eggs in a baking dish.\nSeason.\nPour a thin white sauce over all of this.\nPlace in a moderate oven fifteen \nminutes.",
            Categories = new List<RecipeCategory> { vegetarianCategory, sideDishesCategory },
            Notes = "",
            Serves = "4",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Potatoes Anna.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 17) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Fish Chowder",
            RecipeId = 17,
            Ingredients = "10 pound fish, (about 1 gallon, diced)\n5 1/2 quart hot fish stock\n1/2 pound salt pork, (1 cup, diced)\n6 onions, sliced\n1 teaspoon pepper\n3 tablespoon salt, (1 1/2 ounces)\n1/4 cup flour, (1 ounce)\n3 quart diced potatoes\n3 quart evaporated milk\n12 hard cooked eggs, chopped, may be omitted",
            Status = Status.Approved,
            Method = "Halibut, haddock or cod are best.\nRemove bone and cut fish into small pieces.\nCook head and back bone in 6 quarts boiling water 15 minutes.\nStrain.\nThere should be 5 1/2 quarts fish stock.\nFry salt pork and onions in soup kettle until onions are slightly brown.\nRemove pork and onions and keep hot.\nArrange layer of fish in bottom of kettle and sprinkle with salt, pepper and flour.\nAdd a layer of potatoes, and then the onion and pork.\nRepeat.\nAdd the fish stock and cook slowly without stirring until potatoes and fish are done, about 45 minutes.\nAdd scalded milk.\n Garnish each serving with chopped egg.",
            Categories = new List<RecipeCategory> { fishCategory },
            Notes = "",
            Serves = "40",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Fish Chowder.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 18) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Cod Fish Stew",
            RecipeId = 18,
            Ingredients = "1 1/2 pound fresh or thawed, frozen cod fillets\n1/4 cup green peppers, diced (optional)\n3 strips bacon\n4 teaspoon salt\n3 medium onions, sliced\n1/4 teaspoon pepper\n1 1/2 pound peeled white potatoes, cut in 3/4 inch cubes\n3 cup boiling water\n1 No. 2-1/2 can tomatoes\n1/2 teaspoon celery seed, or 1/2 cup diced celery\n2 tablespoon parsley, minced\n3 large peeled carrots, cut in 3/4 inch cubes",
            Status = Status.Approved,
            Method = "Saute bacon in deep kettle or dutch oven until lightly browned; then set bacon aside.\nIn same kettle, saute onions until tender.\nAdd fish cut in 2 1/2 inch pieces; add next seven ingredients.\nSimmer, covered, until vegetables are tender, about 25 minutes.\nAdd tomatoes, heat through.\nGarnish with parsley and bacon bits.In same kettle, saute onions until tender.\nAdd fish cut in 2 1/2 inch pieces; add next seven ingredients.\nSimmer, covered, until vegetables are tender, about 25 minutes.\nAdd tomatoes, heat through.\nGarnish with parsley and bacon bits.",
            Categories = new List<RecipeCategory> { fishCategory },
            Notes = "",
            Serves = "6",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Cod Fish Stew.jpg")
        }).Entity;

        _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 19) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Currant Jelly",
            RecipeId = 19,
            Ingredients = "2 quart currants\nsugar",
            Status = Status.Approved,
            Method = "Pick over currants, but do not remove the stems.\n Wash and drain.\n Mash a few with a vegetable masher in the bottom of a porcelain - lined or granite kettle.\n Add more currants and mash.\n Continue adding currants until all are used.\n Bring to a boil slowly and let simmer without stirring until the currants appear white.\n Strain through a coarse strainer, and allow juice to drain through a jelly bag.\nMeasure the juice, and boil ten minutes. Gradually add an equal amount of heated sugar, stirring occasionally to prevent burning, and continue boiling until the test shows that the mixture has jelled. When filling sterilized glasses, place them in a pan containing a little boiling water.\nThis keeps the glasses from breaking when hot jelly is poured in. Fill and set the glasses of jelly aside to cool.\nCover with hot melted paraffin.",
            Categories = new List<RecipeCategory> { DessertsCategory, specialCategory, sideDishesCategory },
            Notes = "",
            Serves = "9",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/Currant Jelly.jpg")
        }).Entity;



        await using (var transaction = await ctx.Database.BeginTransactionAsync())
        {
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Recipes] ON");
            await ctx.SaveChangesAsync();
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Recipes] OFF");
            await transaction.CommitAsync();
        }
        
    }
    
    public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("FoodbankAdmin")); // unlimited access to single foodbank
        
        await roleManager.CreateAsync(new IdentityRole("SiteAdmin")); // unlimited access to global site
        await roleManager.CreateAsync(new IdentityRole("FoodbanksAdmin")); // unlimited access to all foodbanks
        await roleManager.CreateAsync(new IdentityRole("UsersAdmin")); // unlimited access to users
        await roleManager.CreateAsync(new IdentityRole("RecipeAdmin")); // unlimited access to recipes
        await roleManager.CreateAsync(new IdentityRole("LoggingAdmin")); // unlimited access to logging
        await roleManager.CreateAsync(new IdentityRole("ApprovalAdmin")); // unlimited access to approval/denial
    }


    public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Seed Basic User
        var defaultUser = new IdentityUser
        {
            UserName = "user@user.com",
            Email = "user@user.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "236!User?339");
                await userManager.AddToRoleAsync(defaultUser, "FoodbankAdmin");
            }
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Seed SuperAdmin User
        var defaultUser = new IdentityUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "236!Admin?339");
                await userManager.AddToRoleAsync(defaultUser, "SiteAdmin");
            }
        }
    }
}