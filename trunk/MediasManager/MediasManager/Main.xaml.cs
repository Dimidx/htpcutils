using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Windows.Controls.Ribbon;
using AllocineLib;
using MediaManager;

namespace AllocineViewer
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Allocine AllocineHandler = null;
        public BackgroundWorker BackWorker = new BackgroundWorker();
        public bool _RechercheTerminée = true;
        public Films _Resultats = new Films();
        public string _CritereRecherche = "";

        //public Films CollectionFilms = new Films();

        //Collection de films
        //ObservableCollection<Film> _CollectionFilms = new ObservableCollection<Film>();

        //public ObservableCollection<Film> CollectionFilms
        //{ get { return _CollectionFilms; } }

        public Main()
        {
            InitializeComponent();
            AllocineHandler = new Allocine(@"C:\Temp\");
        }

        private void btn_Recherche_Click(object sender, RoutedEventArgs e)
        {

            BackWorker.DoWork += new DoWorkEventHandler(Recherche_DoWork);
            BackWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Recherche_RunWorkerCompleted);

            _CritereRecherche = sai_Recherche.Text;
            _RechercheTerminée = false;
            if (BackWorker.IsBusy == false)
            {
                this.jauge_progress.Visibility = Visibility.Visible;

                BackWorker.RunWorkerAsync();
            }
            

        }

        private void Recherche_DoWork(object sender, DoWorkEventArgs e)
        {
            _Resultats = AllocineHandler.RechercheFilm(_CritereRecherche, true);

        }

        private void Recherche_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
  
            //Lance la recherche
            Binding b = new Binding();
            b.Source = _Resultats;
            list_recherche.SetBinding(ItemsControl.ItemsSourceProperty, b);
            _RechercheTerminée = true;
            this.jauge_progress.Visibility = Visibility.Collapsed;

        }

        private void list_recherche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_recherche.SelectedItem != null)
            {
                Film MonFilm;
                MonFilm = (Film)list_recherche.SelectedItem;
                MonFilm = AllocineHandler.GetFilmDetails(MonFilm.ID, true, true);
                this.DataContext = MonFilm;
            }
        }


    }



}
