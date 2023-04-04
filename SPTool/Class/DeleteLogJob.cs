using Quartz;
using SPTool.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SPTool
{
    [DisallowConcurrentExecution]
    public class DeleteLogJob : IJob
    {
        static DateTime date1;
        static DateTime date2;
        public void Execute(IJobExecutionContext context)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\" + "log";
            if (Directory.Exists(path))
            {
                date1 = DateTime.Now.AddDays(-30);
                date2 = DateTime.Now.AddDays(-60);
                delete(path);
            }
        }

        private static void delete(String dir)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();//文件
            DirectoryInfo[] directs = d.GetDirectories();//文件夹
            foreach (FileInfo f in files)
            {
                FileInfo fs = new FileInfo(dir + "\\" + f.Name);
                DateTime dates = Convert.ToDateTime(fs.LastWriteTime);
                if (dates <= date1 )
                {
                    fs.Delete();
                }
            }
        }

    }
}
