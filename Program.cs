using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class Program
    {
        private static int transferCounter = 0;

        static void Main(string[] args)
        {
        }

        public File ExternalMergeSort(File sourceFile, uint bufferSize)
        {
            //Check wether there is enough blocks in the buffer?
           
            //Maybe should initialize later not to waste memory? Do I care?
            File sortedFile = new File(
                nBlocks: sourceFile.nBlocks,
                blockSize: sourceFile.blockSize,
                initializer: 0);

            Buffer buffer = new Buffer(
                nBlocks: bufferSize,
                blockSize: sourceFile.blockSize);

            //Logic here
            int nRuns = (int) Math.Ceiling((decimal) (sourceFile.nBlocks / bufferSize));
            if (nRuns > 1)
            {
                List<File> runFiles = new List<File>();
                for (int runIndex = 0; runIndex < nRuns; runIndex++)
                {
                    loadBuffer(
                        sourceFile: sourceFile,
                        buffer: buffer,
                        startingBlock: (int) (runIndex * bufferSize));

                    sortBuffer(buffer);

                    runFiles.Add(new File(
                        nBlocks: bufferSize,
                        blockSize: sourceFile.blockSize,
                        initializer: 0));

                    writeBackBuffer(
                        destFile: runFiles[runIndex],
                        buffer: buffer);
                }
                
                //How to merge?
            }

            return sortedFile;
        }

        private void writeBackBuffer(File destFile, Buffer buffer, int startingBlock = 0)
        {
            for (int bufferBlockIndex = 0; bufferBlockIndex < buffer.nBlocks; bufferBlockIndex++)
            {
                int fileBlockIndex = startingBlock + bufferBlockIndex;
                Block fileBlock = destFile.Blocks[fileBlockIndex];
                Block bufferBlock = buffer.BufferBlocks[bufferBlockIndex];

                transferBlock(
                    source: bufferBlock,
                    destination: fileBlock);
            }
        }

        private void sortBuffer(Buffer buffer)
        {
            //In memory: just functional
            List<uint> sortedData = new List<uint>();
            foreach (var block in buffer.BufferBlocks)
            {
                sortedData.AddRange(block.Data);
            }
            sortedData.Sort();

            for (int blockIndex = 0; blockIndex < buffer.nBlocks; blockIndex++)
            {
                Block block = buffer.BufferBlocks[blockIndex];
                for (int blockElementIndex = 0; blockElementIndex < block.Data.Length; blockElementIndex++)
                {
                    block.Data[blockElementIndex] = sortedData[0];
                    sortedData.RemoveAt(0);
                }
            }
        }

        private void loadBuffer(File sourceFile, Buffer buffer, int startingBlock)
        {
            for (int bufferBlockIndex = 0; bufferBlockIndex < buffer.nBlocks; bufferBlockIndex++)
            {
                int fileBlockIndex = startingBlock + bufferBlockIndex;
                Block fileBlock = sourceFile.Blocks[fileBlockIndex];
                Block bufferBlock = buffer.BufferBlocks[bufferBlockIndex];

                transferBlock(
                    source: fileBlock, 
                    destination: bufferBlock);
            }
        }

        private void transferBlock(Block source, Block destination)
        {
            if (source == null || destination == null || source.Data.Length != destination.Data.Length)
                throw new Exception("Bad transfer");

            transferCounter++;

            for (int index = 0; index < source.Data.Length; index++)
            {
                destination.Data[index] = source.Data[index];
            }
        }
    }
}
