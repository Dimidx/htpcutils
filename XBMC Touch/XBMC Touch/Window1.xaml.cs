using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using XBMC_Touch.Properties;

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

            InitializeComponent();
            Load();
            this.DataContext = XBMC;


            if (Settings.Default.FirstTime)
            {
                LoadConfig();
                this.GridConfig.Visibility = Visibility.Visible;
                Storyboard sbdshowMenu = (Storyboard)FindResource("ShowConfig");
                sbdshowMenu.Begin(this);
            }


        }

        private void Load()
        {
            XBMC.SetIp(Settings.Default.Ip);
            XBMC.SetConnectionTimeout(Settings.Default.ConnectionTimeout);
            XBMC.SetCredentials(Settings.Default.UserName, Settings.Default.Password);
            XBMC.NowPlaying.FistTime = true;
            XBMC.Status.Refresh();


            //Chargement des artistes
            string[] _art = XBMC.Database.GetArtists();
            ObservableCollection<MusicArtist> _artistes = new ObservableCollection<MusicArtist>();
            if (_art != null)
            {
                foreach (string item in _art)
                {
                    MusicArtist art = new MusicArtist();
                    art.Artist = item;
                    _artistes.Add(art);
                }
            }
            lstArtistes.ItemsSource = _artistes;
            Console.WriteLine(lstPlaylist.Items.Count.ToString());
        }


        private void LoadConfig()
        {
            txtIP.Text = Settings.Default.Ip;
            txtUsername.Text = Settings.Default.UserName;
            txtPassword.Text = Settings.Default.Password;
            txtTimeout.Text = Settings.Default.ConnectionTimeout.ToString();

        }

        private void SaveConfig()
        {
            Settings.Default.Ip = txtIP.Text;
            Settings.Default.UserName = txtUsername.Text;
            Settings.Default.Password = txtPassword.Text;
            Settings.Default.ConnectionTimeout = Convert.ToInt32(txtTimeout.Text);
            Settings.Default.FirstTime = false;
            Settings.Default.Save();
            Load();
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

        private void btn_VolMute_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.ToggleMute();

        }

        private void btn_VolUp_Click(object sender, RoutedEventArgs e)
        {
            if (XBMC.Status.IsMuted) XBMC.Controls.ToggleMute();
            XBMC.Controls.SetVolume(XBMC.Status.Volume + 1);
        }

        private void btn_VolDown_Click(object sender, RoutedEventArgs e)
        {
            if (XBMC.Status.IsMuted) XBMC.Controls.ToggleMute();
            XBMC.Controls.SetVolume(XBMC.Status.Volume - 1);

        }

        private void btn_Recule_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.SeekPercentage(XBMC.Status.Progress - 1);
        }

        private void btn_Avance_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.SeekPercentage(XBMC.Status.Progress + 1);
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            XBMC.Controls.Stop();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.GridMenu.Visibility == Visibility.Hidden)
            { 
                this.GridMenu.Visibility = Visibility.Visible;
                Storyboard sbdshowMenu = (Storyboard)FindResource("ShowMenu"); 
                sbdshowMenu.Begin(this); 
                return;
            }

            if (this.GridMenu.Visibility == Visibility.Visible) { this.GridMenu.Visibility = Visibility.Hidden; return; }

        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSaveParam_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            this.GridConfig.Visibility = Visibility.Hidden;
            this.GridMenu.Visibility = Visibility.Hidden;
        }

        private void btnAnnulerParam_Click(object sender, RoutedEventArgs e)
        {
            this.GridConfig.Visibility = Visibility.Hidden;
            this.GridMenu.Visibility = Visibility.Hidden;
        }

        private void btnParametres_Click(object sender, RoutedEventArgs e)
        {
                LoadConfig();
                this.GridConfig.Visibility = Visibility.Visible;
                Storyboard sbdshowMenu = (Storyboard)FindResource("ShowConfig");
                sbdshowMenu.Begin(this);

        }

        private void btnLoadPlaylist_Click(object sender, RoutedEventArgs e)
        {
            string[] _art = XBMC.Database.GetArtists();


            //ObservableCollection<MusicSong> plsong = new ObservableCollection<MusicSong>();

            ObservableCollection<MusicArtist> _artistes = new ObservableCollection<MusicArtist>();

            foreach (string item in _art)
            {
                //MusicSong song = new MusicSong(); ;
                //song = XBMC.Database.GetSongByFileName(item);
                //plsong.Add(song);
                MusicArtist art = new MusicArtist();
                art.Artist = item;

                _artistes.Add(art);

            }

            lstPlaylist.ItemsSource = _artistes;
            Console.WriteLine(lstPlaylist.Items.Count.ToString());

            
        }


    }
}
