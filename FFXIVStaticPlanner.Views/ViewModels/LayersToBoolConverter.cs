using System;
using System.Globalization;
using System.Windows.Data;

namespace FFXIVStaticPlanner.ViewModels
{
    public class LayersToBoolConverter : IValueConverter
    {
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