using System.Collections.Generic;
using System.Linq;
using Home.Invintory.Service.Model;

namespace Home.Invintory.Service.Services
{
    public class MissingItemsService
    {
        private readonly IEnumerable<Item> items;

        public MissingItemsService(IEnumerable<Ingrident> ingridents)
        {
            ingridents
            .GroupBy(i => i.Name)
            .Select(g => new Item
            {
                Asked = false,
                Needed = false,
                Ingredient = g.Aggregate((a, b) => new Ingrident { Name = a.Name, Quantity = a.Quantity + b.Quantity, Unit = a.Unit })
            });
        }

        public bool IsComplete() => items.Any(i => !i.Asked);

        public Item NextItem() => items.FirstOrDefault(i => !i.Asked);

        public void UpdateItem(Item item)
        {
            var original = items.FirstOrDefault(i => i.Ingredient.Name == item.Ingredient.Name);
            original.Asked = true;
            original.Needed = item.Needed;
        }

        public IEnumerable<Item> GetList() => items.Where(i => i.Needed);
    }
}
