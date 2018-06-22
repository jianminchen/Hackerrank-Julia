using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    internal class CostInfo
    {
        public int RecipeID { get; set; }
        public HashSet<int> ingredients { get; set; }
        public int Cost { get; set; }
    }

    static void Main(String[] args)
    {
        ProcessInput();
        //RunSampleTestcase();
    }

    public static void RunSampleTestcase()
    {
        int recipes = 4;
        //int ingredients = 5;
        int dishes = 2;
        //int pantries = 2;

        var pantry = new int[] { 3, 4 };

        var cost = new int[] { 1, 3, 2, 4, 8 };

        int[][] recipe = new int[recipes][];

        recipe[0] = new int[] { 0, 0, 1, 0, 0 };
        recipe[1] = new int[] { 1, 1, 1, 1, 1 };
        recipe[2] = new int[] { 1, 0, 0, 0, 1 };
        recipe[3] = new int[] { 1, 1, 0, 1, 1 };

        // your code goes here
        var recipeToRemove = new HashSet<int>();
        int minimumCost = ChoosingRecipesMinimuCostGreedyApproach(recipe, cost, dishes, pantry, recipeToRemove);
    }

    public static void ProcessInput()
    {
        int queries = Convert.ToInt32(Console.ReadLine());
        for (int query = 0; query < queries; query++)
        {
            string[] tokens_r = Console.ReadLine().Split(' ');
            int recipes = Convert.ToInt32(tokens_r[0]);
            int ingredients = Convert.ToInt32(tokens_r[1]);
            int dishes = Convert.ToInt32(tokens_r[2]);
            int pantries = Convert.ToInt32(tokens_r[3]);

            string[] pantry_temp = Console.ReadLine().Split(' ');
            int[] pantry = Array.ConvertAll(pantry_temp, Int32.Parse);

            string[] cost_temp = Console.ReadLine().Split(' ');
            int[] cost = Array.ConvertAll(cost_temp, Int32.Parse);

            int[][] recipe = new int[recipes][];

            for (int recipe_i = 0; recipe_i < recipes; recipe_i++)
            {
                string[] recipe_temp = Console.ReadLine().Split(' ');
                recipe[recipe_i] = Array.ConvertAll(recipe_temp, Int32.Parse);
            }

            // your code goes here
            var recipeToRemove = new HashSet<int>();
            int minimumCost = ChoosingRecipesMinimuCostGreedyApproach(recipe, cost, dishes, pantry, recipeToRemove);
            Console.WriteLine(minimumCost);
        }
    }

    ///
    /// Calculate minimum cost for n dishes
    /// cost - int[], cost for each ingredient
    /// dishes - 
    /// pantry - int[], 
    /// recipe - int[][] - recipes 1 - 30, each recipe with all ingredient 0 or 1
    /// 
    /// once the ingredient is purchased, it can be used infinite recipes <- do not count more than once
    /// 
    /// This algorithm is designed to take greedy approach, it will not guarantee the minimum cost for all n dishes. 
    /// 
    /// 
    public static int ChoosingRecipesMinimuCostGreedyApproach(
        int[][] recipe,
        int[] cost,
        int dishes,
        int[] pantry,
        HashSet<int> recipeToRemove)
    {
        if (dishes == 0)
        {
            return 0;
        }

        var recipes = recipe.Length;
        var ingredients = recipe[0].Length;

        var pantryIngredients = new HashSet<int>(pantry);
        var minimumPurchasedIngridents = new HashSet<int>();

        // duplicate purchases first
        int minimumCost = Int32.MaxValue;
        int recipeId = 0;

        for (int i = 0; i < recipes; i++)
        {
            if (recipeToRemove.Contains(i))
            {
                continue;
            }

            int costExcludingPantryOnes = 0;
            var purchasedIngredients = new HashSet<int>();

            for (int ingredient = 0; ingredient < ingredients; ingredient++)
            {
                int requireIngredient = recipe[i][ingredient];

                if (requireIngredient == 0 ||
                    pantryIngredients.Contains(ingredient)
                    )
                {
                    continue;
                }

                purchasedIngredients.Add(ingredient);
                costExcludingPantryOnes += cost[ingredient];
            }

            if (costExcludingPantryOnes < minimumCost)
            {
                minimumPurchasedIngridents.Clear();

                minimumCost = costExcludingPantryOnes;
                minimumPurchasedIngridents = new HashSet<int>(purchasedIngredients);
                recipeId = i;
            }
        }

        var newPantry = new HashSet<int>(pantry);
        newPantry.UnionWith(minimumPurchasedIngridents);
        recipeToRemove.Add(recipeId);

        return minimumCost + ChoosingRecipesMinimuCostGreedyApproach(recipe, cost, dishes - 1, newPantry.ToArray(), recipeToRemove);
    }
}
