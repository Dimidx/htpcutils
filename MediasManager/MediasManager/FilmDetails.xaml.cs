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
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MediaManager
{
	/// <summary>
	/// Logique d’interaction pour FilmDetails.xaml
	/// </summary>
	public partial class FilmDetails
	{
		
        public FilmDetails()
		{
			this.InitializeComponent();
            DispatcherTimer dp = new System.Windows.Threading.DispatcherTimer();
            dp.Tick += new EventHandler(dp_Tick);
            dp.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            dp.Start();
            //TimerCallback _RefletCallBack = new TimerCallback(
            //Timer _Reflet = new Timer(RefletCallBack,false,100,500);
            
        }

        void dp_Tick(object sender, EventArgs e)
        {
            Storyboard _sbReflet = (Storyboard)FindResource("sbRefletCover");
            _sbReflet.Begin(this);
        }

        public void RefletCallBack(Object stateInfo)
        {

        }



        private void btnPlayTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(lib_Trailer.Text))
            {
                System.Diagnostics.Process.Start(lib_Trailer.Text);

            }


        }

	}
}