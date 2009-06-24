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
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
//using System.Windows.Forms;

namespace XBMC
{
    public class XBMC_Communicator : INotifyPropertyChanged
    {
        public XBMC_Database Database = null;
        public XBMC_Playlist Playlist = null;
        public XBMC_Controls Controls = null;
        //public XBMC_NowPlaying NowPlaying = null;
        private XBMC_Status _Status = null;
        public XBMC_Media Media = null;
        public XBMC_Video Video = null;

        private string _xbmcUsername;
        private XBMC_NowPlaying _NowPlaying;
        private string configuredIp = null;
        
        public string xbmcUsername
        {
            get { return _xbmcUsername; }
            set { _xbmcUsername = value; OnPropertyChanged("xbmcUsername"); }
        }


        /// <summary>
        /// NowPlaying
        /// </summary>
        public XBMC_NowPlaying NowPlaying
        {
            get { return _NowPlaying; }
            set { _NowPlaying = value; OnPropertyChanged("NowPlaying"); }
        }

        /// <summary>
        /// Status
        /// </summary>
        public XBMC_Status Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged("Status"); }
        }

        private string xbmcPassword = null;
        private string apiPath = "/xbmcCmds/xbmcHttp";
        private string logFile = "log/xbmcontrol.log";
        private int connectionTimeout = 2000;

        public XBMC_Communicator()
        {
            Database = new XBMC_Database(this);
            Playlist = new XBMC_Playlist(this);
            Controls = new XBMC_Controls(this);
            _NowPlaying = new XBMC_NowPlaying(this);
            Status = new XBMC_Status(this);
            Media = new XBMC_Media(this);
            Video = new XBMC_Video(this);
        }

        private string[] MySplitString(string s, string delimeter)
        {
            if (s == null)
                throw new ArgumentNullException("stringToBeSplitted is null.");
            if (delimeter == null)
                throw new ArgumentNullException("delimeter is null.");

            int dsum = 0;
            int ssum = 0;
            int dl = delimeter.Length;
            int sl = s.Length;

            if (dl == 0 || sl == 0 || sl < dl)
                return new string[] { s };

            char[] cd = delimeter.ToCharArray();
            char[] cs = s.ToCharArray();
            List<string> retlist = new List<string>();

            for (int i = 0; i < dl; i++)
            {
                dsum += cd[i];
                ssum += cs[i];
            }

            int start = 0;
            for (int i = start; i < sl - dl; i++)
            {
                if (i >= start && dsum == ssum && s.Substring(i, dl) == delimeter)
                {
                    retlist.Add(s.Substring(start, i - start));
                    start = i + dl;
                }

                ssum += cs[i + dl] - cs[i];
            }

            if (dsum == ssum && s.Substring(sl - dl, dl) == delimeter)
            {
                retlist.Add(s.Substring(start, sl - dl - start));
                retlist.Add("");
            }
            else
            {
                retlist.Add(s.Substring(start, sl - start));
            }

            return retlist.ToArray();
        }

        public string[] Request(string command, string parameter, string ip)
        {
            string[] pageItems = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            string[] pageContent = null;
            string tempString = null;

            bool isQuery = (command.ToLower() == "querymusicdatabase" || command.ToLower() == "queryvideodatabase") ? true : false;

            string ipAddress = (ip == null) ? configuredIp : ip;
            parameter = (parameter == null) ? "" : parameter;
            command = "?command=" + Uri.EscapeDataString(command);
            command += (parameter == null || parameter == "") ? "" : "&parameter=" + Uri.EscapeDataString(parameter);
            string uri = "http://" + ipAddress + apiPath + command;

            //WriteToLog(uri);

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = "GET";
                request.Timeout = connectionTimeout;
                if (xbmcUsername != "" && xbmcPassword != "") request.Credentials = new NetworkCredential(xbmcUsername, xbmcPassword);
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                if (isQuery)
                {
                    tempString = reader.ReadToEnd().Replace("</field>", "").Replace("\n", "").Replace("<html>", "").Replace("</html>", "");
                    pageContent = MySplitString(tempString, "<field>");
                }
                else
                {
                    tempString = reader.ReadToEnd().Replace("\n", "").Replace("<html>", "").Replace("</html>", "");
                    pageContent = MySplitString(tempString, "<li>");
                }

                if (pageContent != null)
                {
                    if (pageContent.Length > 1)
                    {
                        pageItems = new string[pageContent.Length-1];

                        for (int x = 1; x < pageContent.Length; x++)
                            pageItems[x-1] = pageContent[x];
                    }
                    else
                    {
                        pageItems = new string[1];
                        pageItems[0] = pageContent[0];
                    }
                }
            }
            catch (WebException e)
            {
                WriteToLog("ERROR - " + e.Message);
            }
            finally
            {
                if (response != null) response.Close();
                if (reader != null) reader.Close();
            }

            return pageItems;
        }

        public string[] Request(string command, string parameter)
        {
            return Request(command, parameter, null);
        }

        public string[] Request(string command)
        {
            return Request(command, null, null);
        }

        private void WriteToLog(string message)
        {

            StreamWriter sw = null;
            string error = null;

            try
            {
                sw = new StreamWriter(logFile, true);
                sw.WriteLine(DateTime.Now + " : " + message);
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            if (sw != null)
            {
                sw.Flush();
                sw.Close();
            }
        }

        public void SetIp(string ip)
        {
            configuredIp = ip;

            if (configuredIp == null || configuredIp == "")
                Status.DisableHeartBeat();
            else
                Status.EnableHeartBeat();
        }

        public string GetIp()
        {
            return configuredIp;
        }

        public void SetCredentials(string username, string password)
        {
            xbmcUsername = username;
            xbmcPassword = password;
        }

        public void SetConnectionTimeout(int timeOut)
        {
            connectionTimeout = timeOut;
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
