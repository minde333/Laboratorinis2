using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2_U2_10
{
    class Branch
    {
        public const int MaxNumberOfRefrigerator = 100;

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public RefrigeratorContainer Refrigerators { get; private set; }

        public Branch(string name, string address, string phone)
        {
            Name = name;
            Address = address;
            Phone = phone;
            Refrigerators = new RefrigeratorContainer(MaxNumberOfRefrigerator);
        }
    }
}
