using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class SortBuffer
    {

        public Block[] BufferBlocks;
        internal readonly long nBlocks;
        
        public SortBuffer(uint nBlocks, uint blockSize)
        {
            this.nBlocks = nBlocks;

            BufferBlocks = new Block[nBlocks];
            for (int i = 0; i < nBlocks; i++)
            {
                BufferBlocks[i] = new Block(blockSize);
            }
        }

        public void loadBlocks(File sourceFile, int startingBlock)
        {
            for (int bufferBlockIndex = 0; bufferBlockIndex < nBlocks; bufferBlockIndex++)
            {
                int fileBlockIndex = startingBlock + bufferBlockIndex;
                Block fileBlock = sourceFile.Blocks[fileBlockIndex];
                Block bufferBlock = BufferBlocks[bufferBlockIndex];

                Block.transferBlock(
                    source: fileBlock,
                    destination: bufferBlock);
            }
        }

        public void writeBack(File destFile, int startingBlock = 0)
        {
            for (int bufferBlockIndex = 0; bufferBlockIndex < nBlocks; bufferBlockIndex++)
            {
                int fileBlockIndex = startingBlock + bufferBlockIndex;
                Block fileBlock = destFile.Blocks[fileBlockIndex];
                Block bufferBlock = BufferBlocks[bufferBlockIndex];

                Block.transferBlock(
                    source: bufferBlock,
                    destination: fileBlock);
            }
        }

        public void Sort()
        {
            //In memory: just functional
            List<uint> sortedData = new List<uint>();
            foreach (var block in BufferBlocks)
            {
                sortedData.AddRange(block.Data);
            }
            sortedData.Sort();

            for (int blockIndex = 0; blockIndex < nBlocks; blockIndex++)
            {
                Block block = BufferBlocks[blockIndex];
                for (int blockElementIndex = 0; blockElementIndex < block.Data.Length; blockElementIndex++)
                {
                    block.Data[blockElementIndex] = sortedData[0];
                    sortedData.RemoveAt(0);
                }
            }
        }
    }
}
