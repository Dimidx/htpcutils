using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.IO;
using System.Reflection;
using MediaManager.Library;
using MediaManager.Plugins;

namespace MediaManager
{
    /// <summary>
    /// Interaction logic for ScraperSelect.xaml
    /// </summary>
    public partial class ScraperSelect : Window
    {
        private ObservableCollection<IMMPluginScraper> MovieScrapers = new ObservableCollection<IMMPluginScraper>();
        public static IMMPluginScraper Scraper;
        public Film FilmRecherche; //Film a recherché (provient de la fenetre précédente)
        public List<Film> _Resultats = new List<Film>();
        public Film FilmSelectionne = new Film(); //le film sélectionné dans les résultats

        public BackgroundWorker BackWorkerRecherche = new BackgroundWorker();
        public BackgroundWorker BackWorkerDetails = new BackgroundWorker();

        public ScraperSelect(Film _FilmRecherche)
        {
            this.InitializeComponent();
            FilmRecherche = _FilmRecherche;
            ThreadPool.SetMaxThreads(5, 5);


            Assembly PluginFile;
            IMMPluginScraper ScraperPlugin;
            DirectoryInfo DI = new DirectoryInfo(Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "Plugins/Scraper");

            try
            {
                FileInfo[] FIA = DI.GetFiles("*.dll");
                foreach (FileInfo ScraperFile in FIA)
                {
                    PluginFile = Assembly.LoadFrom(ScraperFile.FullName);

                    ScraperPlugin = PluginFile.CreateInstance("MediaManager.Plugins." + ScraperFile.Name.Substring(0
                                                             , ScraperFile.Name.Length - 4)) as IMMPluginScraper;
                    if (ScraperPlugin != null)
                    {
                        MovieScrapers.Add(ScraperPlugin);
                        
                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            cbScraper.ItemsSource = MovieScrapers;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbScraper.SelectedItem != null)
            {
                if (BackWorkerRecherche.IsBusy == true)
                {
                    BackWorkerRecherche.CancelAsync();
                    while (!BackWorkerRecherche.CancellationPending)
                    {
                        Thread.Sleep(1);
                    }
                }
                GridRecherche.Visibility = Visibility.Visible;

                Scraper = (IMMPluginScraper)cbScraper.SelectedItem;
                if (_Resultats != null)
                {
                    _Resultats.Clear();
                    lstResult.ItemsSource = _Resultats;
                }



                BackWorkerRecherche = new BackgroundWorker();
                BackWorkerRecherche.DoWork += new DoWorkEventHandler(BackWorkerRecherche_DoWork);
                BackWorkerRecherche.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackWorkerRecherche_RunWorkerCompleted);
                BackWorkerRecherche.WorkerSupportsCancellation = true;
                BackWorkerRecherche.RunWorkerAsync();
            }
        }

        void BackWorkerRecherche_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lstResult.ItemsSource = _Resultats;
            GridRecherche.Visibility = Visibility.Collapsed;

            //throw new NotImplementedException();
        }

        void BackWorkerRecherche_DoWork(object sender, DoWorkEventArgs e)
        {

            _Resultats = Scraper.SearchMovie(FilmRecherche);
        }


        private void lstResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lstResult.SelectedItem != null)
            {
                if (BackWorkerDetails.IsBusy == true)
                {
                    BackWorkerDetails.CancelAsync();
                    while (!BackWorkerDetails.CancellationPending)
                    {
                        Thread.Sleep(1);
                    }
                }
                
                
                GridRecherche.Visibility = Visibility.Visible;
                Scraper = (IMMPluginScraper)cbScraper.SelectedItem;
                FilmSelectionne = (Film)lstResult.SelectedItem;
                BackWorkerDetails = new BackgroundWorker();
                BackWorkerDetails.DoWork += new DoWorkEventHandler(BackWorkerDetails_DoWork);
                BackWorkerDetails.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackWorkerDetails_RunWorkerCompleted);
                BackWorkerDetails.WorkerSupportsCancellation = true;
                BackWorkerDetails.RunWorkerAsync();





            }
        }

        void BackWorkerDetails_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DataContext = FilmSelectionne;
            this.gridfanarts.DataContext = FilmSelectionne.ListeFanart;
            this.gridaffiches.DataContext = FilmSelectionne.ListeCover;
            GridRecherche.Visibility = Visibility.Collapsed;
        }

        void BackWorkerDetails_DoWork(object sender, DoWorkEventArgs e)
        {
            FilmSelectionne = Scraper.GetMovie(FilmSelectionne);
        }

        private void lstAffiches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstAffiches.SelectedItem != null)
            {
                imageafficheselect.Source = ((Thumb)lstAffiches.SelectedItem).URLImage;
                //DetailsFilm.Affiche.Source = ((Thumb)lstAffiches.SelectedItem).URLImage;
            }
        }

        private void lstFanarts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFanarts.SelectedItem != null)
            {
                imagefanartselect.Source = ((Thumb)lstFanarts.SelectedItem).URLImage;
                //DetailsFilm.Fanart.Source = ((Thumb)lstFanarts.SelectedItem).URLImage;
            }
        }
    }
}