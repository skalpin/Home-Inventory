namespace Home.Invintory.Service.Model
{
    public class Ingrident
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public Department Department { get; set; }
    }
}