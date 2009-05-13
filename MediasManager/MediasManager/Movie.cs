using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using MediaManager.Library.NFO;
using MediaManager;
using System.Text.RegularExpressions;

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

        private Path _paths;
        public Path Paths
        {
            get { return _paths; }
        }


        public bool HasFanart;
        public bool HasPoster;
        public bool HasNfo;
        public bool HasFolderJpg;
        public bool HasValidNfo;

        public String SearchString;

        private String moviename;
       

        public String MovieName
        {
            get { return moviename; }
            set { moviename = value; }
        }
        private string _MediaSource = "Images/flags/source/dvd.png";
        public string MediaSource
        {
            get { return _MediaSource; }
            set { _MediaSource = value; }
        }

        public int FileSize;

        //private downloadManager downloadMgr;

        //internal downloadManager DownloadMgr
        //{
        //    get { return downloadMgr; }
        //}
        private NfoMovie nfoMov = new NfoMovie();
        //private ListViewSubItem status;
        private bool autoMode = false;
        private MovieFolder movieFolder;

        public NfoMovie Nfo
        {
            get { return nfoMov; }
        }

        public Movie(FileInfo movie, MovieFolder mf) //, downloadManager dlMgr)
        {
            fileInfo = movie;
            //downloadMgr = dlMgr;
            movieFolder = mf;
            _paths = new Path(movie);

            updateItem();
        }

        public bool validID()
        {
            if (HasValidNfo)
            {
                string pat = @"(tt\d{7})";
                Regex reg = new Regex(pat);
                Match match = reg.Match(nfoMov.Id);
                return match.Success;
            }
            return false;
        }

        public void deleteNfo()
        {

            foreach (String s in _paths.ValidNfoPaths())
            {
                File.Delete(s);
                SearchString = null;
                updateItem();
            }
        }

        public void updateItem()
        {
            // determine missing fields...

            HasNfo = (_paths.ValidNfoPaths().Count > 0);
            HasFanart = (_paths.ValidFanartPaths().Count > 0);
            HasPoster = (_paths.ValidPosterPaths().Count > 0);
            HasFolderJpg = File.Exists(_paths.FolderPath);

            bool nfoValid = false;
            // does it load?
            if (HasNfo)
            {
                try
                {
                    if (File.Exists(_paths.NfoPath))
                    {
                        nfoMov = NfoFile.getNfoMovie(_paths.NfoPath);
                        nfoValid = (nfoMov != null);
                    }
                    if (!nfoValid)
                    {
                        foreach (String s in _paths.ValidNfoPaths())
                        {
                            nfoMov = NfoFile.getNfoMovie(s);
                            nfoValid = ((nfoMov != null) || nfoValid);
                        }
                    }
                }
                catch (Exception)
                {
                    HasNfo = false;
                }
            }
            HasValidNfo = nfoValid;

            if (Settings.XML.Config.confMovie.useFolderForSearch && movieFolder.containsFolders)
            {
                MovieName = _paths.ShortDir;
            }
            else
            {
                MovieName = fileInfo.Name;
            }
            if (Settings.XML.Config.confMovie.cleanFilename) MovieName = cleanMovieFilename(MovieName);

            // Search / nfo : override earlier moviename...
            if (nfoValid)
            {
                MovieName = nfoMov.Title;
            }

            if (SearchString == null) SearchString = MovieName;

            // Filesize
            FileSize = Convert.ToInt32(fileInfo.Length / (1024 * 1024));

            //Source
            if (fileInfo.Name.ToLower().Contains("bluray") | fileInfo.Name.ToLower().Contains("bluray") | fileInfo.Name.ToLower().Contains("brrip"))
            {
                _MediaSource = "Images/flags/source/bluray.png";
            }
            if (fileInfo.Name.ToLower().Contains("sddvd") | fileInfo.Name.ToLower().Contains("dvdrip") | fileInfo.Name.ToLower().Contains("dvd"))
            {
                _MediaSource = "Images/flags/source/dvd.png";
            }
            if (fileInfo.Name.ToLower().Contains("hdtv"))
            {
                _MediaSource = "Images/flags/source/hdtv.png";
            }
            if (fileInfo.Name.ToLower().Contains("hddvd"))
            {
                _MediaSource = "Images/flags/source/hddvd.png";
            }


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

        public class Path
        {
            private String[] nfoPaths;
            private String[] posterPaths;
            private String[] fanartPaths;
            private String folderPath;
            private String fullDir;
            private String shortDir;

            public Path(FileInfo fi)
            {
                String ext = fi.Extension;
                nfoPaths = new String[2];
                nfoPaths[0] = fi.DirectoryName + "\\movie.nfo";
                nfoPaths[1] = fi.FullName.Replace(ext, ".nfo");

                posterPaths = new String[2];
                posterPaths[0] = fi.DirectoryName + "\\movie.tbn";
                posterPaths[1] = fi.FullName.Replace(ext, ".tbn");

                fanartPaths = new String[2];
                fanartPaths[0] = fi.DirectoryName + "\\fanart.jpg";
                fanartPaths[1] = fi.FullName.Replace(ext, "-fanart.jpg");

                folderPath = fi.DirectoryName + "\\folder.jpg";

                fullDir = fi.DirectoryName;
                shortDir = fi.Directory.Name;
            }

            public String NfoPath
            {
                get { return Settings.XML.Config.confMovie.saveAsMovie ? nfoPaths[0] : nfoPaths[1]; }
            }

            public String PosterPath
            {
                get { return Settings.XML.Config.confMovie.saveAsMovie ? posterPaths[0] : posterPaths[1]; }
                //get { return posterPaths[1]; }
            }

            public String FanartPath
            {
                get { return Settings.XML.Config.confMovie.saveAsMovie ? fanartPaths[0] : fanartPaths[1]; }
            }

            public String FolderPath
            {
                get { return folderPath; }
            }

            public String FullDir
            {
                get { return fullDir; }
            }

            public String ShortDir
            {
                get { return shortDir; }
            }

            public List<String> ValidNfoPaths()
            {
                List<String> result = new List<String>();
                foreach (String s in nfoPaths)
                {
                    if (File.Exists(s)) result.Add(s);
                }
                return result;
            }

            public List<String> ValidFanartPaths()
            {
                List<String> result = new List<String>();
                foreach (String s in fanartPaths)
                {
                    if (File.Exists(s)) result.Add(s);
                }
                return result;
            }

            public List<String> ValidPosterPaths()
            {
                List<String> result = new List<String>();
                foreach (String s in posterPaths)
                {
                    if (File.Exists(s)) result.Add(s);
                }
                return result;
            }


        }
    }

