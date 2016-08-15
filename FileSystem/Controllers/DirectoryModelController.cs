using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using FileSystem.Models;
using FileSystem.Services;


namespace FileSystem.Controllers
{
    public class DirectoryModelController : ApiController
    {
        //GET /api/directorymodel
        public DirectoryModel GetDirectoryModel()
        {
            return FileService.GetDirectoryModel("/");
        }

        //GET /api/DirectoryModel/?path={path}
        public DirectoryModel GetDirectoryModel(string path)
        {
            return FileService.GetDirectoryModel(path);
        }
    }
}
