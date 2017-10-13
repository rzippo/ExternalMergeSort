using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class Block
    {
        public uint[] Data;

        public Block(uint size, uint? initializer = null)
        {
            Data = new uint[size];
            Random rng = new Random();
            for (int i = 0; i < size; i++)
            {
                Data[i] = initializer ?? (uint) rng.Next();
            }
        }
    }
}
