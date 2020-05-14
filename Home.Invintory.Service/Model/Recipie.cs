using System.Collections.Generic;

namespace Home.Invintory.Service.Model
{
    public class Recipie
    {
        public string Name { get; set; }
        IEnumerable<Ingrident> Ingredients { get; set; }
    }
}
