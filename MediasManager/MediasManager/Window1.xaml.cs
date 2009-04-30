using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MediaManager;
using MediaManager.Library.NFO;
using System.IO;

namespace MediaManager
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Media media = new Media();
        private Movie MonFilm = null;
        public BackgroundWorker BackWorker = new BackgroundWorker();
        public bool _RechercheTerminée = true;
        public ImageSource PosterSource = null;
        public ImageSource FanartSource = null;


        public Window1()
        {
            InitializeComponent();
            Settings.xmlPath = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\settings.xml";
            Settings.XML = new Config.XmlSettings();
            if (!Settings.Load())
            {
                MessageBox.Show("No valid settings.xml found. Loading defaults");
                //conf.ShowDialog();
            }

            ScanDir();

        }


        /// <summary>
        /// Lance le scan des dossiers pour afficher les 
        /// </summary>
        private void ScanDir()
        {
            media.Movies.OrderBy(m => m.MovieName);

            if (BackWorker.IsBusy == false)
            {
                BackWorker = new BackgroundWorker();
                BackWorker.DoWork += new DoWorkEventHandler(ScanDir_DoWork);
                BackWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanDir_RunWorkerCompleted);
                BackWorker.ProgressChanged += new ProgressChangedEventHandler(BackWorker_ProgressChanged);
                BackWorker.WorkerReportsProgress = true;
                media = new Media();

                media.Movies.Clear();
                this.jauge_progress.Visibility = Visibility.Visible;
                this.lib_BarreEtat.Visibility = Visibility.Visible;
                BackWorker.RunWorkerAsync();
            }
        }

        private void BackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lib_BarreEtat.Text =  "Nombre de films trouvés : " + media.Movies.Count.ToString() + " - " + e.UserState.ToString() ;

        }


        private void ScanDir_DoWork(object sender, DoWorkEventArgs e)
        {
            media.scanMovieDirs(BackWorker);
        }

        private void ScanDir_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //Lance la recherche
            Binding b = new Binding();
            b.Source = media.Movies.OrderBy(m => m.MovieName); //.OrderBy(m => m.MovieName); sert a trier
            listBox_Films.SetBinding(ItemsControl.ItemsSourceProperty, b);
            this.jauge_progress.Visibility = Visibility.Collapsed;
            this.lib_BarreEtat.Visibility = Visibility.Collapsed;

        }

        private void LoadPoster_DoWork(object sender, DoWorkEventArgs e)
        {
            PosterSource = null;
            if (MonFilm.HasPoster)
            {
                PosterSource = new BitmapImage(new Uri(MonFilm.Paths.PosterPath));
            }
        }

        private void LoadPoster_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void listBox_Films_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox_Films.SelectedItem != null)
            {
                MonFilm = (Movie)listBox_Films.SelectedItem;
                MonFilm.updateItem();

                //MonFilm = AllocineHandler.GetFilmDetails(MonFilm.ID, true, true);
                this.DataContext = MonFilm;
				FilmDetails.ImagePoster.Source = null;
				FilmDetails.ImageFanart.Source = null;
                if (MonFilm.HasPoster)
                {
                    FilmDetails.ImagePoster.Source = new BitmapImage(new Uri(MonFilm.Paths.PosterPath));
                }
				if (MonFilm.HasFanart)
                {
                    FilmDetails.ImageFanart.Source = new BitmapImage(new Uri(MonFilm.Paths.FanartPath));
                }


            }
        }

        #region Menu

        private void mnuScan_Click(object sender, RoutedEventArgs e)
        {
            ScanDir();

        }

        private void mnuPreference_Click(object sender, RoutedEventArgs e)
        {
            Configuration conf = new Configuration();

            conf.ShowDialog();
            Settings.Load();
        }


        private void mnuQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        #endregion

    }
}
