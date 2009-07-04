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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MediaManager;
using MediaManager.Library;
using MediaManager.Plugins;
using System.IO;

namespace MediaManager
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //private Media media = new Media();

        private Film _MonFilm = new Film();

        //public MovieCollection _Movies = new MovieCollection();
        public BackgroundWorker BackWorker = new BackgroundWorker();
        public BackgroundWorker bwSelect = new BackgroundWorker();
        public bool _RechercheTerminée = true;
        public ImageSource PosterSource = null;
        public ImageSource FanartSource = null;
        public MovieManager MovieManager = new MovieManager();
        public Movie MaMovie = null;
        public Window1()
        {

            Settings.xmlPath = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\settings.xml";
            Settings.XML = new Configuration.XmlSettings();
            if (!Settings.Load())
            {
                //MessageBox.Show("No valid settings.xml found. Loading defaults");
                Settings.Save();
                //conf.ShowDialog();
            }
            InitializeComponent();
            //ucFilmDetails.DataContext = MonFilm;
            //this.DataContext = MonFilm;


            ListCollectionView lcv = new ListCollectionView(MovieManager.Movies);
            lcv.SortDescriptions.Add(new System.ComponentModel.SortDescription("MovieName", System.ComponentModel.ListSortDirection.Ascending));
            listBox_Films.ItemsSource = lcv;
            //this.DataContext = lcv;
            //lcv.Filter = film => ((Film)film).Titre.ToLower().Contains(this.txt_Recherche);
            //ucFilmDetails.DataContext = _MonFilm;
            ScanDir();

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            App.Current.Shutdown();
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
            //ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as ObservableCollection<Movie>;
            //this.lib_BarreEtat.Text = "Nombre de films trouvés : " + _Movies.Count.ToString() + " - " + e.UserState.ToString();

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

                if (bwSelect.IsBusy == true)
                {
                    bwSelect.CancelAsync();
                    while (!bwSelect.CancellationPending)
                    {
                        Thread.Sleep(1);
                    }
                }
                ucFilmDetails.DataContext = null;
                Storyboard FadeIn = (Storyboard)FindResource("FadeIn");
                FadeIn.Begin(this);
                MaMovie = (Movie)listBox_Films.SelectedItem;
                bwSelect = new BackgroundWorker();
                bwSelect.DoWork += new DoWorkEventHandler(bwSelect_DoWork);
                bwSelect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSelect_RunWorkerCompleted);
                bwSelect.WorkerSupportsCancellation = true;
                bwSelect.RunWorkerAsync();

            }

        }

        void bwSelect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //ucFilmDetails.DataContext = _MonFilm;
            Storyboard FadeOut = (Storyboard)FindResource("FadeOut");
            FadeOut.Begin(this);
            bwSelect.Dispose();

        }

        void bwSelect_DoWork(object sender, DoWorkEventArgs e)
        {

            _MonFilm = null;
            _MonFilm = new Film();
            _MonFilm = MaMovie.updateItem();
            if (_MonFilm.Titre == null) _MonFilm.Titre = MaMovie.MovieName;
            System.Threading.Thread thread = new System.Threading.Thread(
    new System.Threading.ThreadStart(
      delegate()
      {
          ucFilmDetails.Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.DataBind,
            new Action(
              delegate()
              {
                  ucFilmDetails.DataContext = _MonFilm;
              }
          ));
      }
  ));

            thread.Start();

        }



        #region Menu

        private void mnuScan_Click(object sender, RoutedEventArgs e)
        {
            ScanDir();

        }

        private void mnuPreference_Click(object sender, RoutedEventArgs e)
        {
            Fen_Configuration conf = new Fen_Configuration();

            conf.ShowDialog();
            Settings.xmlPath = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\settings.xml";
            Settings.XML = new Configuration.XmlSettings();
            Settings.Load();
        }


        private void mnuQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void btnScraper_Click(object sender, RoutedEventArgs e)
        {
            //Movie _mov = (Movie)listBox_Films.SelectedItem;
            ScraperSelect _scraper = new ScraperSelect(_MonFilm);
            _scraper.ShowDialog();
            if (_scraper.FilmValid != null)
            {
                _MonFilm = _scraper.FilmValid;
                if (_MonFilm.Cover != null) _MonFilm.Cover.GetImage();
                if (_MonFilm.Fanart != null) _MonFilm.Fanart.GetImage();
                ucFilmDetails.DataContext = _MonFilm;
            }

        }

        private void txt_Recherche_TextChanged(object sender, TextChangedEventArgs e)
        {

            ListCollectionView lcv = new ListCollectionView(MovieManager.Movies);
            lcv.SortDescriptions.Add(new System.ComponentModel.SortDescription("MovieName", System.ComponentModel.ListSortDirection.Ascending));
            lcv.Filter = film => ((Movie)film).MovieName.ToLower().Contains(this.txt_Recherche.Text);

            listBox_Films.ItemsSource = lcv;
            //this.DataContext = lcv;


        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Movie _movie = (Movie)listBox_Films.SelectedItem;

            foreach (IMMPluginImportExport plug in Settings.PluginsImportExport)
            {
                try
                {
                    plug.Export(_MonFilm, _movie.fileInfo);
                }
                catch (Exception exe)
                {
                    Console.WriteLine("Erreur Export " + plug.Name + Environment.NewLine + exe.Message);
                }

            }
            _MonFilm = _movie.updateItem();

            if (_MonFilm.Cover != null) _MonFilm.Cover.GetImage(true);
            if (_MonFilm.Fanart != null) _MonFilm.Fanart.GetImage(true);

        }

    }



}
