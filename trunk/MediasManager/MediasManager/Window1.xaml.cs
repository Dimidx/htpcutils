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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MediaManager;
using MediaManager.Library;
using MediaManager.Library.NFO;
using System.IO;

namespace MediaManager
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //private Media media = new Media();
        private Film MonFilm = null;
        //public MovieCollection _Movies = new MovieCollection();
        public BackgroundWorker BackWorker = new BackgroundWorker();
        public bool _RechercheTerminée = true;
        public ImageSource PosterSource = null;
        public ImageSource FanartSource = null;
        public MovieManager MovieManager = new MovieManager();

        public Window1()
        {

            Settings.xmlPath = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\settings.xml";
            Settings.XML = new Config.XmlSettings();
            if (!Settings.Load())
            {
                MessageBox.Show("No valid settings.xml found. Loading defaults");
                Settings.Save();
                //conf.ShowDialog();
            }

            InitializeComponent();



            ListCollectionView lcv = new ListCollectionView(MovieManager.Movies);
            lcv.SortDescriptions.Add(new System.ComponentModel.SortDescription("MovieName", System.ComponentModel.ListSortDirection.Ascending));
            listBox_Films.ItemsSource = lcv;
            this.DataContext = lcv;
            ScanDir();

        }


        /// <summary>
        /// Lance le scan des dossiers pour afficher les 
        /// </summary>
        private void ScanDir()
        {
            MovieManager.Movies.OrderBy(m => m.MovieName);

            if (BackWorker.IsBusy == false)
            {
                BackWorker = new BackgroundWorker();
                BackWorker.DoWork += new DoWorkEventHandler(ScanDir_DoWork);
                BackWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanDir_RunWorkerCompleted);
                BackWorker.ProgressChanged += new ProgressChangedEventHandler(BackWorker_ProgressChanged);
                BackWorker.WorkerReportsProgress = true;
                //ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as MovieCollection;
                //_Movies.Clear();
                this.jauge_progress.Visibility = Visibility.Visible;
                this.lib_BarreEtat.Visibility = Visibility.Visible;
                BackWorker.RunWorkerAsync();
            }
        }

        private void BackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as ObservableCollection<Movie>;
            this.lib_BarreEtat.Text = "Nombre de films trouvés : " + _Movies.Count.ToString() + " - " + e.UserState.ToString();

        }


        private void ScanDir_DoWork(object sender, DoWorkEventArgs e)
        {
            //ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as ObservableCollection<Movie>;
            MovieManager.scanMovieDirs();
            //_Movies.scanMovieDirs();
        }

        private void ScanDir_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.jauge_progress.Visibility = Visibility.Collapsed;
            this.lib_BarreEtat.Visibility = Visibility.Collapsed;

        }



        private void listBox_Films_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (listBox_Films.SelectedItem != null)
            {
                Movie _mov = (Movie)listBox_Films.SelectedItem;
                _mov.updateItem();
                MonFilm = new Film();
                MonFilm.Titre = _mov.Nfo.Title;
                MonFilm.TitreOriginal = _mov.Nfo.Title;
                MonFilm.Annee = _mov.Nfo.Year;
                MonFilm.Avis = _mov.Nfo.Mpaa;
                MonFilm.Critique = _mov.Nfo.Outline;
                
                #region Date de sortie
		        try
                {
                    MonFilm.DateSortie = DateTime.Parse(_mov.Nfo.Premiered);
                }
                catch { } 
	            #endregion

                MonFilm.DureeChaine = _mov.Nfo.Runtime;
                MonFilm.Genres = _mov.Nfo.Genre.Split('/');
                MonFilm.ID = _mov.Nfo.Id;

                #region Note
                try
                {
                    MonFilm.NotePresse = Convert.ToInt32(_mov.Nfo.Rating);
                }
                catch { } 
                #endregion

                MonFilm.Synopsis = _mov.Nfo.Plot;

                #region Réalisateurs
                foreach (string dir in _mov.Nfo.Director.Split('/'))
                {
                    MonFilm.Realisateurs.Add(new Personne(dir, "Réalisateur"));
                } 
                #endregion

                #region Acteurs
                foreach (Actor act in _mov.Nfo.Actor)
                {
                    MonFilm.Acteurs.Add(new Personne(act.Name, act.Role));
                }
                #endregion

                //FilmDe_mov.Paths.FanartPath
                Thumb t = new Thumb();
                t.URLImage = _mov.Paths.PosterPath;
                
                MonFilm.ListeCover.Add(t);

                Thumb f = new Thumb();
                f.URLImage = _mov.Paths.FanartPath;
                MonFilm.ListeFanart.Add(f);

                FilmDetails.DataContext = MonFilm;
                FilmDetails.Affiche.Source = MonFilm.Cover.URLImage;
                FilmDetails.Fanart.Source = MonFilm.Fanart.URLImage;


                //MonFilm = AllocineHandler.GetFilmDetails(MonFilm.ID, true, true);
                //FilmDetails.DataContext = MonFilm;
                //FilmDetails.ImagePoster.Source = null;
                //FilmDetails.ImageFanart.Source = null;
                //if (MonFilm.HasPoster)
                //{
                //    FilmDetails.ImagePoster.Source = new BitmapImage(new Uri(MonFilm.Paths.PosterPath));
                //}
                //if (MonFilm.HasFanart)
                //{
                //    FilmDetails.ImageFanart.Source = new BitmapImage(new Uri(MonFilm.Paths.FanartPath));
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
            Settings.xmlPath = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\settings.xml";
            Settings.XML = new Config.XmlSettings();
            Settings.Load();
        }


        private void mnuQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void mnuEditer_Click(object sender, RoutedEventArgs e)
        {
            Movie _movie = (Movie)listBox_Films.SelectedItem;
            _movie.MovieName = "Aviator2";
            NfoFile.saveNfoMovie(_movie.Nfo, _movie.Paths.NfoPath);

        }

        private void btnScraper_Click(object sender, RoutedEventArgs e)
        {
            Movie _movie = (Movie)listBox_Films.SelectedItem;
            Film _Film = new Film();
            _Film.Titre = _movie.MovieName;
            _Film.AlloID = _movie.Nfo.AlloId;

            ScraperSelect _scraper = new ScraperSelect(_Film);
            _scraper.ShowDialog();

        }

    }
}
