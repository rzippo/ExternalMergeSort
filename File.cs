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
        public uint nData => nBlocks*blockSize;
        public readonly uint nBlocks;
        public readonly uint blockSize;

        public File(uint nBlocks, uint blockSize, uint? initializer = null)
        {
            this.nBlocks = nBlocks;
            this.blockSize = blockSize;

            Blocks = new Block[nBlocks];
            for (int i = 0; i < nBlocks; i++)
            {
                Blocks[i] = new Block(blockSize, initializer);
            }
        }

        //TODO: add a print function
    }
}
