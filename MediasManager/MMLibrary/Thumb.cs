using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
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
        private Image m_Miniature;

        /// <summary>
        /// L'image de la miniature
        /// </summary>
        [XmlIgnore]
        public Image Miniature
        {
            get 
            {
                if (m_Miniature == null) GetMiniature();
                return m_Miniature; 
            
            }
            
            
            //set { m_Miniature = value; }
            
        }

        private string m_URLMiniature;
        /// <summary>
        /// URL du fichier miniature
        /// </summary>
        public string URLMiniature
        {
            get { return m_URLMiniature; }
            set { m_URLMiniature = value; }
        }

        private int m_Hauteur;
        /// <summary>
        /// Hauteur de l'image
        /// </summary>
        public int Hauteur
        {
            get { return m_Hauteur; }
            //set { m_Hauteur = value; }
        }

        private int m_Largeur;
        /// <summary>
        /// Largeur de l'image
        /// </summary>
        public int Largeur
        {
            get { return m_Largeur; }
            //set { m_Hauteur = value; }
        }

        private string m_URLImage;
        /// <summary>
        /// URL du fichier image
        /// </summary>
        public string URLImage
        {
            get { return m_URLImage; }
            set { m_URLImage = value; }
        }

        private int m_ProgressImage;
        /// <summary>
        /// Pourcentage chargé de l'image
        /// </summary>
        public int ProgressImage
        {
            get { return m_ProgressImage; }
            set { m_ProgressImage = value; }
        }

        private int m_ProgressMiniature;
        /// <summary>
        /// Pourcentage chargé de la miniature
        /// </summary>
        public int ProgressMiniature
        {
            get { return m_ProgressMiniature; }
            set { m_ProgressMiniature = value; }
        }

        private Image m_Image;
        /// <summary>
        /// L'image
        /// </summary>
        [XmlIgnore]
        public Image Image
        {
            get 
            {
                if (m_Image == null) GetImage();
                return m_Image; 
            
            }
            //set { m_Image = value; }
        }

        /// <summary>
        /// Type d'image
        /// </summary>
        public enum ImageType {Miniature=1,Image=2}

        /// <summary>
        /// Sauve l'image sur le disque
        /// </summary>
        /// <param name="TypeImage">Type de l'image à télécharger</param>
        /// <param name="Path">Chemin où l'image sera enregistrée</param>
        public void SaveImage(ImageType TypeImage,string Path)
        {

            try
            {
                switch (TypeImage)
                {
                    case ImageType.Image:
                        if (this.m_Image != null)
                        {
                            if (File.Exists(Path) == true) File.Delete(Path);
                            this.m_Image.Save(Path);
                        }


                        break;
                    case ImageType.Miniature:
                        if (this.m_Miniature != null)
                        {
                            if (File.Exists(Path) == true) File.Delete(Path);
                            this.m_Miniature.Save(Path);
                        }
                        break;
                    default:

                        break;

                }
            }
            catch (Exception)
            {
                
            }

        }

        /// <summary>
        /// Télécharge l'image
        /// </summary>
        private void GetMiniature()
        {
            try
            {
                WebClient client = new WebClient();

                #region Proxy
                //WebProxy wProxy = new WebProxy("10.126.71.12", 80);
                //wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
                //client.Proxy = wProxy;
                #endregion

                //Téléchargement
                client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(Miniature_DownloadDataCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Miniature_DownloadProgressChanged);
                if (this.m_URLMiniature != null)
                {
                    client.DownloadDataAsync(new Uri(this.m_URLMiniature));
                }
                else
                {
                    client.DownloadDataAsync(new Uri(this.m_URLImage));
                }

            }
            catch (Exception ex)
            {

            }

        }


        /// <summary>
        /// Télécharge l'image
        /// </summary>
        private void GetImage()
        {
            try
            {
                WebClient client = new WebClient();

                #region Proxy
                //WebProxy wProxy = new WebProxy("10.126.71.12", 80);
                //wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
                //client.Proxy = wProxy;
                #endregion

                //Téléchargement
                client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(Image_DownloadDataCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Image_DownloadProgressChanged);
                client.DownloadDataAsync(new Uri(this.m_URLImage));

            }
            catch (Exception ex)
            {
                
            }

        }

        void Image_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.m_ProgressImage = e.ProgressPercentage;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ProgressImage"));
            }
        }

        private BitmapImage m_BImage;
        /// <summary>
        /// L'image
        /// </summary>
        [XmlIgnore]
        public BitmapImage BImage
        {
            get
            {
                if (m_Image == null) GetImage();
                return m_BImage;

            }
            //set { m_Image = value; }
        }

        void Image_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            WebClient c = (WebClient)sender;
            c.Dispose();

            byte[] _result = e.Result;

            MemoryStream ms = new MemoryStream(_result);
            this.m_BImage = new BitmapImage();
            this.m_BImage.BeginInit();
            this.m_BImage.UriSource = new Uri("file:///D:/20090311122354158_0001.jpg", UriKind.RelativeOrAbsolute);
            this.m_BImage.EndInit();
            
            
            this.m_Image = Image.FromStream(ms, true, true);

            this.m_Hauteur = m_Image.Height;
            this.m_Largeur = m_Image.Width;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("BImage"));
                PropertyChanged(this, new PropertyChangedEventArgs("Image"));
                PropertyChanged(this, new PropertyChangedEventArgs("Hauteur"));
                PropertyChanged(this, new PropertyChangedEventArgs("Largeur"));

            }
        }

        void Miniature_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.m_ProgressMiniature = e.ProgressPercentage;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ProgressMiniature"));
            }
        }

        void Miniature_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            WebClient c = (WebClient)sender;
            c.Dispose();

            byte[] _result = e.Result;

            MemoryStream ms = new MemoryStream(_result);
            this.m_Miniature = Image.FromStream(ms, true, true);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Miniature"));
            }
        }

        public Thumb()
        {

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
