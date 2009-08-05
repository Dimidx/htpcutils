using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using MediaManager.Library;
using MediaManager.Plugins;
namespace MediaManager
{
    public class Movie : INotifyPropertyChanged
    {

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

        public FileInfo fileInfo;
        public String SearchString;

        private String _MovieName;
        /// <summary>
        /// Nom du film
        /// </summary>
        public String MovieName
        {
            get { return _MovieName; }
            set { _MovieName = value; }
        }


        public int FileSize;
        //private ListViewSubItem status;
        private bool autoMode = false;
        private MovieFolder movieFolder;
        //public Film Infos;

        public Movie(FileInfo movie, MovieFolder mf) //, downloadManager dlMgr)
        {
            //Infos = new Film();
            fileInfo = movie;
            //downloadMgr = dlMgr;
            movieFolder = mf;
            _MovieName = cleanMovieFilename(movie.Name.Replace(movie.Extension, ""));
            //this.Infos.Titre = _MovieName;
            //this.Infos.TitreOriginal = _MovieName;
            //updateItem();
        }


        public Film updateItem()
        {
            Film _Film = new Film();

            foreach (IMMPluginImportExport plug in Master.Settings.PluginsImportExport)
            {
                try
                {
                    _Film = plug.Import(this.fileInfo);
                }
                catch ( Exception e)
                {
                    Console.WriteLine("UpdateItem" + Environment.NewLine + e.Message);
                }


            }

            return _Film;


        }

        /// <summary>
        /// Nettoie le nom du fichier
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private String cleanMovieFilename(String file)
        {
            String[][] replace = new String[][] { 
                new String[] { ".", " " },
                new String[] { "_", " " },
                new String[] { "-", " " },
                new String[] { " (", " " },
                new String[] { ") ", " " },
                new String[] { " bluray", " " },
                new String[] { " blu ray", " " },
                new String[] { " dvdrip", " " },
                new String[] { " hddvd", " " },
                new String[] { " dvd5", " " },
                new String[] { " dvdr", " " },
                new String[] { " dvd9", " " },
                new String[] { " bdrip", " " },
                new String[] { " 720p", " " },
                new String[] { " 1080p", " " },
                new String[] { " 720", " " },
                new String[] { " 1080", " " },
                new String[] { " dts-es", " " },
                new String[] { " dts", " " },
                new String[] { " ac3", " " },
                new String[] { " hdtv", " " },
                new String[] { " xvid", " " },
                new String[] { " x264", " " },
                new String[] { " h264", " " },
                new String[] { " wmv", " " },
                new String[] { " unrated", " " },
                new String[] { " oar", " " },
                new String[] { " directors cut", " " },
                new String[] { " dc", " " },
                new String[] { " dircut", " " },
                new String[] { " extended cut", " " },
                new String[] { " extended", " " },
                new String[] { " proper", " " },
                new String[] { " remastered", " " },
                new String[] { " repack", " " },
                new String[] { " dd5 1", " " },
                new String[] { " dd 5 1", " " },
                new String[] { " hd", " " },
                new String[] { " 5 1", " " },
                new String[] { " hd", " " },
                new String[] { " 2in1", " " },
                new String[] { " final cut", " " },
                new String[] { "&", " and " },
                new String[] { "   ", " " },
                new String[] { "  ", " " },
                new String[] { "  ", " " }
            };
            // remove group
            int pos = file.LastIndexOf("-");
            if (file.Length - pos < 10 && pos > 5) file = file.Substring(0, pos);

            // remove year

            string pat = @".\d{4}.";
            Regex reg = new Regex(pat);
            Match match = reg.Match(file);
            if (match.Success)
            {
                if (match.Index > 2) file = file.Substring(0, match.Index);
            }

            // remove unwanted text
            file = file.ToLower();
            foreach (String[] s in replace)
            {
                file = file.Replace(s[0], s[1]);
            }
            file = file.Trim();
            return file;
        }
    }

}

