using System;
using System.Windows.Data;
using System.Windows;

namespace Municipal_App.Converters
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Converts the height of an element into a negative bottom margin that is half the height.
    /// This converter is useful for creating a margin proportional to the element's height.
    /// Implements the IValueConverter interface for use in XAML data binding.
    /// Disclosure of AI use: ChatGPT helped write this class
    /// </summary>
    internal class HeightToMarginConverter : IValueConverter
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Converts the height of an element to a bottom margin that is half the height, 
        /// returned as a Thickness with the bottom margin being negative.
        /// </summary>
        /// <param name="value">The actual height value (double).</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">Optional converter parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>A Thickness with a negative bottom margin equal to half the height.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double height)
            {
                // Return a Thickness where the bottom margin is half the height
                return new Thickness(0, 0, 0, (height / 2) * -1);
            }
            return new Thickness(0);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// ConvertBack is not implemented because converting a margin back to height is not required.
        /// </summary>
        /// <param name="value">The value to convert back (not used).</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>Throws a NotImplementedException, as this functionality is not needed.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
//---------------------------------------EOF-------------------------------------------