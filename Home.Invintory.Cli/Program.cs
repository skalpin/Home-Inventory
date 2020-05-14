using System;
using System.Collections.Generic;
using System.Linq;
using Home.Invintory.Service;
using Home.Invintory.Service.Model;
using Home.Invintory.Service.Services;

namespace Home.Invintory.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's build your grocery list");
            Console.WriteLine("Please enter a comma separated list of meals you need to make sure you have everything for.");
            var meals = Console.ReadLine().Split(',');

            var fileRecipieProvier = new FileRecipieProvider();
            var missingService = new MissingItemsService(fileRecipieProvier.GetIngridentsFor(meals));

            while(!missingService.IsComplete())
            {
                var nextIngredient = missingService.NextItem();
                Console.Write($"Do you have {nextIngredient.Quantity} {nextIngredient.Unit} {nextIngredient.Name}?");
                Console.Write(" (y/n) ");
                var response = Console.ReadKey();
                Console.WriteLine();
                if(response.Key == ConsoleKey.Y)
                {
                    missingService.UpdateItem(nextIngredient.Name, false);
                }
                else if (response.Key == ConsoleKey.N)
                {
                    missingService.UpdateItem(nextIngredient.Name, true);
                }
            }

            Console.WriteLine("You need to purchase:");
            foreach (var group in missingService.GetList().GroupBy(i => i.Department).OrderBy(i => i.Key))
            {
                Console.WriteLine("********************************************************************************");
                Console.WriteLine(Enum.GetName(typeof(Department), group.Key));
                Console.WriteLine("********************************************************************************");
                foreach(var item in group)
                    Console.WriteLine($"  {item.Quantity} {item.Unit} {item.Name}");
            }
        }
    }
}
