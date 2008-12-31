using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper
{
    public class Movie
    {
        /// <summary>
        /// Title of movie
        /// </summary>
        private string m_Title;
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        /// <summary>
        /// ID of movie
        /// </summary>
        private string m_ID;
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// URL of page detail of movie
        /// </summary>
        private string m_URLPage;
        public string URLPage
        {
            get { return m_URLPage; }
            set { m_URLPage = value; }
        }

        /// <summary>
        /// Year of movie
        /// </summary>
        private string m_Year;
        public string Year
        {
            get { return m_Year; }
            set { m_Year = value; }
        }

        /// <summary>
        /// List of genre
        /// </summary>
        private string[] m_Genre;
        public string[] Genre
        {
            get { return m_Genre; }
            set { m_Genre = value; }
        }

        /// <summary>
        /// List of Cover for the movie
        /// </summary>
        private ImageMovie[] m_Cover;
        public ImageMovie[] Cover
        {
            get { return m_Cover; }
            set { m_Cover = value; }
        }

        /// <summary>
        /// List of Fanart for the movie
        /// </summary>
        private ImageMovie[] m_Fanart;
        public ImageMovie[] Fanart
        {
            get { return m_Fanart; }
            set { m_Fanart = value; }
        }

    }
}
