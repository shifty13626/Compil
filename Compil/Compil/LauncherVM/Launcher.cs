using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Compil.LauncherVM
{
    public class Launcher
    {
        private string pathMsm;
        private string msmProgrammName;

        public Launcher(string pathMsm, string msmProgrammName)
        {
            this.pathMsm = pathMsm;
            this.msmProgrammName = msmProgrammName;
        }

        public void CopyOutFile()
        {
            // Copy out file on folder msm (virtual machine)
            File.Copy(Path.Combine(Environment.CurrentDirectory, "code.txt"),
                Path.Combine(pathMsm, "code.txt"),
                true);
        }

        public void LaunchCodeOnVm()
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine("C:\\Program Files\\WindowsApps\\CanonicalGroupLimited.UbuntuonWindows_1804.2019.521.0_x64__79rhkp1fndgsc", "ubuntu.exe");
            process.StartInfo.WorkingDirectory = this.pathMsm;
            process.StartInfo.Arguments = "./" + msmProgrammName +" code.txt -d > out.txt";
            process.StartInfo.CreateNoWindow = false;
            process.Start();

            Thread.Sleep(1000);
        }

    }
}
