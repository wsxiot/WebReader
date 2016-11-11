using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
namespace flexpaper
{
    public class ConvertUtil
    {
        //注意所有路路径均不能出现空格，包括工程的路径
        string pdf2swftoolpath = HttpContext.Current.Server.MapPath("~/lib/pdf2swf.exe");
        string savepath = HttpContext.Current.Server.MapPath("~/dataout");//保存路径
        string sourcepath = HttpContext.Current.Server.MapPath("~/data");//源文件路径

        public String CanvertFormat(String filename)
        {
            String extension = Path.GetExtension(filename).ToLower();
            switch (extension)
            {
                case ".pdf":
                    return PDFConvertToSWF(filename) ? null : "格式转换失败";
                case ".xls":
                    return XLSConvertToSWF(filename) ? null : "格式转换失败";
                case ".xlsx":
                    return XLSConvertToSWF(filename) ? null : "格式转换失败";
                case ".doc":
                    return DOCConvertToSWF(filename) ? null : "格式转换失败";
                case ".docx":
                    return DOCConvertToSWF(filename) ? null : "格式转换失败";
                case ".ppt":
                    return PPTConvertToSWF(filename) ? null : "格式转换失败";
                case ".pptx":
                    return PPTConvertToSWF(filename) ? null : "格式转换失败";
                default:
                    return "格式不支持";
            }
        }

        public bool PDFConvertToSWF(string fullfilename)
        {
            if (!File.Exists(sourcepath + "\\" + fullfilename))
                return false;
            if (!System.IO.Path.GetExtension(fullfilename).Equals(".pdf"))
                return false;
            string args = " -t " + sourcepath + "\\" + fullfilename + " -o " + savepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".swf";
            Process pc = new Process();
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(pdf2swftoolpath, args);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                pc.StartInfo = psi;
                pc.Start();
                pc.WaitForExit();
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                pc.Close();
                pc.Dispose();
            }
            return true;
        }

        public bool DOCConvertToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
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
                Word.WdExportOptimizeFor paramExportOptimizeFor = Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
                Word.WdExportRange paramExportRange = Word.WdExportRange.wdExportAllDocument;
                int paramStartPage = 0;
                int paramEndPage = 0;
                Word.WdExportItem paramExportItem = Word.WdExportItem.wdExportDocumentContent;
                bool paramIncludeDocProps = true;
                bool paramKeepIRM = true;
                Word.WdExportCreateBookmarks paramCreateBookmarks = Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
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
                result = true;
            }
            catch
            {
                result = false;
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        public bool XLSConvertToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Excel.XlFixedFormatType targetType = Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Excel.Application application = null;
            Excel.Workbook workBook = null;
            try
            {
                application = new Excel.Application();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch
            {
                result = false;
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        public bool PPTConvertToPDF(string sourcePath, string targetPath)
        {
            bool result;
            PowerPoint.PpSaveAsFileType targetFileType = PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            object missing = Type.Missing;
            PowerPoint.Application application = null;
            PowerPoint.Presentation persentation = null;
            try
            {
                application = new PowerPoint.Application();
                persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

                result = true;
            }
            catch
            {
                result = false;
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
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        public bool DOCConvertToSWF(string fullfilename)
        {
            if (!System.IO.Path.GetExtension(fullfilename).Equals(".doc") && !System.IO.Path.GetExtension(fullfilename).Equals(".docx"))
                return false;
            string sour = sourcepath + "\\" + fullfilename;
            string dest = sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf";
            if (!DOCConvertToPDF(sour, dest))
                return false;
            if (!PDFConvertToSWF(System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf"))
                return false;
            if (File.Exists(sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf"))
                File.Delete(sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf");
            return true;
        }

        public bool XLSConvertToSWF(string fullfilename)
        {
            if (!System.IO.Path.GetExtension(fullfilename).Equals(".xls") && !System.IO.Path.GetExtension(fullfilename).Equals(".xlsx"))
                return false;
            string sour = sourcepath + "\\" + fullfilename;
            string dest = sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf";
            if (!XLSConvertToPDF(sour, dest))
                return false;
            if (!PDFConvertToSWF(System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf"))
                return false;
            if (File.Exists(sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf"))
                File.Delete(sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf");
            return true;
        }

        public bool PPTConvertToSWF(string fullfilename)
        {
            if (!System.IO.Path.GetExtension(fullfilename).Equals(".ppt") && !System.IO.Path.GetExtension(fullfilename).Equals(".pptx"))
                return false;
            string sour = sourcepath + "\\" + fullfilename;
            string dest = sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf";
            if (!PPTConvertToPDF(sour, dest))
                return false;
            if (!PDFConvertToSWF(System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf"))
                return false;
            if (File.Exists(sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf"))
                File.Delete(sourcepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullfilename) + ".pdf");
            return true;
        }
    }
}