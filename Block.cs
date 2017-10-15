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
        public int Size => Data.Length;

        public Block(uint size, uint? initializer = null)
        {
            Data = new uint[size];
            Random rng = new Random();
            for (int i = 0; i < size; i++)
            {
                Data[i] = initializer ?? (uint) rng.Next();
            }
        }

        static public void transferBlock(Block source, Block destination)
        {
            if (source == null || destination == null || source.Data.Length != destination.Data.Length)
                throw new Exception("Bad transfer");

            for (int index = 0; index < source.Data.Length; index++)
                destination.Data[index] = source.Data[index];
        }
    }
}
