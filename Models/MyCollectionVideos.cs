using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.Models
{
    public class MyCollectionVideos
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int videoid { get; set; }
        public int videocategroyid { get; set; }
        public string videocategroy { get; set; }
        public string videotitile { get; set; }
        public string videodes { get; set; }
        public string videoimageurl { get; set; }
        public decimal videoprice { get; set; }
        public int videolecturerid { get; set; }
        public string videolecturer { get; set; }
        public string issign { get; set; }
        public int watchcounts { get; set; }
        public int mywatchcounts { get; set; }


        //        ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        //UserId INT NOT NULL,
        // VideoId INT NOT NULL,
        // VideoCategroy VARCHAR(100) NULL,
        // VideoTitile VARCHAR(100) NULL,    
        // VidelDes VARCHAR(200) NULL,
        // VideoIamgeUrl VARCHAR(200) NULL,
        // VideoPrice DECIMAL(16,2) DEFAULT(0) NOT NULL,
        // Videolecturer VARCHAR(50) NULL,
        // IsSign VARCHAR(10) DEFAULT(0) NOT NULL,
        // WatchCounts INT DEFAULT(0) NOT NULL,
        // MyWatchCouns INT DEFAULT(0) NOT NULL,
        // DeleteLogic BIT DEFAULT(0) NOT NULL,
        // CreateTime DATETIME DEFAULT(GETDATE()) NOT NULL
    }
}