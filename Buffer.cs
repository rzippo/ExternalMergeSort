using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class Buffer
    {
        internal class BufferBlock  //For now unused for simplification
        {
            public bool Valid;
            //Add MRU data?

            public Block Block;

            public BufferBlock(uint blockSize)
            {
                Valid = false;
                Block = new Block(blockSize, 0);
            }
        }

        public Block[] BufferBlocks;
        internal readonly long nBlocks;

        public Buffer(uint nBlocks, uint blockSize)
        {
            this.nBlocks = nBlocks;

            BufferBlocks = new Block[nBlocks];
            for (int i = 0; i < nBlocks; i++)
            {
                BufferBlocks[i] = new Block(blockSize);
            }
        }
    }
}
