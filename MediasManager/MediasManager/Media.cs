using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using MediaManager;
using MediaManager.Library.NFO;

namespace MediaManager
{
    class Media
    {
        private ObservableCollection<Movie> movies = new ObservableCollection<Movie>();

        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
        }

        //private downloadManager dlMgr = new DownloadManagerImpl();

        public ObservableCollection<Movie> scanMovieDirs()
        {
            //dlMgr.CancelAllDownloads();
            MovieFolder[] paths = Settings.XML.Config.confMovie.MovieFolders;
            movies.Clear();
            if (paths != null)
            {
                foreach (MovieFolder mf in paths)
                {
                    if (mf.monitorFolder)
                    {

                    }
                    String directory = mf.path;
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    if (mf.containsFolders)
                    {
                        foreach (DirectoryInfo dinf in dir.GetDirectories())
                        {
                            foreach (String ext in Settings.XML.Config.confMovie.extensions)
                            {
                                foreach (FileInfo fileInfo in dinf.GetFiles(ext))
                                {
                                    if (!fileInfo.Name.ToLower().Contains("sample") || !Settings.XML.Config.confMovie.skipSample)
                                    {
                                        if (fileInfo != null) movies.Add(new Movie(fileInfo, mf));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (String ext in Settings.XML.Config.confMovie.extensions)
                        {
                            foreach (FileInfo fileInfo in dir.GetFiles(ext))
                            {
                                if (!fileInfo.Name.ToLower().Contains("sample") || !Settings.XML.Config.confMovie.skipSample)
                                {
                                    if (fileInfo != null) movies.Add(new Movie(fileInfo, mf));
                                }
                            }
                        }
                    }
                }
            }
            return movies;
        }
    }
}
