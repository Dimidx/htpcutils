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
        public BackgroundWorker bwScanDossier = new BackgroundWorker();
        public BackgroundWorker bwSelect = new BackgroundWorker();
        public BackgroundWorker bwScrapeAll = new BackgroundWorker();

        public bool _RechercheTerminée = true;
        public ImageSource PosterSource = null;
        public ImageSource FanartSource = null;
        public MovieManager MovieManager = new MovieManager();
        public Movie MaMovie = null;

        public IMMPluginScraper Scraper = null;
        public ObservableCollection<Utils.ChampModifiable> ListeChampReplace = new ObservableCollection<Utils.ChampModifiable>();


        public Window1()
        {


            InitializeComponent();
            ListCollectionView lcv = new ListCollectionView(MovieManager.Movies);
            lcv.SortDescriptions.Add(new System.ComponentModel.SortDescription("MovieName", System.ComponentModel.ListSortDirection.Ascending));
            listBox_Films.ItemsSource = lcv;

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

            if (bwScanDossier.IsBusy == false)
            {

                bwScanDossier = new BackgroundWorker();
                bwScanDossier.DoWork += new DoWorkEventHandler(ScanDir_DoWork);
                bwScanDossier.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanDir_RunWorkerCompleted);
                bwScanDossier.ProgressChanged += new ProgressChangedEventHandler(bwScanDossier_ProgressChanged);
                bwScanDossier.WorkerReportsProgress = true;
                bwScanDossier.WorkerSupportsCancellation = true;
                //ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as MovieCollection;
                //_Movies.Clear();
                GridPatientez.Visibility = Visibility.Visible;
                bwScanDossier.RunWorkerAsync();
            }
        }

        private void bwScanDossier_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as ObservableCollection<Movie>;
            //this.lib_BarreEtat.Text = "Nombre de films trouvés : " + _Movies.Count.ToString() + " - " + e.UserState.ToString();

        }


        private void ScanDir_DoWork(object sender, DoWorkEventArgs e)
        {
            MovieManager.scanMovieDirs();
        }

        private void ScanDir_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            GridPatientez.Visibility = Visibility.Collapsed;

        }


        private void listBox_Films_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _MonFilm = null;
            if (listBox_Films.SelectedItem != null)
            {

                if (bwSelect.IsBusy == true)
                {
                    Console.WriteLine("Attente bw select");
                    bwSelect.CancelAsync();
                    while (!bwSelect.CancellationPending)
                    {
                        Console.WriteLine("Attente bw select");
                        Thread.Sleep(1);
                    }
                }
                bwSelect.Dispose();

                dpInfosFilm.DataContext = null;
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
                  dpInfosFilm.DataContext = _MonFilm;
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
            Master.Settings.xmlPath = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\settings.xml";
            Master.Settings.XML = new Master.XmlSettings();
            Master.Settings.Load();
        }


        private void mnuQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void btnScraper_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Films.SelectedItem != null)
            {
                //dpInfosFilm.DataContext = null;
                ScraperSelect _scraper = new ScraperSelect(_MonFilm);
                try
                {
                    //Movie _mov = (Movie)listBox_Films.SelectedItem;

                    _scraper.ShowDialog();
                    if (_scraper.FilmValid != null)
                    {
                        _MonFilm = _scraper.FilmValid;
                        if (_MonFilm.Cover != null) _MonFilm.Cover.GetImage();
                        if (_MonFilm.Fanart != null) _MonFilm.Fanart.GetImage();
                        dpInfosFilm.DataContext = _MonFilm;
                    }
                }
                catch (Exception de)
                {
                    Console.WriteLine("Scrap erreur" + de.Message + " " + de.Source);
                    _scraper.Close();
                    dpInfosFilm.DataContext = _MonFilm;
                    //throw;
                }
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
            if (listBox_Films.SelectedItem != null)
            {
                dpInfosFilm.DataContext = null;
                Movie _movie = (Movie)listBox_Films.SelectedItem;

                foreach (IMMPluginImportExport plug in Master.Settings.PluginsImportExport)
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
                dpInfosFilm.DataContext = _MonFilm;
                //Database.Database.SaveFilmToDB(_MonFilm,_mov);

            }
        }

        private void mnuToutScrape_Click(object sender, RoutedEventArgs e)
        {
            if (bwScrapeAll.IsBusy == false)
            {
                //Charge les scrapers
                cbScraper.ItemsSource = Master.Settings.PluginsScraper;
                //Charge la liste des champs modifiables
                GridConfigScraper.Visibility = Visibility.Visible;
                ListeChampReplace = Utils.GetChampsModifiables(new Film());
                ListCollectionView lcv = new ListCollectionView(ListeChampReplace);
                lcv.SortDescriptions.Add(new System.ComponentModel.SortDescription("NomChamp", System.ComponentModel.ListSortDirection.Ascending));
                lstChamps.ItemsSource = lcv;

            }
        }

        void bwScrapeAll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            lib_BarreEtat.Text = e.UserState.ToString();
            jauge_progress.Value = e.ProgressPercentage;
        }

        void bwScrapeAll_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GridPatientez.Visibility = Visibility.Collapsed;
        }

        void bwScrapeAll_DoWork(object sender, DoWorkEventArgs e)
        {
            //Movie _movie = (Movie)listBox_Films.SelectedItem;

            int i = 0; int _progress = 0; int _nbrfilm = MovieManager.Movies.Count;
            foreach (Movie item in MovieManager.Movies)
            {
                if (bwScrapeAll.CancellationPending) goto Cancel;
                i += 1;
                _progress = (int)((float)i / (float)_nbrfilm * (float)100);
                if (!bwScrapeAll.CancellationPending) bwScrapeAll.ReportProgress((int)_progress, "Recherche " + item.MovieName);

                //Le film a scraper
                Film _ScrapeFilm = new Film();
                _ScrapeFilm = item.updateItem();
                if (String.IsNullOrEmpty(_ScrapeFilm.Titre)) _ScrapeFilm.Titre = item.MovieName;
                ObservableCollection<Utils.ChampModifiable> _ListeChampsModif = new ObservableCollection<Utils.ChampModifiable>();
                _ListeChampsModif = Utils.GetChampsModifiables(_ScrapeFilm);

                //Teste si il y a des champs à mettre à jour
                bool _MAJAfaire = false;

                foreach (Utils.ChampModifiable c in ListeChampReplace)
                {
                    if (c.IsModifiable)
                    { //Le champ est paramétré comme modifiable
                        foreach (Utils.ChampModifiable ChampOriginal in _ListeChampsModif)
                        {
                            if (ChampOriginal.NomChamp == c.NomChamp) //On cherche le champ original pour savoir si il est null
                            {
                                if (ChampOriginal.IsModifiable) _MAJAfaire = true;
                            }
                        }
                    }
                }
                if (_MAJAfaire)
                {

                    List<Film> _ListResult = new List<Film>();
                    _ListResult = Scraper.SearchMovie(_ScrapeFilm);

                    if (_ListResult != null & _ListResult.Count > 0)
                    {
                        if (!bwScrapeAll.CancellationPending) bwScrapeAll.ReportProgress((int)_progress, "Charge les détails de " + _ListResult[0].Titre);
                        Film _ScrapeFilmResult = new Film(); //Le film resultat du scraper
                        _ScrapeFilmResult = Scraper.GetMovie(_ListResult[0]);
                        if (_ScrapeFilmResult != null)
                        {
                            foreach (Utils.ChampModifiable c in ListeChampReplace)
                            {

                                if (c.IsModifiable)
                                { //Le champ est paramétré comme modifiable
                                    foreach (Utils.ChampModifiable ChampOriginal in _ListeChampsModif)
                                    {
                                        if (ChampOriginal.NomChamp == c.NomChamp) //On cherche le champ original pour savoir si il est null
                                        {
                                            if (ChampOriginal.IsModifiable) //Il est null on le remplace par le champ du scrapper
                                            {
                                                c.PropertyInfo.SetValue(_ScrapeFilmResult, c.PropertyInfo.GetValue(_ScrapeFilmResult, null), null);
                                            }
                                            else //Il est déjà renseigné on garde tel quel
                                            {
                                                c.PropertyInfo.SetValue(_ScrapeFilmResult, c.PropertyInfo.GetValue(_ScrapeFilm, null), null);
                                            }

                                        }
                                    }


                                }
                                else
                                {
                                    c.PropertyInfo.SetValue(_ScrapeFilmResult, c.PropertyInfo.GetValue(_ScrapeFilm, null), null);

                                }

                            }

                            foreach (IMMPluginImportExport plug in Master.Settings.PluginsImportExport)
                            {
                                try
                                {
                                    if (!bwScrapeAll.CancellationPending) bwScrapeAll.ReportProgress((int)_progress, "Export " + plug.Name + " " + item.MovieName);
                                    _ScrapeFilmResult.Cover.GetImage(true);
                                    _ScrapeFilmResult.Fanart.GetImage(true);

                                    while (_ScrapeFilmResult.Cover.IsLoading)
                                    {
                                        Thread.Sleep(1);
                                    }
                                    while (_ScrapeFilmResult.Fanart.IsLoading)
                                    {
                                        Thread.Sleep(1);
                                    }

                                    plug.Export(_ScrapeFilmResult, item.fileInfo);
                                }
                                catch (Exception exe)
                                {
                                    Console.WriteLine("Erreur Export " + plug.Name + Environment.NewLine + exe.Message);
                                }

                            }
                            _ScrapeFilm = item.updateItem();

                            if (_ScrapeFilm.Cover != null) _ScrapeFilm.Cover.GetImage(true);
                            if (_ScrapeFilm.Fanart != null) _ScrapeFilm.Fanart.GetImage(true);
                        }
                    }



                }
            }
        Cancel:
            {
            }
        }

        private void btn_ScrapeAllOK_Click(object sender, RoutedEventArgs e)
        {
            if (bwScrapeAll.IsBusy == false)
            {
                bwScrapeAll = new BackgroundWorker();
                bwScrapeAll.DoWork += new DoWorkEventHandler(bwScrapeAll_DoWork);
                bwScrapeAll.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwScrapeAll_RunWorkerCompleted);
                bwScrapeAll.ProgressChanged += new ProgressChangedEventHandler(bwScrapeAll_ProgressChanged);
                bwScrapeAll.WorkerReportsProgress = true;
                bwScrapeAll.WorkerSupportsCancellation = true;
                //ObservableCollection<Movie> _Movies = this.Resources["MovieCollectionDataSource"] as MovieCollection;
                //_Movies.Clear();
                GridConfigScraper.Visibility = Visibility.Collapsed;
                GridPatientez.Visibility = Visibility.Visible;
                jauge_progress.IsIndeterminate = false;
                jauge_progress.Maximum = 100;

                Scraper = (IMMPluginScraper)cbScraper.SelectedItem;

                bwScrapeAll.RunWorkerAsync();
            }


        }

        private void btn_ScrapeAllCancel_Click(object sender, RoutedEventArgs e)
        {
            GridConfigScraper.Visibility = Visibility.Collapsed;
        }

        private void btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            if (bwScrapeAll.IsBusy)
            {
                bwScrapeAll.ReportProgress(100, "Annulation en cours...");
                bwScrapeAll.CancelAsync();


            }
            if (bwScanDossier.IsBusy) bwScrapeAll.CancelAsync();

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Films.SelectedItem != null)
            {
                dpInfosFilm.DataContext = null;
                Movie _movie = (Movie)listBox_Films.SelectedItem;
                _MonFilm = _movie.updateItem();
                if (_MonFilm.Cover != null) _MonFilm.Cover.GetImage(true);
                if (_MonFilm.Fanart != null) _MonFilm.Fanart.GetImage(true);

                dpInfosFilm.DataContext = _MonFilm;
            }

        }


    }

}

