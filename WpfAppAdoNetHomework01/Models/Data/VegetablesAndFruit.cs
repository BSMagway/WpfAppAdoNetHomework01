using System.Data.Common;

namespace WpfAppAdoNetHomework01.Models.Data
{
    public class VegetablesAndFruit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeProduct { get; set; }
        public string Color { get; set; }
        public double Calories { get; set; }

        public VegetablesAndFruit()
        {

        }

        public VegetablesAndFruit(int id, string name, string typeProduct, string color, double calories)
        {
            Id = id;
            Name = name;
            TypeProduct = typeProduct;
            Color = color;
            Calories = calories;
        }

        public override string ToString()
        {
            return $"Id: {Id}\t Name: {Name}\t Type: {TypeProduct}\t Color: {Color}\t Calories: {Calories}";
        }

        public static VegetablesAndFruit GetVegetablesAndFruitr(DbDataReader reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1).Trim();
            string type = reader.GetString(2).Trim();
            string color = reader.GetString(3).Trim();
            double calories = reader.GetDouble(4);
            return new VegetablesAndFruit(id, name, type, color, calories);
        }
    }
}
