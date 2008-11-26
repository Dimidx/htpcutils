using System;
using System.Collections.Generic;
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

  

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        private void BTN_Charge_Click(object sender, RoutedEventArgs e)
        {
    
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Chargement";
            openFileDialog1.Filter = @"Fichiers image (*.jpg, *.png)|*.jpg;*.png"; ;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] _ListeFichier = openFileDialog1.FileNames;

            if (_ListeFichier.Length != 0)
            {

                string _Fichier = NiceCovers.FusionSave(_ListeFichier);
                if (_Fichier != "")
                {
                    DvdBox.Source = new BitmapImage(new Uri(_Fichier));
                }

                if (_ListeFichier.Length > 1)
                {
                    System.Windows.MessageBox.Show("Tous les covers sont générés.");

                }

            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bVide = new Bitmap(571, 720);
            Stream ms = new MemoryStream();
            bVide.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            PngBitmapEncoder png = new PngBitmapEncoder();
             
            png.Metadata = new BitmapMetadata("png"); //error - Property
            png.Metadata.SetQuery("/tEXt/", "Software");
            png.Metadata.SetQuery("/tEXt/Software", "moi");

            //png.Metadata.ApplicationName = "Moi";

            //png.Metadata.SetQuery("/tEXt/Software", "moi");

            png.Frames.Add(BitmapFrame.Create(ms));
            using (Stream stm = File.Create("foo.png"))
            {
                png.Save(stm);
                bVide = new Bitmap(stm);
            }

            bVide.Save("titi.png");

            ms.Close();


        }

        
    }
}
