using System.Collections.Generic;
using System.Linq;
using Home.Invintory.Service.Model;

namespace Home.Invintory.Service.Services
{
    public class MissingItemsService
    {
        private readonly List<Item> items;

        public MissingItemsService(IEnumerable<Ingrident> ingridents)
        {
            items = ingridents
            .GroupBy(i => i.Name)
            .Select(g => new Item
            {
                Asked = false,
                Needed = false,
                Ingrident = g.Aggregate((a, b) => new Ingrident
                {
                    Name = a.Name,
                    Quantity = a.Quantity + b.Quantity,
                    Unit = a.Unit,
                    Department = a.Department
                })
            })
            .OrderBy(i => i.Ingrident.Department)
            .ToList(); // must make sure the list is only executed once.
        }

        public bool IsComplete() => !items.Any(i => !i.Asked);

        public Ingrident NextItem() => items.FirstOrDefault(i => !i.Asked)?.Ingrident;

        public void UpdateItem(string name, bool needed)
        {
            var original = items.FirstOrDefault(i => i.Ingrident.Name == name);
            original.Asked = true;
            original.Needed = needed;
        }

        public IEnumerable<Ingrident> GetList() => items.Where(i => i.Needed).Select(i => i.Ingrident);
    }
}
