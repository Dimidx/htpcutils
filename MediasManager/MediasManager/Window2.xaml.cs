using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TechNuts;

namespace MediaManager
{
	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>
	public partial class Window2 : Window
	{
		public Window2()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TechNuts.ScraperXML.ScraperManager s = new TechNuts.ScraperXML.ScraperManager(@"D:\Perso\Dev\HTPCUtils\MediasManager\MediasManager\bin\Debug\Plugins\ScraperXML\", @"D:\Perso\Dev\HTPCUtils\MediasManager\MediasManager\bin\Debug\Cache\Scraper\");
            foreach (TechNuts.ScraperXML.ScraperInfo item in s.Movies)

            {
                Console.WriteLine(item.ScraperName);

            }
            Console.WriteLine(s.Log);
        }
	}
}