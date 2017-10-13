﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class Buffer
    {
        internal class BufferBlock
        {
            public bool Valid;
            //Add MRU data?

            public Block Block;

            public BufferBlock(int blockSize)
            {
                Valid = false;
                Block = new Block(blockSize, 0);
            }
        }

        public BufferBlock[] BufferBlocks;

        public Buffer(int nBlocks, int blockSize)
        {
            BufferBlocks = new BufferBlock[nBlocks];
            for (int i = 0; i < nBlocks; i++)
            {
                BufferBlocks[i] = new BufferBlock(blockSize);
            }
        }
    }
}
