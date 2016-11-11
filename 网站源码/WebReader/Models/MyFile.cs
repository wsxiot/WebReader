using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebReader.Models
{
    public class MyFile
    {
        private int ID;
        private string title;
        private string path;
        private string description;
        private string tags;
        private string uploadtime;
        private string author;
        private int visits;
        private bool ispublic;

        public MyFile() { }

        public int getID()
        {
            return ID;
        }
        public String getTitle()
        {
            return title;
        }
        public String getPath()
        {
            return path;
        }
        public String getDescription()
        {
            return description;
        }
        public String getTags()
        {
            return tags;
        }
        public String getUploadtime()
        {
            return uploadtime;
        }
        public String getAuthor()
        {
            return author;
        }
        public int getVisits()
        {
            return visits;
        }
        public bool getIspublic()
        {
            return ispublic;
        }
        public void setID(int ID)
        {
            this.ID = ID;
        }
        public void setTitle(String title)
        {
            this.title = title;
        }
        public void setPath(String path)
        {
            this.path = path;
        }
        public void setDescription(String description)
        {
            this.description = description;
        }
        public void setTags(String tags)
        {
            this.tags = tags;
        }
        public void setUploadtime(String uploadtime)
        {
            this.uploadtime = uploadtime;
        }
        public void setAuthor(String author)
        {
            this.author = author;
        }
        public void setVisits(int visits)
        {
            this.visits = visits;
        }
        public void setIspublic(bool ispublic)
        {
            this.ispublic = ispublic;
        }
    }
}