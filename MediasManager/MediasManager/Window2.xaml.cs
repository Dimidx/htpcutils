using System;
using System.Collections.ObjectModel;
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
using TvdbLib;
using TvdbLib.Cache;
using TvdbLib.Data;
using MediaManager.Library;
using TvdbLib.Data.Banner;

namespace MediaManager
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            App.Current.Shutdown();
        }


        private void btn_ChargeDetails_Click(object sender, RoutedEventArgs e)
        {


        }

        private void tvSeries_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Console.WriteLine(tvSeries.SelectedItem.ToString());
            Type t = tvSeries.SelectedItem.GetType();
            if (t == typeof(Serie))
            {
                dtSerieDetails.DataContext = ((Serie)tvSeries.SelectedItem).SerieInfo;
            }



        }

    }

    public class Serie
    {
        private List<Saison> _ListeSaisons;
        private TvdbSeries _Serie;

        public List<Saison> ListeSaisons
        {
            get { return _ListeSaisons; }
        }

        public string SerieName
        {
            get { return _Serie.SeriesName; }
        }

        public TvdbSeries SerieInfo
        {
            get { return _Serie; }
        }


        public Serie(TvdbHandler TvdbHandler, int SerieID)
        {
            _Serie = TvdbHandler.GetSeries(SerieID, TvdbHandler.UserInfo.UserPreferredLanguage, true, true, true);
            _ListeSaisons = new List<Saison>();

            _ListeSaisons.Add(new Saison("Spécial"));

            for (int i = 1; i <= _Serie.NumSeasons; i++)
            {
                _ListeSaisons.Add(new Saison("Saison " + i.ToString()));
            }

            foreach (TvdbEpisode item in _Serie.Episodes)
            {
                Console.WriteLine(item.SeasonNumber.ToString());

                if (item.SeasonNumber < _ListeSaisons.Count)
                {
                    _ListeSaisons[item.SeasonNumber].ListeEpisodes.Add(item);
                }
                else
                {
                    Console.WriteLine(item.ToString());
                }

            }
            Console.WriteLine(_Serie.SeriesName + "Terminéé");

        }

    }

    public class Saison
    {
        private List<TvdbEpisode> _ListeEpisodes;
        private string _NomSaison;

        public List<TvdbEpisode> ListeEpisodes
        {
            get { return _ListeEpisodes; }
        }

        public string NomSaison
        {
            get { return _NomSaison; }
        }

        public Saison(string Name)
        {
            _NomSaison = Name;
            _ListeEpisodes = new List<TvdbEpisode>();
        }

    }


    public class ListeSeries : List<Serie>
    {
        public ListeSeries()
        {

            TvdbHandler tvdbHandler = new TvdbHandler(new XmlCacheProvider(System.Environment.CurrentDirectory + @"\Cache\Series\"), "69C6FDC7E5F4B985");
            tvdbHandler.InitCache();
            TvdbUser user = new TvdbUser("Danone-KiD", "401D8B31F9832AA3");
            tvdbHandler.UserInfo = user;
            tvdbHandler.UserInfo.UserPreferredLanguage = tvdbHandler.GetPreferredLanguage();
            //get list of all favorites and print them to console
            List<int> favList = tvdbHandler.GetUserFavouritesList();

            foreach (int id in favList)
            {
                //TvdbSeries s = tvdbHandler.GetSeries(id, tvdbHandler.UserInfo.UserPreferredLanguage, true, true, true);
                Add(new Serie(tvdbHandler, id));

            }
            //Thumb t = new Thumb("http://thetvdb.com/banners/" + s.BannerPath);
            //this.DataContext = s;
            //lstSaisons.ItemsSource = seasonList;
            tvdbHandler.CloseCache();


        }
    }
}