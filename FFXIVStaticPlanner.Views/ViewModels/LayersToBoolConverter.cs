using System;
using System.Globalization;
using System.Windows.Data;

namespace FFXIVStaticPlanner.ViewModels
{
    /// <summary>
    /// Allows for conversion of <see cref="Layers"/> to <see cref="bool"/>
    /// </summary>
    public class LayersToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Layers"/> value to a <see langword="true"/>or <see langword="false"/> value
        /// </summary>
        /// <param name="value">The Layers value to convert</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">The comparison item to see if the given value should return <see langword="true"/></param>
        /// <param name="culture">not used</param>
        /// <returns><see langword="true"/> if the value and the parameter are equal, otherwise <see langword="false"/></returns>
        public object Convert ( object value , Type targetType , object parameter , CultureInfo culture )
        {
            if ( value is Layers layers )
            {
                Layers paramValue = Enum.Parse<Layers> ( parameter.ToString ( ) );

                return layers == paramValue;
            }

            return false;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) => value is bool bValue ? Enum.Parse<Layers> ( parameter.ToString ( ) ) : ( object )Layers.None;
    }
}