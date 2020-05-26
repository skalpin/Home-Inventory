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
            var fileRecipieProvier = new FileRecipieProvider();
            var missingService = new MissingItemsService(
                fileRecipieProvier.GetIngridentsFor(SelectRecipies(fileRecipieProvier.GetAllRecipies()))
                .Concat(GetStaples()));

            Console.WriteLine();

            while (!missingService.IsComplete())
            {
                var nextIngredient = missingService.NextItem();
                Console.Write($"Do you have {nextIngredient.Quantity} {nextIngredient.Unit} {nextIngredient.Name}?");
                Console.Write(" (y/n) ");
                var response = Console.ReadKey();
                Console.WriteLine();
                if (response.Key == ConsoleKey.Y)
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
                foreach (var item in group)
                    Console.WriteLine($"  {item.Quantity} {item.Unit} {item.Name}");
            }
        }

        private static IEnumerable<Ingrident> GetStaples()
        {
            Console.WriteLine("How many days are you shopping for?");
            var days = 0;
            while (!int.TryParse(Console.ReadLine(), out days) && days < 0)
                ;
            return StaplesProvider.GetStablesFor(days);
        }

        private static IEnumerable<string> SelectRecipies(IEnumerable<string> recipies)
        {
            foreach (var recipie in recipies)
            {
                Console.WriteLine();
                Console.Write($"{recipie}? ");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                    yield return recipie;
            }
        }
    }
}
