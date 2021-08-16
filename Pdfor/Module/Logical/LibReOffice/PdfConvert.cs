
using System;
using System.IO;
using Vit.Core.Module.Log;
using Vit.Core.Util.Shell;

namespace Pdfor.Module.Logical.LibReOffice
{
    public class PdfConvert : IPdfConvert
    {
        static readonly object locker = new object();


        public string fileName = "soffice";

        public string WorkingDirectory = null;


        #region ConvertToPdf

        /// <summary>
        /// 转换Office文件(doc、docx、xls、xlsx、ppt、pptx)为pdf文件
        /// </summary>
        /// <param name="officeFilePath"></param>
        /// <param name="pdfFilePath"></param>    
        /// <returns>是否存在目的pdf文件</returns>
        public bool ConvertToPdf(string officeFilePath, string pdfFilePath)
        {
            if (!File.Exists(officeFilePath))
            {
                return false;
            }

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }

            var outFileDir = pdfFilePath + "_temp";
            var outFilePath = Path.Combine(outFileDir, Path.GetFileNameWithoutExtension(officeFilePath) + ".pdf");

            string output;
            try
            {
                lock (locker)
                {
                    // soffice --headless  --convert-to pdf --outdir "/root/libreoffice" "/root/libreoffice/doc.docx"
                    string arg = "--headless  --convert-to pdf --outdir \"" + outFileDir + "\" \"" + officeFilePath + "\"";
                    //最长转换时间半个小时
                    bool success = OsShell.Shell(fileName, arg, out output, millisecondsOfWait: 30 * 60 * 1000,WorkingDirectory: WorkingDirectory);

                    if (!success|| !File.Exists(outFilePath)) return false;
                }
                new FileInfo(outFilePath).MoveTo(pdfFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            finally
            {
                try
                {
                    Directory.Delete(outFileDir);
                }
                catch { }

            }

        }
        #endregion



    }
}
