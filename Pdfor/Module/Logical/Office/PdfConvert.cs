using Microsoft.Office.Core;
using System;
using System.IO;
using Vit.Core.Module.Log;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Word = Microsoft.Office.Interop.Word;

namespace Pdfor.Module.Logical.Office
{
    public class PdfConvert:IPdfConvert
    {
        

        #region ConvertToPdf

        /// <summary>
        /// 转换Office文件(doc、docx、xls、xlsx、ppt、pptx)为pdf文件
        /// </summary>
        /// <param name="officeFilePath"></param>
        /// <param name="pdfFilePath"></param>    
        /// <returns>是否存在目的pdf文件</returns>
        public bool ConvertToPdf(string officeFilePath, string pdfFilePath)
        {
            //File.Copy(officeFilePath, pdfFilePath);
            //return true;

            if (!File.Exists(officeFilePath))
            {
                return false;
            }

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }


            switch (Path.GetExtension(officeFilePath)?.ToLower())
            {
                case ".doc":
                    WordToPDF(officeFilePath, pdfFilePath);
                    return true;
                case ".docx":
                    WordToPDF(officeFilePath, pdfFilePath);
                    return true;
                case ".xls":
                    ExcelToPDF(officeFilePath, pdfFilePath);
                    return true;
                case ".xlsx":
                    ExcelToPDF(officeFilePath, pdfFilePath);
                    return true;
                case ".ppt":
                    PptToPDF(officeFilePath, pdfFilePath);
                    return true;
                case ".pptx":
                    PptToPDF(officeFilePath, pdfFilePath);
                    return true;
                case ".pdf"://pdf不需要重新转化
                    File.Copy(officeFilePath, pdfFilePath);
                    return true;
                default: return false;
            }
        }
        #endregion



        #region CompareWord 
        /// <summary>
        /// 比较前两个word文件的内容，并把比较结果保存到第三个word文件
        /// </summary>
        /// <param name="OriginalWord"></param>
        /// <param name="DestWord"></param>
        /// <param name="differentFile"></param>
        /// <returns></returns>
        public static void CompareWord(string OriginalWord, string DestWord, string differentFile)
        {
            Microsoft.Office.Interop.Word._Application WordApp = null;
            try
            {
                #region (x.1)比较前两个word文件 比较结果为word

                WordApp = new Microsoft.Office.Interop.Word.Application();
                WordApp.Visible = false;
                Object oMissing = System.Reflection.Missing.Value;
                object sPath1 = OriginalWord;
                Word.Document doc1 = WordApp.Documents.Open(ref sPath1, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                object sPath2 = DestWord;
                Word.Document doc2 = WordApp.Documents.Open(ref sPath2, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                Word.Document doc3 = WordApp.CompareDocuments(doc1, doc2);
                #endregion


                #region (x.2)保存比较结果到word文件
                if (true)
                {
                    //Type docsType = WordApp.Documents.GetType();
                    doc3.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                    // 转换格式，另存为
                    Type docType = doc3.GetType();
                    object saveFileName = differentFile;
                    docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc3,
                        new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument });

                    //保存
                    WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                    WordApp = null;
                }
                #endregion



                #region //(x.3)保存比较结果到html文件
                if (false)
                {
                    //Type docsType = WordApp.Documents.GetType();
                    doc3.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                    // 转换格式，另存为
                    Type docType = doc3.GetType();
                    object saveFileName = differentFile;
                    docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc3,
                        new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML });

                    //保存
                    WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                    WordApp = null;
                }
                #endregion


            }
            finally
            {
                if (WordApp != null)
                {
                    WordApp.Quit();
                    WordApp = null;
                }
            }
        }
        #endregion


        #region 弃用 DataTableToExcel

        /// <summary>
        /// 把Excel文档转化为xls
        /// </summary>
        /// <param name="table">数据</param>
        /// <returns></returns>
        private static byte[] DataTableToExcel(System.Data.DataTable table)
        {
            string htmlFileName = Path.GetTempFileName();

            Microsoft.Office.Interop.Excel._Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                Object oMissing = System.Reflection.Missing.Value;

                //Workbook wb = ExcelApp.Workbooks.Open(System.Web.HttpContext.Current.Request.MapPath("temp/1.xlsx"));
                ExcelApp.UserControl = true;
                ExcelApp.DisplayAlerts = false;
                //为true时显示Excel表格
                ExcelApp.Visible = false;


                Excel.Workbook wb = ExcelApp.Workbooks.Add(oMissing);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                //导入数据的行数
                int nRowCount = table.Rows.Count;
                //导入字传的个数
                int nColCount = table.Columns.Count;
                object[,] objData = new object[nRowCount + 1, nColCount];
                for (int i = 0; i < nColCount; i++)
                {
                    objData[0, i] = table.Columns[i].ColumnName;
                }
                for (int i = 0; i < nRowCount; i++)
                {
                    System.Data.DataRow dr = table.Rows[i];
                    for (int j = 0; j < nColCount; j++)
                    {
                        objData[i + 1, j] = dr[j];
                    }
                }

                Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ExcelApp.Cells[1, 1], ExcelApp.Cells[nRowCount + 1, nColCount]);
                r.NumberFormat = "@";
                r.Value2 = objData;
                r.EntireColumn.AutoFit();
                //object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12;
                wb.SaveAs(htmlFileName, oMissing, oMissing, oMissing, oMissing, oMissing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, oMissing, oMissing, oMissing, oMissing, oMissing);

                //wb.SaveAs

                wb.Close(oMissing, oMissing, oMissing);
                return System.IO.File.ReadAllBytes(htmlFileName);

            }
            finally
            {
                ExcelApp.Quit();
                try
                {
                    if (File.Exists(htmlFileName)) File.Delete(htmlFileName);

                }
                catch { }     
            }  
        }
        #endregion


        #region office -> html


        /// <summary>
        /// 把Word文档转化为Html文件
        /// </summary>
        /// <param name="wordFileName">word文件名</param>
        /// <param name="htmlFileName">要保存的html文件名</param>
        /// <returns></returns>
        public static void WordToHtml(string wordFileName, string htmlFileName)
        {
            Word._Application WordApp = new Microsoft.Office.Interop.Word.Application();
            Object oMissing = System.Reflection.Missing.Value;

            try
            {                  
            
                WordApp.Visible = false;
                object filename = wordFileName;
                Microsoft.Office.Interop.Word._Document WordDoc = WordApp.Documents.Open(ref filename, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                // Type wordType = WordApp.GetType();
                // 打开文件
                Type docsType = WordApp.Documents.GetType();
                WordDoc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                // 转换格式，另存为
                Type docType = WordDoc.GetType();
                object saveFileName = htmlFileName;
                docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, WordDoc,
                    new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML });

                //保存
                WordDoc.Save();

                WordDoc.Close(ref oMissing, ref oMissing, ref oMissing);
            }
            finally
            {           
                WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
            }
     
        }


        /// <summary>
        /// 把Excel文档转化为Html文件
        /// </summary>
        /// <param name="excelFileName">Excel文件名</param>
        /// <param name="htmlFileName">要保存的html文件名</param>
        /// <returns></returns>
        public static void ExcelToHtml(string excelFileName, string htmlFileName)
        {
            Object oMissing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel._Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
      
                ExcelApp.UserControl = true;
                ExcelApp.DisplayAlerts = false;
                //ExcelApp.Visible = true;
                object filename = excelFileName;
                Microsoft.Office.Interop.Excel._Workbook ExcelWorkbook = ExcelApp.Workbooks.Open(excelFileName, oMissing
                    , oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                    oMissing, oMissing);

                ExcelWorkbook.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                // 打开文件
                //Type docsType = ExcelApp.Workbooks.GetType();
                //// 转换格式，另存为
                //Type docType = ExcelWorkbook.GetType();
                //object saveFileName = htmlFileName;
                //docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, ExcelWorkbook,
                //    new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML });
                object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
                ExcelWorkbook.SaveAs(htmlFileName, ofmt, oMissing, oMissing, oMissing, oMissing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, oMissing, oMissing, oMissing, oMissing, oMissing);


                //保存
                ExcelWorkbook.Save();
                ExcelWorkbook.Close(oMissing, oMissing, oMissing);
 
            }            
            finally
            {
                ExcelApp.Quit();
            }
         
        }


        /// <summary>
        /// 把Ppt文档转化为Html文件
        /// </summary>
        /// <param name="pptFileName">Ppt文件名</param>
        /// <param name="htmlFileName">要保存的html文件名</param>
        /// <returns></returns>
        public static void PptToHtml(string pptFileName, string htmlFileName)
        {
            Object oMissing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.PowerPoint._Application PptApp = new Microsoft.Office.Interop.PowerPoint.Application();
            Microsoft.Office.Interop.PowerPoint.Presentation PptDoc=null;

            try
            {              
         
                PptDoc = PptApp.Presentations.Open(pptFileName, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
                PptDoc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                PptDoc.SaveAs(htmlFileName, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsHTML, Microsoft.Office.Core.MsoTriState.msoTriStateMixed);

            }
           finally
            {
                PptDoc?.Close();
                PptApp.Quit();
            }
        }
        #endregion


        #region office -> pdf




        /// <summary>
        /// 将word文档转换成PDF格式
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static void WordToPDF(string sourcePath, string targetPath)
        {
       
            Word.WdExportFormat exportFormat = Word.WdExportFormat.wdExportFormatPDF;
            object paramMissing = Type.Missing;
            Word.Application wordApplication = new Word.Application();
            Word.Document wordDocument = null;
            try
            {
                object paramSourceDocPath = sourcePath;
                string paramExportFilePath = targetPath;

                Word.WdExportFormat paramExportFormat = exportFormat;
                bool paramOpenAfterExport = false;
                Word.WdExportOptimizeFor paramExportOptimizeFor =
                        Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
                Word.WdExportRange paramExportRange = Word.WdExportRange.wdExportAllDocument;
                int paramStartPage = 0;
                int paramEndPage = 0;
                Word.WdExportItem paramExportItem = Word.WdExportItem.wdExportDocumentContent;
                bool paramIncludeDocProps = true;
                bool paramKeepIRM = true;
                Word.WdExportCreateBookmarks paramCreateBookmarks =
                        Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
                bool paramDocStructureTags = true;
                bool paramBitmapMissingFonts = true;
                bool paramUseISO19005_1 = false;

                wordDocument = wordApplication.Documents.Open(
                        ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing, ref paramMissing, ref paramMissing,
                        ref paramMissing);

                if (wordDocument != null)
                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
                            paramExportFormat, paramOpenAfterExport,
                            paramExportOptimizeFor, paramExportRange, paramStartPage,
                            paramEndPage, paramExportItem, paramIncludeDocProps,
                            paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                            paramBitmapMissingFonts, paramUseISO19005_1,
                            ref paramMissing);
            
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordDocument = null;
                }

                if (wordApplication != null)
                {
                    wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordApplication = null;
                }

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
      
            }     
        }


        /// <summary>
        /// 将excel文档转换成PDF格式
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static void ExcelToPDF(string sourcePath, string targetPath)
        {
          
            Excel.XlFixedFormatType targetType = Excel.XlFixedFormatType.xlTypePDF;

            object missing = Type.Missing;
            Excel._Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                application = new Excel.Application();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
    
            }            
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }

                //GC.Collect();
                //GC.WaitForPendingFinalizers();           
            }
   
        }

        /// <summary>
        /// 将ppt文档转换成PDF格式
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static void PptToPDF(string sourcePath, string targetPath)
        {     
          
  
            PowerPoint._Application application = null;
            PowerPoint.Presentation persentation = null;
            try
            {
                application = new PowerPoint.Application();
                persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.SaveAs(targetPath, PowerPoint.PpSaveAsFileType.ppSaveAsPDF, Microsoft.Office.Core.MsoTriState.msoTrue);              
            }           
            finally
            {
                if (persentation != null)
                {
                    persentation.Close();
                    persentation = null;
                }

                if (application != null)
                {
                    application.Quit();
                    application = null;
                }

                //GC.Collect();
                //GC.WaitForPendingFinalizers();               
            }
        }
        #endregion
    }
}
