using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    class Program
    {
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

            return sortedFile;
        }
    }
}
