using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace CGGeosModernUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StartUpdate();
        }
        public async void StartUpdate()
        {
           await  StartDownloadAsync();
        }
        public async Task StartDownloadAsync()
        {
            download_Prog.Visibility = Visibility.Visible;
            AppendText("Starting download of update.");

            var client = new GitHubClient(new ProductHeaderValue("Updatetor"));
            Console.WriteLine("a");
            var releases = await client.Repository.Release.GetAll("cj1705", "CarsonGamesGeos2");
            Console.WriteLine("a");
            var latestRelease = releases[0];
           
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            Console.WriteLine(latestRelease.ZipballUrl);
            webClient.Headers.Add("user-agent", "Anythin1g");
            webClient.DownloadFileAsync(new Uri(latestRelease.ZipballUrl),"latest.zip");
         


        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)

        {
            update_checkmark.IsChecked = true;
            download_Prog.Visibility = Visibility.Hidden;
            AppendText("Finshed Downloading.");
            AppendText("Setting up Build Process.");
            System.IO.Compression.ZipFile.ExtractToDirectory("latest.zip", "latestGEOS");
            string projectpath = " ";
            foreach( var project in Directory.EnumerateDirectories("latestGEOS"))
            {
                projectpath = System.IO.Path.GetFullPath(project); 
            }
            AppendText("Project Path : " + projectpath +"\\CarsonGamesGeos2");
            AppendText("Opening Project Builder");
            new Form1(projectpath +"\\CarsonGamesGeos2").ShowDialog();






        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            download_Prog.Value = e.ProgressPercentage;
        }

        private void building_checkmark_Copy_Checked(object sender, RoutedEventArgs e)
        {

        }
        public void AppendText(string text)
        {
            update_textbox.AppendText("\n" + text);
        }

        private void update_checkmark_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void update_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void building_checkmark_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
