using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
//using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Reflection;
using NiceCovers_Library;

namespace NiceCovers_Creator
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public Window1()
        {
            InitializeComponent();

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        public void LinkClicked(object sender, RoutedEventArgs e)
        {
            Hyperlink senderLink = sender as Hyperlink;
            if (senderLink != null)
            {
                Process.Start(senderLink.NavigateUri.ToString());
            }
        }

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        private void BTN_About_Click(object sender, RoutedEventArgs e)
        {


            // code pour vérifier la version :
            Assembly _Assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo  _fileinfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            
    
            string version = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Replace("Version=", "").Trim();
            LIB_Titre.Content = _fileinfo.ProductName;
            LIB_Version.Content = _fileinfo.ProductVersion;
            LIB_Auteur.Content = _fileinfo.CompanyName;
            LIB_Description.Text = _fileinfo.Comments;
            

            


        }

        private void BTN_Charge_Click(object sender, RoutedEventArgs e)
        {
            string _type = "MOVIE";

            if (INT_Movie.IsChecked == true)
            {
                _type = "MOVIE";
            }
            else
            {
                _type = "MUSIC";
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Chargement";
            openFileDialog1.Filter = @"Fichiers image (*.jpg, *.png)|*.jpg;*.png"; ;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] _ListeFichier = openFileDialog1.FileNames;
            string _Result = "";
            if (_ListeFichier.Length != 0)
            {
                //Affiche.Source = new BitmapImage(new Uri(_ListeFichier[_ListeFichier.Length-1]));
                foreach (string _Fichier in _ListeFichier)
                {
                    _Result = NiceCovers.FusionSave(_Fichier, _type);

                    if (_Result == "")
                    {
                        System.Windows.MessageBox.Show("Impossible de générer le fichier "+_Fichier,"Erreur", MessageBoxButton.OK,MessageBoxImage.Error);
                    }

                    
                }
                if (_Result != "")
                {
                    Img_NiceCover.Source = new BitmapImage(new Uri(_Result));
                }

                if (_ListeFichier.Length > 1)
                {
                    System.Windows.MessageBox.Show("Tous les covers sont générés.");

                }
                

            }

        }



        private void BTN_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();


        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (INT_Movie.IsChecked == true)
            {

                Img_NiceCover.Source = Convert(NiceCovers_Creator.Properties.Resources.dvdbox);

            }
                else
            {
                Img_NiceCover.Source = Convert(NiceCovers_Creator.Properties.Resources.CDBox);

            }
 
        }


         public BitmapImage Convert(object value)
        {
            BitmapImage ImgSource = null;

            System.Drawing.Image image = value as System.Drawing.Image;

            if (image != null)
            {
                ImgSource = new BitmapImage();
                ImgSource.BeginInit();

                ImgSource.StreamSource = new MemoryStream(ConvertImageToByteArray(image));

                ImgSource.EndInit();
            }

            return ImgSource;
        }

        /// <summary>
        /// Convert a System.Drawing.Image to a byte array
        /// </summary>
        /// <param name="img">The System.Drawing.Image to convert in a byte array</param>
        /// <returns>A byte array</returns>
        private byte[] ConvertImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
