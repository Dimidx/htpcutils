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
using System.ComponentModel;
using System.IO;
using System.Reflection;
using MediaManager.Library;

namespace MediaManager
{
    /// <summary>
    /// Interaction logic for ScraperSelect.xaml
    /// </summary>
    public partial class ScraperSelect : Window
    {
        private ObservableCollection<MovieScraper> MovieScrapers = new ObservableCollection<MovieScraper>();
        public static MovieScraper Scraper;
        public Film FilmRecherche; //Film a recherché (provient de la fenetre précédente)
        public List<Film> _Resultats = new List<Film>();
        public Film FilmSelectionne = new Film(); //le film sélectionné dans les résultats

        public BackgroundWorker BackWorkerRecherche = new BackgroundWorker();
        public BackgroundWorker BackWorkerDetails = new BackgroundWorker();

        public ScraperSelect(Film _FilmRecherche)
        {
            this.InitializeComponent();

            FilmRecherche = _FilmRecherche;

            Assembly PluginFile;
            MovieScraper ScraperPlugin;
            DirectoryInfo DI = new DirectoryInfo(Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "Scraper" + System.IO.Path.DirectorySeparatorChar + "Movies");

            try
            {
                FileInfo[] FIA = DI.GetFiles("*.dll");
                foreach (FileInfo ScraperFile in FIA)
                {
                    PluginFile = Assembly.LoadFrom(ScraperFile.FullName);

                    ScraperPlugin = PluginFile.CreateInstance("MediaManager.Library." + ScraperFile.Name.Substring(0
                                                             , ScraperFile.Name.Length - 4)) as MovieScraper;
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
                GridRecherche.Visibility = Visibility.Visible;

                Scraper = (MovieScraper)cbScraper.SelectedItem;
                if (_Resultats != null)
                {
                    _Resultats.Clear();
                    lstResult.ItemsSource = _Resultats;
                }



                BackWorkerRecherche = new BackgroundWorker();
                BackWorkerRecherche.DoWork += new DoWorkEventHandler(BackWorkerRecherche_DoWork);
                BackWorkerRecherche.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackWorkerRecherche_RunWorkerCompleted);
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
                GridRecherche.Visibility = Visibility.Visible;
                Scraper = (MovieScraper)cbScraper.SelectedItem;
                FilmSelectionne = (Film)lstResult.SelectedItem;
                BackWorkerDetails = new BackgroundWorker();
                BackWorkerDetails.DoWork += new DoWorkEventHandler(BackWorkerDetails_DoWork);
                BackWorkerDetails.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackWorkerDetails_RunWorkerCompleted);
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
    }
}