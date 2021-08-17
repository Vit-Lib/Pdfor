using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Pdfor.Module.Logical;

namespace BIMProduct.Module.Doc.Controllers
{

    [Route("Pdfor")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        string rootDir;
        public PdfController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            rootDir = Path.Combine(environment.WebRootPath,"Temp");
        }

        /// <summary>
        /// 转换office文件(word、excel、ppt)为pdf
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConvertToPdf")]
        [DisableRequestSizeLimit]
        public IActionResult ConvertToPdf([FromForm] IList<IFormFile> files,[FromServices] IPdfConvert pdfConvert)
        {
            if (files == null || files.Count != 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new EmptyResult();
            }


            #region 保存文件并转换为pdf格式
            var oriFilePath = SaveFile(rootDir, files[0],addDateToPath: false);
            string pdfFilePath = oriFilePath + ".pdf";
            try
            {
                if (pdfConvert.ConvertToPdf(oriFilePath, pdfFilePath))               
                {             
                    return File(System.IO.File.ReadAllBytes(pdfFilePath), "application/pdf", Path.ChangeExtension(files[0].FileName, ".pdf"));
                }
            }
            //catch (Exception ex)
            //{  
            //    throw;
            //}
            finally
            {
                try
                {
                    if (System.IO.File.Exists(oriFilePath)) System.IO.File.Delete(oriFilePath);
                }
                catch { }

                try
                {
                    if (System.IO.File.Exists(pdfFilePath)) System.IO.File.Delete(pdfFilePath);
                }
                catch { }
            }

            #endregion

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new EmptyResult();
        }

        #region 保存文件至临时文件夹
        /// <summary>
        ///       wwwroot/Temp/2020-08/12/{HHmmss}_{random}_{FileName}.{txt}
        /// </summary>
        /// <param name="basePath"></param>    
        /// <param name="file"></param>
        /// <param name="addDateToPath"></param>
        /// <returns></returns>
        internal static string SaveFile(string basePath, IFormFile file, bool addDateToPath = true)
        {
            string filePath;

            string directoryPath = basePath;

            if (addDateToPath)
                directoryPath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyy-MM"), DateTime.Now.ToString("dd"));


            Directory.CreateDirectory(directoryPath);

            do
            {
                var fileName = DateTime.Now.ToString("HHmmss") + "_" + new Random().Next(9999).ToString("D4") + "_" + Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);

                filePath = Path.Combine(directoryPath, fileName);

            } while (System.IO.File.Exists(filePath));


            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                file.CopyTo(fs);
            }
            return filePath;
        }


       
        #endregion
    }
}