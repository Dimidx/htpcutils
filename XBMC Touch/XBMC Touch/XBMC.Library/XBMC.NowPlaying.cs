// ------------------------------------------------------------------------
//    XBMControl - A compact remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2008  Bram van Oploo (bramvano@gmail.com)
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net;
using System.Windows.Media.Imaging;
using System.Timers;

//using System.Windows.Forms;

namespace XBMC
{
    public class XBMC_NowPlaying : INotifyPropertyChanged
    {
        public XBMC_Communicator parent = null;
        private string[,] maNowPlayingInfo = null;
        private string error = null;
        private BitmapImage _Thumb;
        private String _Titre;
        private String _Album;
		private String _Duration;
        private String _Time;
        private String _Artiste;
        private String _Status;
        private int _Percentage;

        private Timer timer;

        /// <summary>
        /// L'image
        /// </summary>
        public BitmapImage Thumb
        {
            get { return _Thumb; }
            set { _Thumb = value; OnPropertyChanged("Thumb"); }
        }

        /// <summary>
        /// Le Titre
        /// </summary>
        public String Titre
        {
            get { return _Titre; }
            set { _Titre = value; OnPropertyChanged("Titre"); }
        }

        /// <summary>
        /// Le Status
        /// </summary>
        public String Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged("Status"); }
        }

        /// <summary>
        /// L'Album
        /// </summary>
        public String Album
        {
            get { return _Album; }
            set { _Album = value; OnPropertyChanged("Album"); }
        }

        /// <summary>
        /// L'Artiste
        /// </summary>
        public String Artiste
        {
            get { return _Artiste; }
            set { _Artiste = value; OnPropertyChanged("Artiste"); }
        }

		        /// <summary>
        /// Time
        /// </summary>
        public String Time
        {
            get { return _Time; }
            set { _Time = value; OnPropertyChanged("Time"); }
        }
		
		/// <summary>
        /// Duration
        /// </summary>
        public String Duration
        {
            get { return _Duration; }
            set { _Duration = value; OnPropertyChanged("Duration"); }
        }

        /// <summary>
        /// Percentage
        /// </summary>
        public int Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; OnPropertyChanged("Percentage"); }
        }

        public void Refresh()
        {
            if (parent.Status.IsConnected() != false && Get("filename",true) != "[Nothing Playing]")
            {
                //_Titre =
                Titre = Get("title", true);
                Album = Get("album");
                Artiste = Get("artist");
				Time = Get("time");
				Duration = Get("duration");
                Status = Get("playstatus");
                Percentage = Convert.ToInt32(Get("percentage"));
                
                //_Thumb = GetCoverArt();
                Thumb = GetCoverArt();
            }
        }


        public XBMC_NowPlaying(XBMC_Communicator p)
        {
            parent = p;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Refresh();
        }

        public string Get(string field, bool refresh)
        {
            string returnValue = null;

            if (refresh)
            {
                string[] aNowPlayingTemp = parent.Request("GetCurrentlyPlaying");

                if (aNowPlayingTemp != null)
                {
                    maNowPlayingInfo = new string[aNowPlayingTemp.Length, 2];
                    for (int x = 0; x < aNowPlayingTemp.Length; x++)
                    {
                        int splitIndex = aNowPlayingTemp[x].IndexOf(':') + 1;

                        if (splitIndex > 2)
                        {
                            maNowPlayingInfo[x, 0] = aNowPlayingTemp[x].Substring(0, splitIndex - 1).Replace(" ", "").ToLower();
                            maNowPlayingInfo[x, 1] = aNowPlayingTemp[x].Substring(splitIndex, aNowPlayingTemp[x].Length - splitIndex);

                            if (maNowPlayingInfo[x, 0] == field)
                                returnValue = this.maNowPlayingInfo[x, 1];
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < this.maNowPlayingInfo.GetLength(0); x++)
                {
                    if (this.maNowPlayingInfo[x, 0] == field)
                        returnValue = this.maNowPlayingInfo[x, 1];
                }
            }

            return returnValue;
        }

        public string Get(string field)
        {
            return this.Get(field, false);
        }

        public BitmapImage GetCoverArt()
        {
            string[] downloadData;
            string ipString;
            string[] fileExist;
            BitmapImage _thumbnail = new BitmapImage();

            ipString = Get("thumb");
            fileExist = parent.Request("FileExists", ipString);

            if (fileExist[0] == "True")
            {

                try
                {
                    downloadData = parent.Request("FileDownload", ipString);

                    // Convert Base64 String to byte[]
                    byte[] imageBytes = Convert.FromBase64String(downloadData[0]);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    //BitmapImage titi = new BitmapImage();
                    _thumbnail.BeginInit();
                    _thumbnail.StreamSource = ms;
                    _thumbnail.EndInit();
                    _thumbnail.Freeze();
                }
                catch (Exception)
                {

                    return _thumbnail;
                }

                    //thumbnail = Image.FromStream(ms, true);

            }
            else
            {
                //_Thumb = Resources.video_32x32;
            }

            return _thumbnail;



        }

        public string GetMediaType()
        {
            return this.Get("type", true);
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
