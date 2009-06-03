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


namespace MediaManager.Library
{
    /// <summary>
    /// Représente une image avec sa miniature 
    /// </summary>
    [Serializable]
    public class Thumb : INotifyPropertyChanged
    {
        //private BitmapImage m_Miniature;

        ///// <summary>
        ///// L'image de la miniature
        ///// </summary>
        //[XmlIgnore]
        //public BitmapImage Miniature
        //{
        //    get 
        //    {
        //        if (m_Miniature == null) GetMiniature();

        //            return m_Miniature;


        //    }


        //    //set { m_Miniature = value; }

        //}

        private string m_URLMiniature;
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
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("URLMiniature"));
            }
        }

        //private double m_Hauteur;
        ///// <summary>
        ///// Hauteur de l'image
        ///// </summary>
        //public double Hauteur
        //{
        //    get { return m_Hauteur; }
        //    //set { m_Hauteur = value; }
        //}

        //private double m_Largeur;
        ///// <summary>
        ///// Largeur de l'image
        ///// </summary>
        //public double Largeur
        //{
        //    get { return m_Largeur; }
        //    //set { m_Hauteur = value; }
        //}

        private string m_URLImage;
        /// <summary>
        /// URL du fichier image
        /// </summary>
        public string URLImage
        {
            get { return m_URLImage; }
            set
            {
                m_URLImage = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("URLImage"));
            }
        }

        //private int m_ProgressImage;
        ///// <summary>
        ///// Pourcentage chargé de l'image
        ///// </summary>
        //public int ProgressImage
        //{
        //    get { return m_ProgressImage; }
        //    set { m_ProgressImage = value; }
        //}

        //private int m_ProgressMiniature;
        ///// <summary>
        ///// Pourcentage chargé de la miniature
        ///// </summary>
        //public int ProgressMiniature
        //{
        //    get { return m_ProgressMiniature; }
        //    set { m_ProgressMiniature = value; }
        //}

        ///// <summary>
        ///// Type d'image
        ///// </summary>
        //public enum ImageType {Miniature=1,Image=2}

        /// <summary>
        /// Sauve l'image sur le disque
        /// </summary>
        /// <param name="TypeImage">Type de l'image à télécharger</param>
        /// <param name="Path">Chemin où l'image sera enregistrée</param>
        //public void SaveImage(ImageType TypeImage,string Path)
        //{

        //    try
        //    {
        //        switch (TypeImage)
        //        {
        //            case ImageType.Image:
        //                if (this.m_Image != null)
        //                {
        //                    if (File.Exists(Path) == true) File.Delete(Path);
        //                    this.m_Image.Save(Path);
        //                }


        //                break;
        //            case ImageType.Miniature:
        //                if (this.m_Miniature != null)
        //                {
        //                    if (File.Exists(Path) == true) File.Delete(Path);
        //                    this.m_Miniature.Save(Path);
        //                }
        //                break;
        //            default:

        //                break;

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }

        //}

        ///// <summary>
        ///// Télécharge l'image
        ///// </summary>
        //private void GetMiniature()
        //{

        //        //WebClient client = new WebClient();

        //        //#region Proxy
        //        //WebProxy wProxy = new WebProxy("10.126.71.12", 80);
        //        //wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
        //        //client.Proxy = wProxy;
        //        //#endregion

        //        //Téléchargement
        //        //client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(Miniature_DownloadDataCompleted);
        //        //client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Miniature_DownloadProgressChanged);

        //    try
        //    {
        //        this.m_Miniature = new BitmapImage();

        //        this.m_Miniature.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(m_Miniature_DownloadProgress);
        //        this.m_Miniature.DownloadCompleted += new EventHandler(m_Miniature_DownloadCompleted);
        //        this.m_Miniature.DownloadFailed += new EventHandler<ExceptionEventArgs>(m_Miniature_DownloadFailed);
        //        this.m_Miniature.BeginInit();



        //        if (this.m_URLMiniature != null)
        //        {
        //            this.m_Miniature.UriSource = new Uri(this.m_URLMiniature, UriKind.RelativeOrAbsolute);
        //        }
        //        else
        //        {
        //            this.m_Miniature.UriSource = new Uri(this.m_URLImage, UriKind.RelativeOrAbsolute);
        //        }
        //        this.m_Miniature.EndInit();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        //void m_Miniature_DownloadFailed(object sender, ExceptionEventArgs e)
        //{
        //    this.m_Miniature = null;
        //}

        //void m_Miniature_DownloadCompleted(object sender, EventArgs e)
        //{
        //    //if (this.m_Miniature != null)
        //    //PropertyChanged(this, new PropertyChangedEventArgs("Miniature"));

        //}

        //void m_Miniature_DownloadProgress(object sender, DownloadProgressEventArgs e)
        //{
        //    this.m_ProgressMiniature = e.Progress;
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs("ProgressMiniature"));
        //    }
        //}


        /// <summary>
        /// Télécharge l'image
        /// </summary>
        //private void GetImage()
        //{

        //        WebClient client = new WebClient();

        //        //#region Proxy
        //        //WebProxy wProxy = new WebProxy("10.126.71.12", 80);
        //        //wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
        //        //client.Proxy = wProxy;
        //        //#endregion

        //        //Téléchargement
        //        client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(Image_DownloadDataCompleted);
        //        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Image_DownloadProgressChanged);
        //        client.DownloadDataAsync(new Uri(this.m_URLImage));
        //    try
        //    {
        //        this.m_Image = new BitmapImage();

        //        this.m_Image.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(m_Image_DownloadProgress);
        //        this.m_Image.DownloadCompleted += new EventHandler(m_Image_DownloadCompleted);
        //        this.m_Image.DownloadFailed += new EventHandler<ExceptionEventArgs>(m_Image_DownloadFailed);
        //        this.m_Image.BeginInit();
        //        this.m_Image.UriSource = new Uri(this.m_URLImage, UriKind.RelativeOrAbsolute);
        //        this.m_Image.EndInit();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}

        //void Image_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //void m_Image_DownloadFailed(object sender, ExceptionEventArgs e)
        //{
        //    this.m_Image = null;
        //}

        //void m_Image_DownloadCompleted(object sender, EventArgs e)
        //{

        //    //if (PropertyChanged != null)
        //    //{
        //    if (this.m_Image != null)
        //    {
        //        this.m_Largeur = this.m_Image.Width;
        //        this.m_Hauteur = this.m_Image.Height;
        //        //PropertyChanged(this, new PropertyChangedEventArgs("Image"));

        //    }
        //        //PropertyChanged(this, new PropertyChangedEventArgs("Hauteur"));
        //        //PropertyChanged(this, new PropertyChangedEventArgs("Largeur"));
        //    //}
        //}

        //void m_Image_DownloadProgress(object sender, DownloadProgressEventArgs e)
        //{
        //    this.m_ProgressImage = e.Progress;
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs("ProgressImage"));
        //    }
        //}


        //private BitmapImage m_Image;
        ///// <summary>
        ///// L'image
        ///// </summary>
        //[XmlIgnore]
        //public BitmapImage Image
        //{
        //    get
        //    {
        //        if (m_Image == null) GetImage();
        //        //    return new BitmapImage();
        //        //}
        //        //else
        //        //{  
        //            return m_Image;
        //        //}
        //    }
        //    //set { m_Image = value; }
        //}

        //void Image_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{
        //    WebClient c = (WebClient)sender;
        //    c.Dispose();

        //    //byte[] _result = e.Result;

        //    //MemoryStream ms = new MemoryStream(_result);

        //    BitmapImage bimage = new BitmapImage();

        //    //bimage.SetSource(e.Result);

        //    //this.Image.Source = bimage;

        //    //this.m_Image = Image.FromStream(ms, true, true);

        //    //this.m_Hauteur = m_Image.Height;
        //    //this.m_Largeur = m_Image.Width;

        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs("BImage"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("Image"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("Hauteur"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("Largeur"));

        //    }
        //}

        //void Miniature_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    this.m_ProgressMiniature = e.ProgressPercentage;
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs("ProgressMiniature"));
        //    }
        //}

        //void Miniature_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{
        //    WebClient c = (WebClient)sender;
        //    c.Dispose();

        //    byte[] _result = e.Result;

        //    MemoryStream ms = new MemoryStream(_result);
        //    this.m_Miniature = Image.FromStream(ms, true, true);

        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs("Miniature"));
        //    }
        //}

        public Thumb()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_URLImage"></param>
        /// <param name="_URLMiniature"></param>
        public Thumb(string _URLImage,string _URLMiniature)
        {
            m_URLImage = _URLImage;
            m_URLMiniature = _URLMiniature;
        }

        public Thumb(string _URLImage)
        {
            m_URLImage = _URLImage;
        }

        ~Thumb()
        {

        }

        /// <summary>
        /// Se produit lorsque un membre est modifié
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;



    }
}
