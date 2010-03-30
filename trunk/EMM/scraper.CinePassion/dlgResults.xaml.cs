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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using EmberAPI;

namespace CinePassion
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class dlgResults : Window
    {
        private FilmCollection _ListeResult;
        public FilmCollection ListeResult
        {
            get
            {
                if (_ListeResult == null)
                {
                    return new FilmCollection();
                }
                return _ListeResult;
            }
        }

        private EmberAPI.MediaContainers.Movie _SearchMovie;
        public Film _ResultMovie;
        //private readonly FilmCollection _ListeResult = new FilmCollection();

        public dlgResults(EmberAPI.MediaContainers.Movie _ParamMovie)
        {
            


            // = String.Concat(Master.eLang.GetString(301, "Search Results - "), _SearchMovie.Title);
             //_ListeResult = TryFindResource("ResultDataSource") as FilmCollection;
             _ListeResult = new FilmCollection();
             API _API = new API();
             _SearchMovie = _ParamMovie;
             _ListeResult = _API.Search(_SearchMovie);
             InitializeComponent();
            //lstResults.ItemsSource = _ListeResult;
            string test = "";
            //foreach (var item in _ListeResult)
            //{
            //    lstResults.Items.Add(item);
            //}
        }

        private void Result_Closed(object sender, EventArgs e)
        {

        }

        private void bnt_Annuler_Click(object sender, RoutedEventArgs e)
        {
            _ResultMovie = null;
            base.Close();
        }

        private void btn_Valider_Click(object sender, RoutedEventArgs e)
        {
            _ResultMovie = (Film)lstResults.SelectedItem;
            base.Close();
        }
    }
}
