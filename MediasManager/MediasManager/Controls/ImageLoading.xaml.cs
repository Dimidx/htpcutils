using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace MediaManager.Controls
{
    /// <summary>
    /// Logique d'interaction pour ImageLoading.xaml
    /// </summary>
    public partial class ImageLoading : UserControl
    {
        private string defaultCacheDir = System.Environment.CurrentDirectory + @"\Cache\Images\";
        private string fichierlocal = "";
        public ImageLoading()
        {

            InitializeComponent();
            this.StopAnimation();
        }
        //public BitmapImage Source
        //{

        //    get { return (BitmapImage)GetValue(SourceProperty); }

        //    set
        //    {
        //        var bmp = new BitmapImage();
        //        bmp.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(value_DownloadProgress);
        //        bmp.BeginInit();
        //        bmp.UriSource = new Uri(value, UriKind.RelativeOrAbsolute);
        //        bmp.EndInit();
        //        //source.Source = bmp;

        //        SetValue(SourceProperty, bmp);



        //        this.Image.Source = bmp;

        //        this.BeginAnimation();

        //    }

        //}

        public string Source
        {

            get { return (string)GetValue(SourceProperty); }

            set
            {

                SetValue(SourceProperty, value);
                this.Image.Source = null;
                this.BeginAnimation();
                var bmp = new BitmapImage();
                bmp.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(value_DownloadProgress);
                bmp.DownloadCompleted += new EventHandler(bmp_DownloadCompleted);
                bmp.BeginInit();
                fichierlocal = defaultCacheDir + "\\" + (string)value.Replace(@"\", "_").Replace(@"/", "_").Replace(":", "_");
                if (File.Exists(fichierlocal))
                {
                    this.StopAnimation();
                    bmp.UriSource = new Uri(fichierlocal, UriKind.RelativeOrAbsolute);

                }
                else
                {
                    bmp.UriSource = new Uri((string)value, UriKind.RelativeOrAbsolute);

                }



                try
                {
                    bmp.EndInit();
                }
                catch (Exception)
                {
                    bmp = new BitmapImage();
                    this.StopAnimation();
                    //this.Image.Source = null;
                     //throw;
                }
                this.Image.Source = bmp;
                

            }

        }

        void bmp_DownloadCompleted(object sender, EventArgs e)
        {
//            Converting BitmapImage to Bitmap. (Seldom use)
//code
//BitmapImage bi = GetBitmapImage(); // Get bitmapimage from somewhere
//MemoryStream ms = newMemoryStream();
//BitmapEncoder encoder = new BitmapEncoder();
//encoder.Frames.Add(BitmapFrame.Create(bi));
//encoder.Save(ms);
//Bitmap bmp = new Bitmap(ms);


            if (File.Exists(fichierlocal) != true)
            {
                BitmapImage bi = (BitmapImage)sender; // Get bitmapimage from somewhere
                using (FileStream stream = new FileStream(fichierlocal, FileMode.Create))
                {
                    //if (File.Exists(defaultCacheDir + "\\" + (string)value.Replace(@"\","_")))
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    encoder.Save(stream);
                    stream.Close();
                }
            }
            StopAnimation();
        }
 

        void value_DownloadProgress(object sender, DownloadProgressEventArgs e)

        {
            ProgressChargement.Text = e.Progress.ToString()+ " %";
            if (e.Progress == 100)

            {

                

                //this.Image.SourceUpdated
                //    this.Image.Source.
                StopAnimation();

            }

        }

 

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...

        public static readonly DependencyProperty SourceProperty =

            DependencyProperty.Register("Source", typeof(string), typeof(ImageLoading), new PropertyMetadata(new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)

        {


            ImageLoading source = sender as ImageLoading;


            source.Source = (string)e.NewValue;

        }

 

        private void BeginAnimation()

        {

            //this.Sablier.Start();
            this.ProgressChargement.Text = "0 %";
            this.ProgressChargement.Visibility = Visibility.Visible;

        }

 

        private void StopAnimation()

        {

            //this.Sablier.Stop();
            this.ProgressChargement.Visibility = Visibility.Collapsed;

        }

    }

}

