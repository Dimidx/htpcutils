using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using MediaManager;
using MediaManager.Plugins;
using System.IO;
using System.Reflection;

namespace MediaManager
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    /// 
    public partial class Splash : Window
    {

        public MessageSplash message = new MessageSplash();

        public Splash()
        {
            InitializeComponent();
            this.DataContext = message;
            // Insert code required on object creation below this point.
        }

        private void SplashLoaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(Object state)
            {
                LongOperationExecution();
            }), null);
            
        }

        private void LongOperationExecution()
        {
            // Here, put the code which take a long time to execute
            // BEGIN

            #region Scrapers
            message.Message = "Chargement des scraper...";
            Assembly PluginFile;
            IMMPluginScraper ScraperPlugin;
            DirectoryInfo DI = new DirectoryInfo(Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "Plugins/Scraper");
            try
            {
                FileInfo[] FIA = DI.GetFiles("*.dll");
                foreach (FileInfo ScraperFile in FIA)
                {
                    message.Message = "Chargement du Scraper " + ScraperFile.Name;
                    PluginFile = Assembly.LoadFrom(ScraperFile.FullName);

                    ScraperPlugin = PluginFile.CreateInstance("MediaManager.Plugins." + ScraperFile.Name.Substring(0
                                                             , ScraperFile.Name.Length - 4)) as IMMPluginScraper;
                    if (ScraperPlugin != null)
                    {
                        Settings.PluginsScraper.Add(ScraperPlugin);

                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            } 
            #endregion

            #region InportExport
            message.Message = "Chargement des plugins Import Export...";
            IMMPluginImportExport ImportExportPlugin;
            DirectoryInfo DIR = new DirectoryInfo(Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "Plugins/ImportExport");
            try
            {
                FileInfo[] FIA = DIR.GetFiles("*.dll");
                foreach (FileInfo InportExport in FIA)
                {
                    message.Message = "Chargement du Plugin " + InportExport.Name;
                    PluginFile = Assembly.LoadFrom(InportExport.FullName);

                    ImportExportPlugin = PluginFile.CreateInstance("MediaManager.Plugins." + InportExport.Name.Substring(0
                                                             , InportExport.Name.Length - 4)) as IMMPluginImportExport;
                    if (ImportExportPlugin != null)
                    {
                        Settings.PluginsImportExport.Add(ImportExportPlugin);

                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
            #endregion

            message.Message = "Création du cache...";
            if (!Directory.Exists(System.Environment.CurrentDirectory + @"\Cache\Images\")) Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\Cache\Images\");
            message.Message = "Chargement Terminé !";

            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(Object state)
            {

                Thread.Sleep(2000);
                this.Hide();

                Window2 window = new Window2();
                window.Show();

                return null;
            }), null);
        }
    }

    public class MessageSplash : INotifyPropertyChanged
    {
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { _Message = value; OnPropertyChanged("Message"); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

}