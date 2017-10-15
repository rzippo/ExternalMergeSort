using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class MergeBuffer
    {
        internal class MergeBufferBlock
        {
            public Block Block;

            public File sourceFile;
            public int filePointer;
            public bool HasNext => filePointer < sourceFile.nData;

            public MergeBufferBlock(uint blockSize)
            {
                Block = new Block(blockSize, 0);
            }

            public void assignInputFile(File file)
            {
                sourceFile = file;
                if (HasNext)
                {
                    Block.transferBlock(
                        source: file.Blocks[0],
                        destination: Block
                    );
                }
            }

            public uint peekNext => Block.Data[filePointer % Block.Size];

            public uint readNext()
            {
                uint value = peekNext;
                filePointer++;
                if (filePointer % Block.Size == 0 && HasNext)
                {
                    Block.transferBlock(
                        source: sourceFile.Blocks[
                            (int) Math.Floor((decimal) (filePointer / Block.Size))],
                        destination: Block
                    );
                }

                return value;
            }
        }

        internal class OutputBlock
        {
            public Block Block;
            public File destinationFile;
            public int blockPointer = 0;
            public int fileBlockPointer = 0;

            public OutputBlock(uint blockSize)
            {
                Block = new Block(blockSize, 0);
            }
            
            public void assignOutputFile(File file)
            {
                destinationFile = file;
                blockPointer = 0;
                fileBlockPointer = 0;
            }

            public void writeNext(uint value)
            {
                Block.Data[blockPointer] = value;
                blockPointer = (blockPointer + 1) % Block.Size;
                if (blockPointer == 0)
                {
                    Block.transferBlock(
                        source: Block,
                        destination: destinationFile.Blocks[fileBlockPointer]
                    );
                    fileBlockPointer++;
                }
            }
        }
        
        public MergeBufferBlock[] BufferBlocks;
        public OutputBlock OutputBuffer;
        internal readonly long nBlocks;

        public MergeBuffer(uint nBlocks, uint blockSize)
        {
            this.nBlocks = nBlocks;

            BufferBlocks = new MergeBufferBlock[nBlocks];
            for (int i = 0; i < nBlocks; i++)
            {
                BufferBlocks[i] = new MergeBufferBlock(blockSize);
            }
            
        }
    }
}
