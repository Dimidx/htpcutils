using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaManager.Library.NFO;

namespace MediaManager
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
		public NfoMovie movie = new NfoMovie();
        public Window1()
        {
            InitializeComponent();
			this.DataContext = movie;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            
            movie = NfoFile.getNfoMovie(@"X:\Films\300\300.nfo");
            //this.DataContext = movie;
            //movie.Credits = "";
            




        }
    }
}
