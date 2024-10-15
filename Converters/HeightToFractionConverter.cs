using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Municipal_App.Converters
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Converts a height value to a fraction of the height, based on a multiplier provided as a parameter.
    /// This converter is typically used to proportionally resize UI elements by applying a fraction of their actual height.
    /// Implements the IValueConverter interface for use in XAML data binding.
    /// Disclosure of AI use: ChatGPT helped write this class
    /// </summary>
    internal class HeightToFractionConverter : IValueConverter
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Converts the actual height of an element to a fraction of that height.
        /// The fraction is provided as a string in the parameter and parsed to a double.
        /// </summary>
        /// <param name="value">The actual height value (double).</param>
        /// <param name="targetType">The target type of the conversion (not used).</param>
        /// <param name="parameter">A string representing the fraction to apply.</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>The height multiplied by the given fraction, or the original value if parsing fails.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double actualHeight && parameter is string fractionStr && double.TryParse(fractionStr, out double fraction))
            {
                return actualHeight * fraction;
            }
            return value;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// ConvertBack is not implemented because converting back from a fraction to the original height is not required.
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