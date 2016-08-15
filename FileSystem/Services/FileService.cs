using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using FileSystem.Models;

namespace FileSystem.Services
{
    public static class FileService
    {
        /// <summary>
        /// 10 Mb as specified in task
        /// </summary>
        private static long _smallFileUpperLimit;

        /// <summary>
        /// 50 Mb as specified in task
        /// </summary>
        private static long _mediumFileUpperLimit;

        /// <summary>
        /// 100 Mb as specified in task
        /// </summary>
        private static long _largeFileLowerLimit;

        /// <summary>
        /// static constructor
        /// </summary>
        static FileService()
        {
            long mbInBytes = (long)Math.Pow(1024.0, 2.0);
            _smallFileUpperLimit = 10 * mbInBytes;
            _mediumFileUpperLimit = 50 * mbInBytes;
            _largeFileLowerLimit = 100 * mbInBytes;
        }

        /// <summary>
        /// A method that gets our DirectoryModel for a specified path
        /// </summary>
        /// <param name="path">path of the directory</param>
        /// <returns>DirectoryModel</returns>
        public static DirectoryModel GetDirectoryModel(string path)
        {
            DirectoryModel result = new DirectoryModel();
            result.Path = path;
            if (path == "/") //if we need to see list of the logical drives
            {
                result.Directories = Directory.GetLogicalDrives();
                for (int i=0; i<result.Directories.Length; i++)
                {
                    result.Directories[i] = result.Directories[i].Remove(result.Directories[i].Length-1);
                }
                result.Files = new string[0];
                //we do not calculate files by size contained in ALL logical drives
                //because it would be very inefficient
            }
            else
            {
                result = FileService.CalculateFilesInDirectory(result, path);
                result = FileService.GetDirectoryContentByPath(result, path);
            }
            return result;
        }

        /// <summary>
        /// Supplementary method that calculates amount of files of specified size
        /// </summary>
        /// <param name="dirModel">our DirectoryModel</param>
        /// <param name="path">path of the Directory</param>
        /// <returns></returns>
        public static DirectoryModel CalculateFilesInDirectory(DirectoryModel dirModel, string path)
        {

            var filesInDirectory = FileService.GetDirectoryFiles(path);
            //var filesInDirectory = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
            foreach (string fileName in filesInDirectory)
            {
                long fileLengthInBytes = (new FileInfo(fileName)).Length;
                if (fileLengthInBytes <= _smallFileUpperLimit)
                {
                    dirModel.SmallFilesCounter++;
                    continue;
                }
                if (fileLengthInBytes >= _largeFileLowerLimit)
                {
                    dirModel.LargeFilesCounter++;
                    continue;
                }
                if (fileLengthInBytes > _smallFileUpperLimit && fileLengthInBytes <= _mediumFileUpperLimit)
                {
                    dirModel.MediumFilesCounter++;
                }
            }
            return dirModel;
        }

        /// <summary>
        /// Supplementary method that gets DirectoryContent (directories and files contained in the directory)
        /// </summary>
        /// <param name="dirModel">our DirectoryModel</param>
        /// <param name="path">path of the Directory</param>
        /// <returns></returns>
        public static DirectoryModel GetDirectoryContentByPath(DirectoryModel dirModel, string path)
        {
            dirModel.Directories = Directory.GetDirectories(path);
            for (int i = 0; i < dirModel.Directories.Length; i++)
            {
                dirModel.Directories[i] = dirModel.Directories[i].Remove(0, path.Length);
            }
            dirModel.Files = Directory.GetFiles(path);
            for (int i = 0; i < dirModel.Files.Length; i++)
            {
                dirModel.Files[i] = dirModel.Files[i].Remove(0, path.Length);
            }
            return dirModel;
        }

        /// <summary>
        /// Default Directory.EnumerateFiles method doesn't work with directories that contain read-only subdirectories
        /// That's why I had to implement another recursive method for this purpose
        /// </summary>
        /// <param name="path">path of the directory</param>
        /// <returns>found files in the directory</returns>
        public static IEnumerable<string> GetDirectoryFiles(string path)
        {
            var foundFiles = Enumerable.Empty<string>();
            try
            {
                IEnumerable<string> subDirs = Directory.EnumerateDirectories(path);
                foreach (string dir in subDirs)
                {
                    foundFiles = foundFiles.Concat(GetDirectoryFiles(dir)); // Add files in subdirectories recursively to the list
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (PathTooLongException) { }
            
            try
            {
                foundFiles = foundFiles.Concat(Directory.EnumerateFiles(path)); // Add files from the current directory
            }
            catch (UnauthorizedAccessException) { }
            return foundFiles;
        }

    }
}