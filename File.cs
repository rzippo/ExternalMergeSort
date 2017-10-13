using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class File
    {
        public Block[] Blocks;

        public File(int nBlocks, int blockSize)
        {
            Blocks = new Block[nBlocks];
            for (int i = 0; i < nBlocks; i++)
            {
                Blocks[i] = new Block(blockSize);
            }
        }
    }
}
