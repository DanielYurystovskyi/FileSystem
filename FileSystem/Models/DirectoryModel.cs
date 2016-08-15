using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileSystem.Models
{
    public class DirectoryModel
    {
        /// <summary>
        /// path of the directory
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// counter of files smaller than 10 Mb
        /// </summary>
        public int SmallFilesCounter { get; set; }

        /// <summary>
        /// counter of files between 10Mb and 50Mb
        /// </summary>
        public int MediumFilesCounter { get; set; }

        /// <summary>
        /// counter of files bigger than 100Mb
        /// </summary>
        public int LargeFilesCounter { get; set; }

        /// <summary>
        /// subdirectories
        /// </summary>
        public string[] Directories { get; set; }

        /// <summary>
        /// files in the directory
        /// </summary>
        public string[] Files { get; set; }

    }
}