using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using WebReader.Models;

namespace WebReader.Service
{
    public class FileService
    {
        //保存文件
        public bool SaveFile(String title, String path, String description, String tags, String author, bool ispublic)
        {
            DBService dbService = new DBService();
            String sql = "insert into Files (title, path, description, tags, uploadtime, author, visits, ispublic) " +
              " values('" + title + "', '" + path + "', '" + description + "', '" + tags + "', now(), '" + author + "', 0, " + ispublic + ")";
            bool result = dbService.UpdateTable(sql);
            dbService.Close();
            return result;
        }

        //修改下载量
        public bool UpdateDownLoadTime(String path)
        {
            DBService dbService = new DBService();
            bool result = dbService.UpdateTable("update Files set visits = visits + 1 where path = \"" + path + "\"");
            dbService.Close();
            return result;
        }

        //获得所有的文件
        public List<MyFile> GetFiles(String author)
        {
            String sql = null;
            if (null == author)
                sql = "select * from Files where ispublic = true";
            else if (author.Equals("admin"))
                sql = "select * from Files";
            else
                sql = "select * from Files where author = '" + author + "'";
            DBService dbService = new DBService();
            DataTable dt = dbService.Query(sql);
            List<MyFile> list = ToList(dt, dt.Rows.Count);
            dbService.Close();
            return list;
        }

        //根据上传时间排序
        public List<MyFile> GetTimeOrder(String author)
        {
            String sql = null;
            if (null == author)
                sql = "select * from Files where ispublic = true order by uploadtime desc";
            else if (author.Equals("admin"))
                sql = "select * from Files order by uploadtime desc";
            else
                sql = "select * from Files where author = '" + author + "' order by uploadtime desc";
            DBService dbService = new DBService();
            DataTable dt = dbService.Query(sql);
            List<MyFile> list = ToList(dt, 10);
            dbService.Close();
            return list;
        }

        //根据阅读量排序
        public List<MyFile> GetDownloadOrder(String author)
        {
            String sql = null;
            if (null == author)
                sql = "select * from Files where ispublic = true order by visits desc";
            else if (author.Equals("admin"))
                sql = "select * from Files order by visits desc";
            else
                sql = "select * from Files where author = '" + author + "' order by visits desc";
            DBService dbService = new DBService();
            DataTable dt = dbService.Query(sql);
            List<MyFile> list = ToList(dt, 10);
            dbService.Close();
            return list;
        }

        //搜索文件
        public List<MyFile> Search(String keyword, String author)
        {
            String s = null;
            if (null == author)
                s = " ispublic = true and ";
            else if (author != "admin")
                s = " author = '" + author + "' and ";
            String sql = "select * from Files where " + s + " (LCase(title) like '%" +
                               keyword + "%' or LCase(description) like '%" +
                               keyword + "%' or LCase(tags) like '%" +
                               keyword + "%' or LCase(author) like '%" +
                               keyword + "%')";
            keyword = keyword.ToLower();
            DBService dbService = new DBService();
            DataTable dt = dbService.Query(sql);
            List<MyFile> list = ToList(dt, dt.Rows.Count);
            dbService.Close();
            return list;
        }

        //将DataTable转为List<>
        public List<MyFile> ToList(DataTable dt, int len)
        {
            int count = 1;
            List<MyFile> list = new List<MyFile>();
            foreach (DataRow dr in dt.Rows)
            {
                if (count++ > len)
                    break;
                MyFile f = new MyFile();
                f.setID((int)dr["ID"]);
                f.setTitle((string)dr["title"]);
                f.setPath((string)dr["path"]);
                f.setDescription((string)dr["description"]);
                f.setTags((string)dr["tags"]);
                f.setUploadtime(Convert.ToString(dr["uploadtime"]));
                f.setIspublic((bool)dr["ispublic"]);
                f.setAuthor((string)dr["author"]);
                f.setVisits((int)dr["visits"]);
                list.Add(f);
            }
            return list;
        }

        //修改权限
        public String UpdateAccess(int fid, bool access)
        {
            DBService ds = new DBService();
            bool result = ds.UpdateTable("update Files set ispublic = " + access + " where ID = " + fid);
            ds.Close();
            return result ? "修改成功" : "修改失败";
        }

        //删除文件
        public String DeleteFile(int fid)
        {
            String result = "文件不存在";
            DBService ds = new DBService();
            DataTable dt = ds.Query("select * from Files where ID = " + fid);
            List<MyFile> list = ToList(dt, dt.Rows.Count);
            if(list.Count != 0)
            {
                MyFile mf = list[0];
                result = ds.UpdateTable("delete from Files where ID = " + fid) ? "删除成功" : "删除失败";
                String path = AppDomain.CurrentDomain.BaseDirectory + "data/" + mf.getPath();
                if (File.Exists(path))
                    File.Delete(path);
                path = AppDomain.CurrentDomain.BaseDirectory + "dataout/" + Path.GetFileNameWithoutExtension(mf.getPath()) + ".swf";
                if (File.Exists(path))
                    File.Delete(path);
            }
            ds.Close();
            return result;
        }

        //检查文件是否存在
        public bool CheckFileExsit(String path)
        {
            DBService ds = new DBService();
            bool re1 = ds.Query("select * from Files where path = '"+ path + "'").Rows.Count > 0 ? true : false;
            ds.Close();
            return re1 && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "data/" + path);
        }
    }
}