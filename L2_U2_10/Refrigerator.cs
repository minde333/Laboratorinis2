using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2_U2_10
{
    class Refrigerator
    {        
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public string EnergyLabel { get; set; }
        public string InstallationType { get; set; }
        public string Color { get; set; }
        public bool IsThereAFreezer { get; set; }
        public double Price { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }

        public Refrigerator()
        {
        }

        public Refrigerator(string manufacturer, string model, int capacity, string energyLabel, string installationType,
                            string color, bool isThereAFreezer, double price, int height, int width, int depth)
        {
            Manufacturer = manufacturer;
            Model = model;
            Capacity = capacity;
            EnergyLabel = energyLabel;
            InstallationType = installationType;
            Color = color;
            IsThereAFreezer = isThereAFreezer;
            Price = price;
            Height = height;
            Width = width;
            Depth = depth;
        }
        public override String ToString()
        {
            return String.Format("Gamintojas: {0,10} | Modelis: {1,6} | Talpa: {2,3} | Kaina: {3,3} |",
                                 Manufacturer, Model, Capacity, Price);
        }

        public override bool Equals(object obj)
        {
            Refrigerator e = obj as Refrigerator;
            if (e.Manufacturer == Manufacturer && e.Model == Model)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Manufacturer.GetHashCode() ^ Model.GetHashCode();
        }
        public static bool operator ==(Refrigerator lhs, Refrigerator rhs)
        {
            return lhs.Manufacturer == rhs.Manufacturer && lhs.Model == rhs.Model;
        }
        public static bool operator !=(Refrigerator lhs, Refrigerator rhs)
        {
            return lhs.Manufacturer != rhs.Manufacturer && lhs.Model != rhs.Model;
        }

        public static bool operator <=(Refrigerator lhs, Refrigerator rhs)
        {
            return lhs.Price < rhs.Price || lhs.Price == rhs.Price;
        }
        public static bool operator >=(Refrigerator lhs, Refrigerator rhs)
        {
            return lhs.Price > rhs.Price || lhs.Price == rhs.Price;
        }        
    }
}
// reikia 3 dalyku 1. operatoriu perrasymas 2. lentele 3. kito zmogaus programa