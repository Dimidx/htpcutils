using System;
using System.Collections.Generic;
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
using TvdbLib;
using TvdbLib.Cache;
using TvdbLib.Data;
using MediaManager.Library;
using TvdbLib.Data.Banner;

namespace MediaManager
{
	/// <summary>
	/// Interaction logic for SerieDetails.xaml
	/// </summary>
	public partial class SerieDetails
	{
		public SerieDetails()
		{
			this.InitializeComponent();
		}

        private void btn_ChargeDetails_Click(object sender, RoutedEventArgs e)
        {
   
            TvdbHandler tvdbHandler = new TvdbHandler(new XmlCacheProvider(System.Environment.CurrentDirectory + @"\Cache\Series\"), "69C6FDC7E5F4B985");
            tvdbHandler.InitCache();
            List<TvdbLanguage> m_languages = tvdbHandler.Languages;
            TvdbSeries s = tvdbHandler.GetSeries(79349, TvdbLanguage.DefaultLanguage, true, true, true);
            //Charge poster saisons
            
            List<Thumb> seasonList = new List<Thumb>();
            if (s.SeasonBanners != null && s.SeasonBanners.Count > 0)
            {
                for (int i = 0; i < s.NumSeasons; i++)
                {
                    foreach (TvdbSeasonBanner b in s.SeasonBanners)
                    {
                        if (b.Season == i)
                        {
                            if (b.BannerType == TvdbSeasonBanner.Type.season)
                            {

                                seasonList.Add(new Thumb("http://thetvdb.com/banners/" + b.BannerPath));
                            }
                        }
                    }
                }
            }
            

            //Thumb t = new Thumb("http://thetvdb.com/banners/" + s.BannerPath);
            this.DataContext = s;

            tvdbHandler.CloseCache();
        }
	}
}