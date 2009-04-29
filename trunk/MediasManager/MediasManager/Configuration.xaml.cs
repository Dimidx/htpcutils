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

namespace MediaManager
{
	/// <summary>
	/// Interaction logic for Configuration.xaml
	/// </summary>
	public partial class Configuration : Window
	{

		public Configuration()
		{
			this.InitializeComponent();
			Settings.xmlPath = System.IO.Path.GetDirectoryName(System.Windows.Application.ResourceAssembly.Location) + @"\settings.xml";
            Settings.XML = new Config.XmlSettings();
            
            if (!Settings.Load())
            {
                System.Windows.MessageBox.Show("No valid settings.xml found. Loading defaults");
                //conf.ShowDialog();
            }

            #region Films

            ConfigMovie conf = Settings.XML.Config.confMovie;
            chkSaveFanartJpg.IsChecked = conf.saveFanartJpg;
            chkSaveFolderJpg.IsChecked = conf.saveFolderJpg;
            chkSaveMovieNameFanart.IsChecked = conf.saveMovieNameFanart;
            chkSaveMovieNameTbn.IsChecked = conf.saveMovieNameTbn;
            chkSaveMovieTbn.IsChecked = conf.saveMovieTbn;

            //La liste des paths
            if (conf.MovieFolders != null)
            {
                foreach (MovieFolder p in conf.MovieFolders)
                {
                    lstMoviePaths.Items.Add(p);
                }
            }
            
            #endregion



            //cmbExtensions.Text = "";

            //foreach (String s in conf.extensions)
            //{
            //    cmbExtensions.Text += s + ", ";
            //}
            //// remove last ','
            //if (cmbExtensions.Text.EndsWith(", "))
            //{
            //    cmbExtensions.Text = cmbExtensions.Text.Remove(cmbExtensions.Text.LastIndexOf(","));
            //}

            //if (conf.lastFolder != null) folderPicker.SelectedPath = conf.lastFolder;
            //if (conf.MovieFolders != null)
            //{
            //    listFoldersMovie.Items.Clear();
            //    foreach (MovieFolder p in conf.MovieFolders)
            //    {
            //        listFoldersMovie.Items.Add(new ListViewItemMovie(p));
            //    }
            //}
            //Console.WriteLine("LOAD OK");
            

			// Insert code required on object creation below this point.
		}

        private void btnSupprimerMoviePath_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Voulez-vous vraiment supprimer ce dossier ?", "Congiguration", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (lstMoviePaths.SelectedItem != null)
                {
                    lstMoviePaths.Items.RemoveAt(lstMoviePaths.SelectedIndex);
                }
            }

        }

        private void btnAjouterMoviePath_Click(object sender, RoutedEventArgs e)
        {
            var dlg1 = new Utils.FolderBrowserDialogEx();
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
                lstMoviePaths.Items.Add(fp);
    
            }
        }

        private bool SaveSettings()
        {
            Console.WriteLine("SAVING: " + Settings.xmlPath);
            try
            {
                ConfigMovie conf = Settings.XML.Config.confMovie;

                conf.saveFanartJpg = (bool)chkSaveFanartJpg.IsChecked;
                conf.saveFolderJpg = (bool)chkSaveFolderJpg.IsChecked;
                //conf.savePosterJpg = chkAutoPoster.Checked;

                

                //MovieFolder[] paths = new MovieFolder[lstMoviePaths.Items.Count];
                conf.MovieFolders.Clear();
                for (int i = 0; i < lstMoviePaths.Items.Count; i++)
                {
                    MovieFolder p = ((MovieFolder)lstMoviePaths.Items[i]);
                    conf.MovieFolders.Add(p);
                    //paths[i] = p;
                }
                //conf.MovieFolders = paths;

                Settings.Save();
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