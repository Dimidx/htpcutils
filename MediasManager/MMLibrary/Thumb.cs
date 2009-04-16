using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            get { return m_Miniature; }
            set { m_Miniature = value; }
            
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


        private string m_URLImage;
        /// <summary>
        /// URL du fichier image
        /// </summary>
        public string URLImage
        {
            get { return m_URLImage; }
            set { m_URLImage = value; }
        }

        private Image m_Image;
        /// <summary>
        /// L'image
        /// </summary>
        [XmlIgnore]
        public Image Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }

        /// <summary>
        /// Type d'image
        /// </summary>
        public enum ImageType {Miniature=1,Image=2}

        /// <summary>
        /// Télécharge l'image en mémoire
        /// </summary>
        /// <param name="TypeImage">Type de l'image à télécharger</param>
        /// <example>
        /// <code>MonThumb.LoadImage(ImageType.Miniature);</code>
        /// </example>
        public void LoadImage(ImageType TypeImage)
        {

            switch (TypeImage)
            {
                case ImageType.Image:
                    if (this.m_URLImage != null && this.m_URLImage != "")
                    {
                        this.m_Image = GetImage(new Uri(this.m_URLImage));
                        
                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Image"));
                        }
                    }
                    break;
                case ImageType.Miniature:
                    if (this.m_URLMiniature != null && this.m_URLMiniature != "")
                    {
                        this.m_Miniature = GetImage(new Uri(this.m_URLMiniature));
                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Miniature"));
                        }
                    }
                    break;
                default:
                    
                    break;

            }
            
        }

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
        /// Télécharge une image
        /// </summary>
        /// <param name="URL">URL de l'image à télécharger</param>
        /// <returns></returns>
        private Image GetImage(Uri URL)
        {
            Image _image = null;
            try
            {
                WebClient client = new WebClient();
 
                //Téléchargement
                byte[] _result = client.DownloadData(URL);
                MemoryStream ms = new MemoryStream(_result);
                _image = Image.FromStream(ms, true, true);

                //Libère le client web
                client.Dispose();


            }
            catch (Exception ex)
            {
                
            }
            return _image;
        }

        public Thumb()
        {

        }

        /// <summary>
        /// Se produit lorsque un membre est modifié
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        

    }
}
