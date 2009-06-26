using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace XBMC_Touch.Utils.Converters
{

    #region SecondesToTime
    public class SecondesToTime : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                TimeSpan t = TimeSpan.FromSeconds((int)value);
                if (t.Hours > 0)
                {
                    return t.ToString();
                }
                else
                {
                    return t.ToString().Substring(3, 5);

                }
            }
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
    #endregion
}

