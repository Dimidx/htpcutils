using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using MediaManager;
using MediaManager.Library.NFO;
using System.Threading;


namespace MediaManager
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection()
        {
        }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the CollectionChanged event on the current thread
                RaiseCollectionChanged(e);
            }
            else
            {
                // Post the CollectionChanged event on the creator thread
                _synchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(e);
            }
            else
            {
                // Post the PropertyChanged event on the creator thread
                _synchronizationContext.Post(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }


    public class MovieCollection : AsyncObservableCollection<Movie> 
    {
 
        public void scanMovieDirs() //object sender)
        {
            //dlMgr.CancelAllDownloads();
            //BackgroundWorker BackWork = new BackgroundWorker();
            //if (sender != null)
            //{
            //    BackWork = sender as BackgroundWorker;
            //}

            ObservableCollection<MovieFolder> paths = Settings.XML.Config.confMovie.MovieFolders;

            this.Clear();
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
                                        if (fileInfo != null)
                                        {
                                            //if (sender != null) BackWork.ReportProgress(0, fileInfo.Name);
                                            this.Add(new Movie(fileInfo, mf));
                                        }

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
                                    if (fileInfo != null) this.Add(new Movie(fileInfo, mf));
                                }
                            }
                        }
                    }
                }
            }
            //return this;
        }
    }
}
