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
        public bool FistTime = true;

        #region Private
        private string _Filename;
        private string _PlayStatus;
        private string _SongNo;
        private string _Type;
        private string _Title;
        private string _Track;
        private string _Artist;
        private string _Album;
        private string _Genre;
        private string _Year;
        private string _URL;
        private string _Lyrics;
        private string _Bitrate;
        private string _Samplerate;
        private string _Time;
        private string _Duration;
        private int _Percentage;
        private string _FileSize;
        private bool _Changed;
        private bool _IsPlaying = false;

        private BitmapImage _Thumb;
        private Timer timer;
        private string[,] maNowPlayingInfo = null;
        #endregion

        #region Public

        /// <summary>
        /// L'image
        /// </summary>
        public BitmapImage Thumb
        {
            get { return _Thumb; }
            set { _Thumb = value; _Thumb.Freeze(); OnPropertyChanged("Thumb"); }
        }

        /// <summary>
        /// En cours de lecture
        /// </summary>
        public bool IsPlaying
        {
            get { return _IsPlaying; }
            set { _IsPlaying = value; OnPropertyChanged("IsPlaying"); }
        }

        /// <summary>
        /// Le Titre
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }

        /// <summary>
        /// Le chemin du fichier
        /// </summary>
        public string Filename
        {
            get { return _Filename; }
            set { _Filename = value; OnPropertyChanged("Filename"); }
        }

        /// <summary>
        /// Le Song n°
        /// </summary>
        public string SongNo
        {
            get { return _SongNo; }
            set { _SongNo = value; OnPropertyChanged("SongNo"); }
        }

        /// <summary>
        /// Le type
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }

        /// <summary>
        /// Le n° de la piste
        /// </summary>
        public string Track
        {
            get { return _Track; }
            set { _Track = value; OnPropertyChanged("Track"); }
        }

        /// <summary>
        /// Le Genre
        /// </summary>
        public string Genre
        {
            get { return _Genre; }
            set { _Genre = value; OnPropertyChanged("Genre"); }
        }

        /// <summary>
        /// L'Année
        /// </summary>
        public string Year
        {
            get { return _Year; }
            set { _Year = value; OnPropertyChanged("Year"); }
        }

        /// <summary>
        /// L'URL
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; OnPropertyChanged("URL"); }
        }

        /// <summary>
        /// Les Lyrics
        /// </summary>
        public string Lyrics
        {
            get { return _Lyrics; }
            set { _Lyrics = value; OnPropertyChanged("Lyrics"); }
        }

        /// <summary>
        /// Le Bitrate
        /// </summary>
        public string Bitrate
        {
            get { return _Bitrate; }
            set { _Bitrate = value; OnPropertyChanged("Bitrate"); }
        }

        /// <summary>
        /// Sample Rate
        /// </summary>
        public string SampleRate
        {
            get { return _Samplerate; }
            set { _Samplerate = value; OnPropertyChanged("Samplerate"); }
        }

        /// <summary>
        /// Taille du fichier en octets
        /// </summary>
        public string FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; OnPropertyChanged("FileSize"); }
        }

        /// <summary>
        /// Changement entre 2 appels
        /// </summary>
        public bool Changed
        {
            get { return _Changed; }
            set { _Changed = value; OnPropertyChanged("Changed"); }
        }


        /// <summary>
        /// Le Status
        /// </summary>
        public string PlayStatus
        {
            get
            {
                //return _PlayStatus;
                switch (_PlayStatus)
                {
                    case "Playing":
                        return "Lecture en cours";
                        break;

                    case "Paused":
                        return "Pause";
                        break;
                    default:
                        return "";
                        break;
                }

            }
            set { _PlayStatus = value; OnPropertyChanged("PlayStatus"); }
        }

        /// <summary>
        /// L'Album
        /// </summary>
        public string Album
        {
            get { return _Album; }
            set { _Album = value; OnPropertyChanged("Album"); }
        }

        /// <summary>
        /// L'Artiste
        /// </summary>
        public string Artist
        {
            get { return _Artist; }
            set { _Artist = value; OnPropertyChanged("Artist"); }
        }

        /// <summary>
        /// Time
        /// </summary>
        public string Time
        {
            get { return _Time; }
            set { _Time = value; OnPropertyChanged("Time"); }
        }

        /// <summary>
        /// Duration
        /// </summary>
        public string Duration
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


        #endregion


        /// <summary>
        /// Lance en refresh des données du nowplaying
        /// </summary>
        public void Refresh()
        {
            if (parent.Status.IsConnected() != false)
            {
                if (Get("filename", true) == "[Nothing Playing]") { Reset(); return; }
                switch (Get("changed"))
                {
                    case "True":
                        Changed = true;
                        break;

                    case "False":
                        Changed = false;
                        break;
                    default:
                        Changed = false;
                        break;
                }

                Percentage = Convert.ToInt32(Get("percentage"));
                Time = Get("time");
                IsPlaying = true;

                if (Changed || FistTime)
                {
                    PlayStatus = Get("PlayStatus");
                    SongNo = Get("SongNo");
                    Type = Get("Type");
                    Title = Get("Title");
                    Track = Get("Track");
                    Artist = Get("Artist");
                    Album = Get("Album");
                    Genre = Get("Genre");
                    Year = Get("Year");
                    URL = Get("URL");
                    Lyrics = Get("Lyrics");
                    Bitrate = Get("Bitrate");
                    SampleRate = Get("SampleRate");
                    Thumb = GetCoverArt();
                    Duration = Get("Duration");
                    FileSize = Get("File size");
                    FistTime = false;

                }
            }
            else
            {
                Reset();

            }


        }

        private void Reset()
        {
            IsPlaying = false;
            Percentage = 0;
            Time = "";
            PlayStatus = "";
            SongNo = "";
            Type = "";
            Title = "";
            Track = "";
            Artist = "";
            Album = "";
            Genre = "";
            Year = "";
            URL = "";
            Lyrics = "";
            Bitrate = "";
            SampleRate = "";
            Thumb = GetCoverArt();
            Duration = "";
            FileSize = "";
        }


        public XBMC_NowPlaying(XBMC_Communicator p)
        {
            parent = p;
            FistTime = true;
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
            field = field.ToLower();
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
                if (maNowPlayingInfo != null)
                {
                    for (int x = 0; x < this.maNowPlayingInfo.GetLength(0); x++)
                    {
                        if (this.maNowPlayingInfo[x, 0] == field)
                            returnValue = this.maNowPlayingInfo[x, 1];
                    }
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
            _thumbnail.BeginInit();

            ipString = Get("thumb");
            if (ipString == null) ipString = "";
            fileExist = parent.Request("FileExists", ipString);
            if (fileExist != null)
            {
                if (fileExist[0] == "True")
                {
                    try
                    {

                        downloadData = parent.Request("FileDownload", ipString);

                        // Convert Base64 String to byte[]
                        byte[] imageBytes = Convert.FromBase64String(downloadData[0]);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        _thumbnail.StreamSource = ms;

                    }
                    catch (Exception)
                    {
                        _thumbnail.UriSource = new Uri("pack://application:,,,/Resources/defaultAudio.png", UriKind.RelativeOrAbsolute);
                    }

                }
                else
                {
                    _thumbnail.UriSource = new Uri("pack://application:,,,/Resources/defaultAudio.png", UriKind.RelativeOrAbsolute);
                }
            }
            else
            {
                _thumbnail.UriSource = new Uri("pack://application:,,,/Resources/defaultAudio.png", UriKind.RelativeOrAbsolute);
            }

            _thumbnail.EndInit();
            _thumbnail.Freeze();
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
