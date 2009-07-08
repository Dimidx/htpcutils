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
        public List<Film> _Resultats = null;
        public Film FilmSelectionne = null; //le film sélectionné dans les résultats
        public Film FilmValid = null; //le film sélectionné dans les résultats
        public BackgroundWorker BackWorkerRecherche = new BackgroundWorker();
        public BackgroundWorker BackWorkerDetails = new BackgroundWorker();
        public ObservableCollection<Utils.ChampModifiable> _ListeChampsModif  = new ObservableCollection<Utils.ChampModifiable>();


        public ScraperSelect(Film _FilmRecherche)
        {
            this.InitializeComponent();
            FilmRecherche = _FilmRecherche;
            //ThreadPool.SetMaxThreads(5, 1);
            _ListeChampsModif = Utils.GetChampsModifiables(FilmRecherche);
            ListCollectionView lcv = new ListCollectionView(_ListeChampsModif);
            lcv.SortDescriptions.Add(new System.ComponentModel.SortDescription("NomChamp", System.ComponentModel.ListSortDirection.Ascending));
            lstChamps.ItemsSource = lcv;

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
                    while (!BackWorkerDetails.IsBusy)
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
            //this.DataContext = FilmSelectionne;
            //this.gridfanarts.DataContext = FilmSelectionne.ListeFanart;
            //this.gridaffiches.DataContext = FilmSelectionne.ListeCover;
            GridRecherche.Visibility = Visibility.Collapsed;
        }

        void BackWorkerDetails_DoWork(object sender, DoWorkEventArgs e)
        {
            FilmSelectionne = Scraper.GetMovie(FilmSelectionne);
            System.Threading.Thread thread = new System.Threading.Thread(
new System.Threading.ThreadStart(
delegate()
{
    this.Dispatcher.Invoke(
      System.Windows.Threading.DispatcherPriority.DataBind,
      new Action(
        delegate()
        {
            gridaffiches.DataContext = FilmSelectionne.ListeCover;
            gridfanarts.DataContext = FilmSelectionne.ListeFanart;
            DetailsFilm.DataContext = FilmSelectionne;
        }
    ));
}
));

            thread.Start();


        }


        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            //FilmValid = FilmSelectionne;
            if (FilmSelectionne != null)
            {
                FilmValid = new Film();

                foreach (Utils.ChampModifiable c in _ListeChampsModif)
                {
                    if (c.IsModifiable)
                    {
                        c.PropertyInfo.SetValue(FilmValid, c.PropertyInfo.GetValue(FilmSelectionne, null), null);

                    }
                    else
                    {
                        c.PropertyInfo.SetValue(FilmValid, c.PropertyInfo.GetValue(FilmRecherche, null), null);

                    }

                }
            }
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool _coche = ((Utils.ChampModifiable)lstChamps.Items[0]).IsModifiable;
            foreach (Utils.ChampModifiable c in _ListeChampsModif)
            {
                c.IsModifiable = _coche;
            }

            //lstChamps.Items.Clear();
            //lstChamps.ItemsSource = _ListeChampsModif;

        }

        private void btn_DefAffiche_Click(object sender, RoutedEventArgs e)
        {
            if (lstAffiches.SelectedItem != null)
            {
                FilmSelectionne.ListeCover.Move(lstAffiches.SelectedIndex, 0);
                FilmSelectionne.OnPropertyChanged("Cover");

            }


        }

        private void btn_DefFanart_Click(object sender, RoutedEventArgs e)
        {
            if (lstFanarts.SelectedItem != null)
            {
                FilmSelectionne.ListeFanart.Move(lstFanarts.SelectedIndex, 0);
                FilmSelectionne.OnPropertyChanged("Fanart");

            }

        }
    }
}