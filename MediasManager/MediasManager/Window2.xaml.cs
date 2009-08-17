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

        private void btn_ChargeDetails_Click(object sender, RoutedEventArgs e)
        {

            TvdbHandler tvdbHandler = new TvdbHandler(new XmlCacheProvider(System.Environment.CurrentDirectory + @"\Cache\Series\"), "69C6FDC7E5F4B985");
            tvdbHandler.InitCache();
            TvdbUser user = new TvdbUser("Danone-KiD", "401D8B31F9832AA3");
            tvdbHandler.UserInfo = user;
            tvdbHandler.UserInfo.UserPreferredLanguage = tvdbHandler.GetPreferredLanguage();
            //get list of all favorites and print them to console
            List<int> favList = tvdbHandler.GetUserFavouritesList();
            ObservableCollection<TvdbSeries> _ListeSeries = new ObservableCollection<TvdbSeries>();

            foreach (int id in favList)
            {
                TvdbSeries s = tvdbHandler.GetSeries(id, tvdbHandler.UserInfo.UserPreferredLanguage, true, true, true);
                _ListeSeries.Add(s);

                //List<Thumb> seasonList = new List<Thumb>();
                //if (s.SeasonBanners != null && s.SeasonBanners.Count > 0)
                //{
                //    for (int i = 0; i < s.NumSeasons; i++)
                //    {
                //        foreach (TvdbSeasonBanner b in s.SeasonBanners)
                //        {
                //            if (b.Season == i)
                //            {
                //                if (b.BannerType == TvdbSeasonBanner.Type.season)
                //                {

                //                    seasonList.Add(new Thumb("http://thetvdb.com/banners/" + b.BannerPath));
                //                    goto saisonnext;
                //                }
                //            }
                //        }
                //    saisonnext: { }
                //    }
                //}

            }


            //Thumb t = new Thumb("http://thetvdb.com/banners/" + s.BannerPath);
            //this.DataContext = s;
            //lstSaisons.ItemsSource = seasonList;
            this.DataContext = _ListeSeries;
            lstSeries.ItemsSource = _ListeSeries;
            tvdbHandler.CloseCache();
        }
	}
}