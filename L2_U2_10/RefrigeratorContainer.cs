using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2_U2_10
{
    class RefrigeratorContainer
    {
        private Refrigerator[] Refrigerators;
        public int Count { get; private set; }
        public RefrigeratorContainer(int size)
        {
            Refrigerators = new Refrigerator[size];
            Count = 0;
        }
        public void AddRefrigerator(Refrigerator refrigerators)
        {
            Refrigerators[Count++] = refrigerators;
        }
        public void AddRefrigerator(Refrigerator refrigerators, int index)
        {
            Refrigerators[index] = refrigerators;
        }
        public Refrigerator GetRefrigerator(int index)
        {
            return Refrigerators[index];
        }

    }
}
