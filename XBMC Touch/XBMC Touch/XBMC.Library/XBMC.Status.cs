// ------------------------------------------------------------------------
//    XBMControl - A compact remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2008  Bram van Oploo (bramvano@gmail.com)
//                        Mike Thiels (Mike.Thiels@gmail.com)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
//using System.Threading;
//using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace XBMC
{
    public class XBMC_Status : INotifyPropertyChanged
    {
        XBMC_Communicator parent = null;

        //XBMC Properties
        internal bool isConnected = false;
        //private bool isPlaying = false;
        private bool isNotPlaying = true;
        private bool isPlayingLastFm = false;
        private bool isPaused = false;
        private bool _IsMuted = false;
        private bool _IsPlaying = false;
        private int _volume = 0;
        private int _progress = 1;
        private string mediaNowPlaying = null;
        private bool newMediaPlaying = true;
        private Timer heartBeatTimer = null;
        private int connectedInterval = 1;
        private int disconnectedInterval = 10000;
        private BitmapImage _screenshot = null;

        public BitmapImage Screenshot
        {
            get { return _screenshot; }
            set { _screenshot = value; OnPropertyChanged("Screenshot"); }
        }

        /// <summary>
        /// Volume %
        /// </summary>
        public int Volume
        {
            get { return _volume; }
            set { _volume = value; OnPropertyChanged("Volume"); }
        }

        /// <summary>
        /// Progress %
        /// </summary>
        public int Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged("Progress"); }
        }

        public XBMC_Status(XBMC_Communicator p)
        {
            parent = p;
            heartBeatTimer = new Timer();
            heartBeatTimer.Interval = connectedInterval;
            heartBeatTimer.Elapsed += new ElapsedEventHandler(heartBeatTimer_Elapsed);
        }

        /// <summary>
        /// Mute
        /// </summary>
        public bool IsMuted
        {
            get { return _IsMuted; }
            set { _IsMuted = value; OnPropertyChanged("IsMuted"); }
        }

        /// <summary>
        /// Lecture en cours
        /// </summary>
        public bool IsPlaying
        {
            get { return _IsPlaying; }
            set { _IsPlaying = value; OnPropertyChanged("IsPlaying"); }
        }

        void heartBeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            isConnected = parent.Controls.SetResponseFormat();

            heartBeatTimer.Interval = (isConnected) ? connectedInterval : disconnectedInterval;
            //Refresh();
        }

        public void Refresh()
        {
            if (isConnected)
            {
                if (mediaNowPlaying != parent.NowPlaying.Get("filename", true) || mediaNowPlaying == null)
                {
                    mediaNowPlaying = parent.NowPlaying.Get("filename");
                    newMediaPlaying = true;
                }
                else
                    newMediaPlaying = false;

                //isPlaying = (parent.NowPlaying.Get("playstatus", true) == "Playing") ? true : false;
                //isPaused = (parent.NowPlaying.Get("playstatus", true) == "Paused") ? true : false;
                IsPlaying = (parent.NowPlaying.Get("playstatus") != "[Nothing Playing]") ? true : false;

                if (mediaNowPlaying == null || isNotPlaying || mediaNowPlaying.Length < 6)
                {
                    isPlayingLastFm = false;
                }
                else
                {
                    isPlayingLastFm = (mediaNowPlaying.Substring(0, 6) == "lastfm") ? true : false;
                }

                GetVolume();
                GetProgress();

             
            }
        }

        private void HeartBeat_Tick(object sender, EventArgs e)
        {
            isConnected = parent.Controls.SetResponseFormat();
            
            heartBeatTimer.Interval = (isConnected) ? connectedInterval : disconnectedInterval;
        }

        public bool IsConnected()
        {
            return isConnected;
        }

        public void EnableHeartBeat()
        {
            HeartBeat_Tick(null, null);
            heartBeatTimer.Enabled = true;
        }

        public void DisableHeartBeat()
        {
            heartBeatTimer.Enabled = false;
        }

        public bool WebServerEnabled()
        {
            string[] webserverEnabled = parent.Request("WebServerStatus");

            if (webserverEnabled == null)
                return false;
            else
                return (webserverEnabled[0] == "On") ? true : false;
        }

        public bool IsNewMediaPlaying()
        {
            return newMediaPlaying;
        }

        public bool IsNotPlaying()
        {
            return isNotPlaying;
        }

        public bool IsPaused()
        {
            return isPaused;
        }

        public int GetVolume()
        {
            string[] aVolume = parent.Request("GetVolume");

            if (aVolume == null || aVolume[0] == "Error")
            {
                Volume = 0;
            }
            else
            {
                Volume = Convert.ToInt32(aVolume[0]);
            }
            IsMuted = (Volume == 0) ? true : false;
            return Volume;
        }

        public int GetProgress()
        {
            string[] aProgress = parent.Request("GetPercentage");
            if (aProgress == null || aProgress[0] == "Error" || aProgress[0] == "0" || Convert.ToInt32(aProgress[0]) > 99)
                Progress = 1;
            else
                Progress = Convert.ToInt32(aProgress[0]);

            return Progress;
        }

        public bool LastFmEnabled()
        {
            string[] aLastFmUsername = parent.Request("GetGuiSetting(3;lastfm.username)");
            string[] aLastFmPassword = parent.Request("GetGuiSetting(3;lastfm.password)");

            if (aLastFmUsername == null || aLastFmPassword == null)
                return false;
            else
                return (aLastFmUsername[0] == "" || aLastFmPassword[0] == "") ? false : true;
        }

        public bool RepeatEnabled()
        {
            string[] aRepeatEnabled = parent.Request("GetGuiSetting(1;musicfiles.repeat)");
            if (aRepeatEnabled == null)
                return false;
            else
                return (aRepeatEnabled[0] == "False") ? false : true;
        }


            #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
