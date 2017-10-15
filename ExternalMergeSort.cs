using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalMergeSort
{
    partial class Program
    {
        //TODO: add block transfer counting to evaluate performance

        public File ExternalMergeSort(File sourceFile, uint bufferSize)
        {
            //Check wether there is enough blocks in the buffer?

            File sortedFile;
            int nSortRuns = (int) Math.Ceiling((decimal) (sourceFile.nBlocks / bufferSize));
            if (nSortRuns > 1)
            {
                var runFiles = SortPhase(sourceFile, bufferSize, nSortRuns);

                sortedFile = MergePhase(sourceFile, bufferSize, runFiles);
            }
            else
            {
                SortBuffer sortBuffer = new SortBuffer(
                    nBlocks: bufferSize,
                    blockSize: sourceFile.blockSize);

                sortedFile = SortPass(sourceFile, bufferSize, sortBuffer);
            }

            return sortedFile;
        }

        private static File MergePhase(File sourceFile, uint bufferSize, List<File> runFiles)
        {
            File sortedFile;
            MergeBuffer mergeBuffer = new MergeBuffer(
                nBlocks: bufferSize,
                blockSize: sourceFile.blockSize);

            int nMergingSources = (int) bufferSize - 1;
            int nMergeRuns = (int) Math.Ceiling((decimal) (runFiles.Count / nMergingSources));
            while (nMergeRuns > 1)
            {
                List<File> nextPassFiles = new List<File>();
                for (int passIndex = 0; passIndex < nMergeRuns; passIndex++)
                {
                    var filesToMerge = runFiles.GetRange(
                        index: passIndex * nMergingSources,
                        count: nMergingSources
                    );
                    var outFile = MergePass(filesToMerge, mergeBuffer);
                    nextPassFiles.Add(outFile);
                }
                runFiles = nextPassFiles;
                nMergeRuns = (int) Math.Ceiling((decimal) runFiles.Count / nMergingSources);
            }

            sortedFile = MergePass(runFiles, mergeBuffer);
            return sortedFile;
        }

        private static File MergePass(List<File> filesToMerge, MergeBuffer mergeBuffer)
        {
            File outFile = new File(
                nBlocks: (uint) filesToMerge.Sum(file => file.nBlocks),
                blockSize: filesToMerge[0].blockSize,
                initializer: 0);

            for (int bufferIndex = 0; bufferIndex < (int) mergeBuffer.BufferBlocks.Length; bufferIndex++)
            {
                mergeBuffer.BufferBlocks[bufferIndex]
                    .assignInputFile(filesToMerge[bufferIndex]);
            }
            mergeBuffer.OutputBuffer.assignOutputFile(outFile);

            while (mergeBuffer.BufferBlocks.Any(block => block.HasNext))
            {
                var validBlocks = mergeBuffer.BufferBlocks.Where(block => block.HasNext);

                var nextBlock = validBlocks.First(
                    block => block.peekNext == validBlocks.Min(blk => blk.peekNext));

                mergeBuffer.OutputBuffer.writeNext(nextBlock.readNext());
            }
            return outFile;
        }

        private List<File> SortPhase(File sourceFile, uint bufferSize, int nSortRuns)
        {
            SortBuffer sortBuffer = new SortBuffer(
                nBlocks: bufferSize,
                blockSize: sourceFile.blockSize);

            List<File> runFiles = new List<File>();
            for (int runIndex = 0; runIndex < nSortRuns; runIndex++)
            {
                var outFile = SortPass(sourceFile, bufferSize, sortBuffer, runIndex);
                runFiles.Add(outFile);
            }
            return runFiles;
        }

        private File SortPass(File sourceFile, uint bufferSize, SortBuffer sortBuffer, int runIndex = 0)
        {
            sortBuffer.loadBlocks(
                sourceFile: sourceFile,
                startingBlock: (int) (runIndex * bufferSize)
            );

            sortBuffer.Sort();

            File outFile = new File(
                nBlocks: bufferSize,
                blockSize: sourceFile.blockSize,
                initializer: 0);

            sortBuffer.writeBack(outFile);
            return outFile;
        }
    }
}
