using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebReader.Models;

namespace WebReader.Service
{
    public class UserService
    {
        public User CheckLogin(String uid, String passwd)
        {
            User user = null;
            DBService dbService = new DBService();
            String sql = "select * from Users where uid=\"" + uid + "\" and passwd=\"" + passwd + "\"";
            DataTable dt = dbService.Query(sql);
            if (dt.Rows.Count > 0)
                foreach (DataRow dr in dt.Rows)
                    user = new User((String)dr["Uid"], (String)dr["Uname"]);
            dbService.Close();
            return user;
        }

        //处理注册
        public String DealRegister(string uid, string uname, string passwd)
        {
            String result = null;
            DBService dbService = new DBService();
            String sql = "select * from Users where uid='" + uid + "'";
            DataTable dt = dbService.Query(sql);
            if (dt.Rows.Count > 0)
                result = "账号已存在";
            else
            {
                sql = "insert into Users (uid, uname, passwd) values ('" + uid + "', '" + uname + "', '" + passwd + "')";
                if (!dbService.UpdateTable(sql))
                    result = "注册错误";
            }
            dbService.Close();
            return result;
        }
    }
}