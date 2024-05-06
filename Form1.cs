using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGGeosModernUI
{
    public partial class Form1 : Form
    {
        string path;
        private Thread buildThread;
        public Form1(string path)
        {
            InitializeComponent();
            this.path = path;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            AppendText("Downloading MSBuild");
            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            wc.DownloadFileAsync(new Uri("https://cloud.carsonmedia.net/index.php/s/rypbX8cJYDxw4jD/download/Msbuild.zip"), "MSbuild.zip");

        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            AppendText("Extracting MSBuild");

            System.IO.Compression.ZipFile.ExtractToDirectory("MSbuild.zip", "MSBuild");

            AppendText("MSBuild extracted successfully.");

            buildThread = new Thread(BeginBuild);
            buildThread.Start();

        }

        public void BeginBuild()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("MSBuild/bin/msbuild.exe", path)
            {
                Arguments = $"/p:DeployOnBuild=true /p:PublishDir=finished"
};
            startInfo.WorkingDirectory = path;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += (sender, e) => richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));
            process.ErrorDataReceived += (sender, e) => richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));
        
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
          
        }


        public void AppendText(string text)
        {
            richTextBox1.AppendText("\n" + text);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}