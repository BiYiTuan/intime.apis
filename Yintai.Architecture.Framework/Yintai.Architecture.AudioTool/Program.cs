using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Yintai.Architecture.AudioTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var a = new AutioTools();

            var e = Path.GetFullPath(@"C:\ffmpeg\bin\ffmpeg.exe");
            var i = Path.GetFullPath(@"d:\55384dd8-cef7-4a1b-a631-b715048d05e6.mp3");

            var duration = a.GetLength(e, i);

            if (!String.IsNullOrEmpty(duration))
            {
                var ts = TimeSpan.Parse(duration);

                Console.WriteLine(ts.TotalSeconds);
            }


            Console.ReadLine();
        }
    }

    public class AutioTools
    {
        private static void CallProcess(string exePath, string fileArgs)
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = fileArgs,
                FileName = exePath,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true
            };

            using (var exeProcess = Process.Start(startInfo))//Process.Start(System.IO.Path.Combine(pathImageMagick,appImageMagick),fileArgs))
            {
                exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }

        private static string CallProcessAndReturn(string exePath, string fileArgs)
        {


            var startInfo = new ProcessStartInfo
            {
                Arguments = fileArgs,
                FileName = exePath,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true
            };

            string result;
            using (var exeProcess = Process.Start(startInfo))//Process.Start(System.IO.Path.Combine(pathImageMagick,appImageMagick),fileArgs))
            {
                using (System.IO.StreamReader errorreader = exeProcess.StandardError)
                {
                    result = errorreader.ReadToEnd();
                }

                exeProcess.WaitForExit();
                exeProcess.Close();
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Substring(result.IndexOf("Duration: ", System.StringComparison.Ordinal) + ("Duration: ").Length, ("00:00:00").Length);
            }

            return result;
        }

        public string GetLength(string ex, string fileName)
        {

            //return CallProcessAndReturn("", " -i " + fileName);

            string duration = null;
            using (var pro = new System.Diagnostics.Process())
            {
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.ErrorDialog = false;
                pro.StartInfo.RedirectStandardError = true;

                pro.StartInfo.FileName = ex;
                pro.StartInfo.Arguments = " -i " + fileName;

                pro.Start();
                System.IO.StreamReader errorreader = pro.StandardError;
                pro.WaitForExit();

                string result = errorreader.ReadToEnd();
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Substring(result.IndexOf("Duration: ", System.StringComparison.Ordinal) + ("Duration: ").Length, ("00:00:00").Length);
                    duration = result;
                }

            }

            return duration;
        }
    }



}
