using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiFileUpload.API.Infrastructure;
using System.Configuration;

namespace MIApi.Controllers
{
    public class FileUploadController : ApiController
    {
        [MimeMultipart]
        public async Task<FileUploadResult> Post()
        {

            string strDirectoryName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DicomName"]);
            
            var context = HttpContext.Current.Request;
            string FullPathWithName = string.Empty;
            string EligibilityReviewPath = string.Empty;
            string MarkPath = string.Empty;
            string strOnlyFileName = string.Empty;
            string RequestedFilePath = string.Empty;

            string[] RequestedFilePathArray;

            var file = context.Files.AllKeys.FirstOrDefault();
            RequestedFilePathArray = file.Split('$');

            RequestedFilePath = Convert.ToString(RequestedFilePathArray[0]);
            file = Convert.ToString(RequestedFilePathArray[0]);

            FullPathWithName = strDirectoryName + Convert.ToString(file);
            strOnlyFileName = System.IO.Path.GetFileName(file);
            string strOnlyDirectory = System.IO.Path.GetDirectoryName(file).Replace("\\", "/");
            strDirectoryName += Convert.ToString(strOnlyDirectory);

            FullPathWithName = HttpContext.Current.Server.MapPath(FullPathWithName);
            if (File.Exists(FullPathWithName))
            {
                return new FileUploadResult
                {
                    LocalFilePath = "Exists",

                    FileName = Path.GetFileName("Exists"),

                    FileLength = 1111
                };
            }

            var uploadPath1 = HttpContext.Current.Server.MapPath(strDirectoryName);
            if (!Directory.Exists(uploadPath1))
            {
                Directory.CreateDirectory(uploadPath1);
            }

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath1);
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            if (Convert.ToString(RequestedFilePathArray[1]) != "Null")
            {
                string EligibilityReviewId = Convert.ToString(RequestedFilePathArray[1]);
                string MarkId = Convert.ToString(RequestedFilePathArray[2]);

                string[] FilePathArry = RequestedFilePath.Split('/');
                EligibilityReviewPath = "/" + Convert.ToString(FilePathArry[1]) + "/" + Convert.ToString(FilePathArry[2]) + "/" + EligibilityReviewId + "/" + Convert.ToString(FilePathArry[4]) + "/Uploaded/";   //+ strOnlyFileName;
                MarkPath = "/" + Convert.ToString(FilePathArry[1]) + "/" + Convert.ToString(FilePathArry[2]) + "/" + MarkId + "/" + Convert.ToString(FilePathArry[4]) + "/Uploaded/"; //+ strOnlyFileName;

                strDirectoryName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DicomName"]);
                EligibilityReviewPath = HttpContext.Current.Server.MapPath(strDirectoryName + EligibilityReviewPath);
                MarkPath = HttpContext.Current.Server.MapPath(strDirectoryName + MarkPath);

                if (!Directory.Exists(EligibilityReviewPath))
                {
                    Directory.CreateDirectory(EligibilityReviewPath);
                }
                if (!Directory.Exists(MarkPath))
                {
                    Directory.CreateDirectory(MarkPath);
                }
                await Task.Run(() =>
                {
                    File.Copy(FullPathWithName, EligibilityReviewPath + "\\" + strOnlyFileName, true);
                    File.Copy(FullPathWithName, MarkPath + "\\" + strOnlyFileName, true);
                });
            }

            string _localFileName = multipartFormDataStreamProvider.FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
            return new FileUploadResult
            {
                LocalFilePath = _localFileName,

                FileName = Path.GetFileName(_localFileName),

                FileLength = new FileInfo(_localFileName).Length
            };
        }
    }
}
