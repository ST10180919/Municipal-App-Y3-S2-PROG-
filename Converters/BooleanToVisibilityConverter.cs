using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Municipal_App.Converters
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Converts a boolean value to a Visibility enumeration value.
    /// It returns Visibility.Visible if the value is true, and Visibility.Collapsed if the value is false.
    /// Implements the IValueConverter interface to be used in XAML data binding.
    /// Disclosure of AI use: ChatGPT helped write this class
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Converts a boolean value to Visibility. 
        /// If the input value is true, it returns Visibility.Visible; otherwise, it returns Visibility.Collapsed.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">Optional converter parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>Visibility.Visible if true, otherwise Visibility.Collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Converts a Visibility value back to a boolean.
        /// If the Visibility is Visible, it returns true; otherwise, it returns false.
        /// </summary>
        /// <param name="value">The Visibility value to convert back.</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">Optional converter parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>True if Visibility.Visible, otherwise false.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue)
            {
                return visibilityValue == Visibility.Visible;
            }
            return false;
        }
    }
}
//---------------------------------------EOF-------------------------------------------