using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#region Personal Addition
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        #region Log4Net
        private log4net.ILog DV2SysLogger = log4net.LogManager.GetLogger("DV2");
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tobeWirted"></param>
        public static void LogFileWriter(string tobeWirted)
        {
            string exportPath = "D:\\Takagi_lab\\Projects\\DV2.Net_Graphics_Application\\Logs\\"+ DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string path2 = Environment.CurrentDirectory;
            try
            {
                FileStream fS = new FileStream(exportPath, FileMode.OpenOrCreate);
                StreamWriter sW = new StreamWriter(fS);
                sW.Write(tobeWirted + "\r\n");
                sW.Flush();
                sW.Close();
                sW.Dispose();
                fS.Close();
                fS.Dispose();
            }

            catch (SystemException sye)
            {
                System.Console.WriteLine("ERROR Code 999");
                System.Console.WriteLine("The ERROR comes from Files_CreateClass.cs");
                System.Console.WriteLine(sye.Message);
            }
        }

        /// <summary>  
        /// 日志记录，写入txt文件  
        /// </summary>  
        /// <param name="msg">记录内容</param>  
        /// <returns></returns>  
        public static void AddLog(string msg)
        {
            string saveFolder = "Log";//日志文件保存路径
            string tishiMsg = "";
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, saveFolder);
                if (Directory.Exists(filePath) == false)
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileAbstractPath = filePath + "\\" + fileName + ".txt";
                FileStream fs = new FileStream(fileAbstractPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入     
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                msg = time + "," + msg + System.Environment.NewLine;

                sw.Write(msg);
                //清空缓冲区               
                sw.Flush();
                //关闭流               
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();

                tishiMsg = "写入日志成功";
            }
            catch (Exception ex)
            {
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                tishiMsg = "[" + datetime + "]写入日志出错：" + ex.Message;
            }
        }

        /// <summary>  
        /// 将日志记录换行  
        /// </summary>  
        /// <param name="saveFolder">文件保存的目录</param>  
        /// <param name="rows">换几行</param>  
        public static void AddLine(int rows)
        {
            string saveFolder = "Log";
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, saveFolder);
                if (Directory.Exists(filePath) == false)
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileAbstractPath = filePath + "\\" + fileName + ".txt";
                FileStream fs = new FileStream(fileAbstractPath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入    
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < rows; i++)
                {
                    sb.Append(System.Environment.NewLine);
                }
                string newline = sb.ToString();
                sw.Write(newline);
                //清空缓冲区               
                sw.Flush();
                //关闭流               
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string tishiMsg = "[" + datetime + "]写入日志出错：" + ex.Message;
            }
        }


    }
}
