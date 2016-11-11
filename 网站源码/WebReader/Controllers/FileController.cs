using System;
using System.IO;
using System.Web.Mvc;
using WebReader.Service;
using flexpaper;
using System.Web.Configuration;
using WebReader.Models;

namespace WebReader.Controllers
{
    public class FileController : Controller
    {
        // 主页
        public ActionResult Index()
        {
            FileService fs = new FileService();
            ViewData["Files"] = fs.GetFiles(null);
            ViewData["TimerOrder"] = fs.GetTimeOrder(null);
            ViewData["DownloadOrder"] = fs.GetDownloadOrder(null);
            return View();
        }

        //上传文件并保存
        public ActionResult UploadFile()
        {
            if (null == Session["user"])
                return RedirectToAction("Login", "User");
            return View();
        }

        //处理上传文件
        public ActionResult DealUploadFile(String title, String description, String tags, String access)
        {
            String uid = CheckIsLogin();
            if (null == uid)
                return RedirectToAction("Login", "User");
            else
            {
                String result = "上传失败";
                try
                {
                    FileService fs = new FileService();
                    foreach (String upload in Request.Files)
                    {
                        String path = AppDomain.CurrentDomain.BaseDirectory + "data/";
                        String filename = Path.GetFileName(Request.Files[upload].FileName);
                        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        filename = Convert.ToInt64(ts.TotalSeconds).ToString() + Path.GetExtension(filename);
                        Request.Files[upload].SaveAs(Path.Combine(path, filename));
                        String canvert = new ConvertUtil().CanvertFormat(filename);
                        if (canvert != null)//转换出错
                        {
                            result = canvert;
                            break;
                        }
                        else //保存索引信息到数据库 
                        {
                            if (fs.SaveFile(title, filename, description, tags, uid, access.Equals("public")))
                                result = "上传成功";
                        }
                    }
                }
                catch (Exception) { }
                return Content(result);
            }
        }

        //搜索
        public ActionResult Search([Bind(Prefix = "keyword")]String keyword)
        {
            String uid = CheckIsLogin();
            FileService fs = new FileService();
            ViewData["Files"] = fs.Search(keyword, uid);
            if(null == uid)
                return PartialView("Search");
            return PartialView("SearchUser");
        }

        //我的
        public ActionResult Mine()
        {
            String uid = CheckIsLogin();
            if (null == uid)
                return RedirectToAction("Login", "User");
            FileService fs = new FileService();
            ViewData["Files"] = fs.GetFiles(uid);
            return View();
        }

        //下载文件
        public ActionResult DownFile(String filename)
        {
            FileService fservice = new FileService();
            if (!fservice.CheckFileExsit(filename))
                ViewData["DownloadState"] = "文件不存在";
            else
            {
                fservice.UpdateDownLoadTime(filename);
                ViewData["DownloadState"] = "开始下载";
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "data/" + filename, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(filename));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            return View("DownFile");
        }

        //查看文档
        public ActionResult Show(String filename)
        {
            String realPath = AppDomain.CurrentDomain.BaseDirectory + "dataout/" + Path.GetFileNameWithoutExtension(filename) + ".swf";
            String path = "/dataout/" + Path.GetFileNameWithoutExtension(filename) + ".swf";
            if (!System.IO.File.Exists(realPath))
                ViewData["path"] = "文件不存在";
            else
                ViewData["path"] = path;
            return View();
        }

        //修改权限
        public ActionResult UpdateShare(int fileid, bool access)
        {
            if (null == CheckIsLogin())
                return RedirectToAction("Login", "User");
            return Content(new FileService().UpdateAccess(fileid, access));
        }

        //删除文件
        public ActionResult Delete(int fileid)
        {
            if (null == CheckIsLogin())
                return RedirectToAction("Login", "User");
            return Content(new FileService().DeleteFile(fileid));
        }

        [NonAction]
        //检查是否登录
        public String CheckIsLogin()
        {
            User user = (User)Session["user"];
            if (null == user)
                return null;
            return user.GetUid();
        }
    }
}