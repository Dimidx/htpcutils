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
using Microsoft.Win32;
using System.Windows.Forms;
using MediaManager.Library;

namespace MediaManager
{
	/// <summary>
	/// Interaction logic for Configuration.xaml
	/// </summary>
	public partial class Fen_Configuration : Window
	{

        public Fen_Configuration()
		{
            
			this.InitializeComponent();

            Master.Settings.xmlPath = System.IO.Path.GetDirectoryName(System.Windows.Application.ResourceAssembly.Location) + @"\settings.xml";
            Master.Settings.XML = new Master.XmlSettings();

            if (!Master.Settings.Load())
            {
                System.Windows.MessageBox.Show("No valid settings.xml found. Loading defaults");
                //conf.ShowDialog();
            }

            this.DataContext = Master.Settings.XML.Config;
            
            
		}

        private void btnSupprimerMoviePath_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Voulez-vous vraiment supprimer ce dossier ?", "Congiguration", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (lstMoviePaths.SelectedItem != null)
                {
					Master.Settings.XML.Config.confMovie.MovieFolders.RemoveAt(lstMoviePaths.SelectedIndex);
					
                }
            }

        }

        private void btnAjouterMoviePath_Click(object sender, RoutedEventArgs e)
        {
            var dlg1 = new Utile.FolderBrowserDialogEx();
            dlg1.Description = "Select a folder to extract to:";
            dlg1.ShowNewFolderButton = true;
            dlg1.ShowEditBox = true;
            dlg1.ShowFullPathInEditBox = true;
            dlg1.RootFolder = System.Environment.SpecialFolder.MyComputer;
         
            // Show the FolderBrowserDialog.
            DialogResult result = dlg1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                MovieFolder fp = new MovieFolder();
                fp.path = dlg1.SelectedPath;
                fp.containsFolders = true;
                Master.Settings.XML.Config.confMovie.MovieFolders.Add(fp);
                //lstMoviePaths.Items.Add(fp);
    
            }
        }

        private bool SaveSettings()
        {
            Console.WriteLine("SAVING: " + Master.Settings.xmlPath);
            try
            {
                Master.Settings.Save();
                MediaManager.Properties.Settings.Default.Save();
                Console.WriteLine("SAVE OK");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            Close();
        }

	}


}