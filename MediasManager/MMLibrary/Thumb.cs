using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Net;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Threading;

namespace MediaManager.Library
{




    /// <summary>
    /// Représente une image avec sa miniature 
    /// </summary>
    [Serializable]
    public class Thumb : INotifyPropertyChanged
    {

        #region Private

        private string defaultCacheDir = System.Environment.CurrentDirectory + @"\Cache\Images\";
        private bool _IsLoading = false;
        private string m_URLMiniature = "";
        private string m_URLImage = "";
        private BitmapImage _Image;
        private BitmapImage _Miniature;
        private double _Hauteur;
        private double _Largeur;
        private string _FichierCache;
        private BackgroundWorker bw = new BackgroundWorker();

        #endregion

        #region Public
        /// <summary>
        /// image
        /// </summary>
        public BitmapImage Image
        {
            get
            {
                if (_Image == null && !IsLoading)
                {
                    GetImage();
                }
                return _Image;

            }
            set
            {
                _Image = value;
                if (_Image != null) _Image.Freeze();
                OnPropertyChanged("Image");
            }
        }

        /// <summary>
        /// Miniature
        /// </summary>
        public BitmapImage Miniature
        {
            get
            {
                if (_Image == null && !IsLoading)
                {
                    GetImage();
                }
                return _Miniature;

            }
            set
            {
                _Miniature = value;
                if (_Miniature != null) _Miniature.Freeze();
                OnPropertyChanged("Miniature");
            }
        }

        /// <summary>
        /// IsDownloading
        /// </summary>
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged("IsLoading"); }
        }

        /// <summary>
        /// Fichier Cache
        /// </summary>
        public string FichierCache
        {
            get { return _FichierCache; }
        }

        /// <summary>
        /// URL du fichier miniature
        /// </summary>
        public string URLMiniature
        {
            get
            {
                if (m_URLMiniature == null)
                {
                    return m_URLImage;
                }
                else
                {
                    return m_URLMiniature;
                }
            }
            set
            {
                m_URLMiniature = value;
                OnPropertyChanged("URLMiniature");
            }
        }

        /// <summary>
        /// Hauteur de l'image
        /// </summary>
        public double Hauteur
        {
            get { return _Hauteur; }
        }

        /// <summary>
        /// Largeur de l'image
        /// </summary>
        public double Largeur
        {
            get { return _Largeur; }
        }

        /// <summary>
        /// URL du fichier image
        /// </summary>
        public string URLImage
        {
            get { return m_URLImage; }
            set
            {
                m_URLImage = value;
                _FichierCache = defaultCacheDir + m_URLImage.Replace(@"\", "_").Replace(@"/", "_").Replace(":", "_").Replace("?","_");
                OnPropertyChanged("URLImage");
            }
        }

        /// <summary>
        /// Si l'image est en cache
        /// </summary>
        public bool IsCached
        {
            get { return File.Exists(_FichierCache); }
        }

        /// <summary>
        /// Si l'image est locale
        /// </summary>
        public bool IsLocal
        {
            get 
            {
                if (m_URLImage != "")
                {
                    try
                    {
                        Uri _uri = new Uri(m_URLImage, UriKind.RelativeOrAbsolute);
                        string _protocole = _uri.GetLeftPart(UriPartial.Scheme);
                        return _protocole.Contains("file://");
                    }
                    catch (Exception)
                    {

                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
        }



        #endregion

        private void SaveImage()
        {
            if (!IsCached)
            {
                using (FileStream stream = new FileStream(_FichierCache, FileMode.Create))
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_Image));
                    encoder.Save(stream);
                    stream.Close();
                }
            }
        }


        /// <summary>
        /// Télécharge l'image
        /// </summary>
        private void GetImage()
        {
            IsLoading = true;
            if (bw.IsBusy)
            {
                bw.CancelAsync();
                while (bw.IsBusy)
                {
                    Thread.Sleep(1);
                }
            }

            bw = new BackgroundWorker();

            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsLoading = false;
            Miniature = _Miniature;
            Image = _Image;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (URLImage == "")
            {

            }
            else
            {
                if (IsCached || IsLocal)
                {
                    _Image = new BitmapImage();
                    _Image.BeginInit();
                    _Image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
                    _Image.CacheOption = BitmapCacheOption.OnLoad;
                    if (IsCached) _Image.UriSource = new Uri(_FichierCache,UriKind.RelativeOrAbsolute);
                    if (IsLocal) _Image.UriSource = new Uri(m_URLImage, UriKind.RelativeOrAbsolute);
                    _Image.EndInit();
                    _Image.Freeze();
                    
                }
                else
                {

                    try
                    {
                        WebClient client = new WebClient();

                        //#region Proxy
                        //WebProxy wProxy = new WebProxy("10.126.71.12", 80);
                        //wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
                        //client.Proxy = wProxy;
                        //#endregion

                        //Téléchargement
                        //client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(Image_DownloadDataCompleted);
                        //client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Image_DownloadProgressChanged);
                        byte[] _result = client.DownloadData(new Uri(this.m_URLImage));
                        client.Dispose();
                        MemoryStream ms = new MemoryStream(_result);
                        _Image = new BitmapImage();
                        _Image.BeginInit();
                        _Image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
                        //_Image.DecodePixelWidth = 300;
                        _Image.CacheOption = BitmapCacheOption.OnLoad;
                        _Image.StreamSource = ms;
                        _Image.EndInit();
                        _Image.Freeze();
                        if (!IsLocal) SaveImage();
                    }
                    catch (Exception)
                    {
                        
                        //throw;
                    }



                }

                //Création de la miniature
                _Miniature = new BitmapImage();
                _Miniature.BeginInit();
                _Miniature.CreateOptions = BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.DelayCreation;
                _Miniature.CacheOption = BitmapCacheOption.OnDemand;
                _Miniature.DecodePixelWidth = 100;
                _Miniature.UriSource = new Uri(_FichierCache, UriKind.RelativeOrAbsolute);
                _Miniature.EndInit();
                _Miniature.Freeze();

                //Thread.Sleep(5000);
            }


        }

        public Thumb()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_URLImage"></param>
        /// <param name="_URLMiniature"></param>
        public Thumb(string _URLImage, string _URLMiniature)
        {
            URLImage = _URLImage;
            URLMiniature = _URLMiniature;
        }

        public Thumb(string _URLImage)
        {
            URLImage = _URLImage;
        }

        ~Thumb()
        {

            if (bw.IsBusy)
            {
                bw.CancelAsync();
                while (bw.IsBusy)
                {
                    Thread.Sleep(10);
                }
            }
            this.Image = null;
            this.Miniature = null;
            this._Image = null;
            this._Miniature = null;
            IsLoading = false;
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
