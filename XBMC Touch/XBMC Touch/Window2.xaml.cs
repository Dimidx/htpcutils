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
using System.Windows.Shapes;
using XBMC_Touch.Properties;
using XBMC;
using System.Timers;
using System.IO;

namespace XBMC_Touch
{
    /// <summary>
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public XBMC_Communicator XBMC = new XBMC_Communicator();
        public Timer timer = new Timer();
        public Window2()
        {
            XBMC.SetIp("127.0.0.1:80");
            XBMC.SetConnectionTimeout(Settings.Default.ConnectionTimeout);
            XBMC.SetCredentials(Settings.Default.UserName, Settings.Default.Password);
            this.DataContext = XBMC;

         



            InitializeComponent();
        }

  
    }
}
