using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper
{
    public class ImageMovie
    {
        /// <summary>
        /// URL of the image
        /// </summary>
        private string m_URLImage;
        public string URLImage
        {
            get { return m_URLImage; }
            set { m_URLImage = value; }
        }

        /// <summary>
        /// URL of the thumbnail of the image
        /// </summary>
        private string m_URLThumb;
        public string URLThumb
        {
            get { return m_URLThumb; }
            set { m_URLThumb = value; }
        }

        /// <summary>
        /// Height of the image
        /// </summary>
        private string m_Height;
        public string Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// Whith of the image
        /// </summary>
        private string m_Width;
        public string Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

    }
}
