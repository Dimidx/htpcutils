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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using XBMC;

namespace XBMC_Touch
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public  XBMC_Communicator XBMC = new XBMC_Communicator();

        public Window1()
        {

            //Language = new XBMCLanguage();
            //XBMC = new XBMC_Communicator();
            XBMC.SetIp("127.0.0.1:8080");
            XBMC.SetConnectionTimeout(1000);
            XBMC.SetCredentials("xbmc", "");
            this.DataContext = XBMC;
           
            XBMC.Status.Refresh();
            InitializeComponent();
            //Timer timer = new Timer();
            //timer.Interval = 1000;
            //timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //timer.Enabled = true;
            //timer.Start();


            //ApplySettings();
            //SetLanguageStrings();
            //Initialize();

        }



        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.Play();
        }

        private void btn_Precedent_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.Previous();
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.Next();
        }
    }
}
