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

        private void BTN_SaveClick(object sender, RoutedEventArgs e)
        {

        }

        public static Bitmap NiceCovers(string _FichierCover,Bitmap _DvdBox )
        {
            //Bitmap bDvdBox = new Bitmap(_DvdBox);
            Bitmap bCover = new Bitmap(@_FichierCover);
            Bitmap bVide = new Bitmap(NiceCovers_Creator.Properties.Resources.Vide);
            Graphics g = Graphics.FromImage(bVide);

            g.DrawImage(bCover, 81, 24, 458, 655);
            g.DrawImage(_DvdBox, 0, 0, 571, 720);

            g.Save();
            //bVide.Save(@"c:\test.png");


            //BitmapImage toto = new BitmapImage(new Uri(@"c:\test.png"));

            //DvdBox.Source = bVide.;

            return bVide;
        }



        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        private void BTN_Charge_Click(object sender, RoutedEventArgs e)
        {
            

            Fusion("BLANC");
        }

        private void Fusion(string _typecover)
        {
            string _suffixe;
            Bitmap _dvdbox;

            switch (_typecover)
            {
                case "BLANC":
                    _suffixe = "";
                    _dvdbox = NiceCovers_Creator.Properties.Resources.dvdbox;
                    break;
                case "BLUE":
                    _suffixe = "Blue";
                    _dvdbox = NiceCovers_Creator.Properties.Resources.dvdboxbleu;
                    break;
                default:
                    _suffixe = "";
                    _dvdbox = NiceCovers_Creator.Properties.Resources.dvdbox;
                    break;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Chargement";
            openFileDialog1.Filter = @"Fichiers image (*.jpg, *.png)|*.jpg;*.png"; ;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] _ListeFichier = openFileDialog1.FileNames;

            if (_ListeFichier.Length != 0)
            {
                foreach (string _FileCover in _ListeFichier)
                {
                    Bitmap _NiceCovers = NiceCovers(_FileCover, _dvdbox);
                    FileInfo _file = new FileInfo(_FileCover);
                    string _NomFichier = _file.DirectoryName + "\\" + _file.Name.Replace(_file.Extension, "") + "_NiceCoversBlue"+_suffixe+".png";
                    _NiceCovers.Save(_NomFichier);
                    DvdBox.Source = new BitmapImage(new Uri(_NomFichier));
                }
                if (_ListeFichier.Length > 1)
                {
                    System.Windows.MessageBox.Show("Tous les covers sont générés.");

                }

            }

        }


            private void BTN_ChargeBlue_Click(object sender, RoutedEventArgs e)
        {

            Fusion("BLUE");

        }
    }
}
