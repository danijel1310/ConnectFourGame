using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ConnectFour.Model;

namespace ConnectFour.View
{
    public class CellFillingToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EConnectFourCellContent type = (EConnectFourCellContent) value;
            switch (type)
            {
                case EConnectFourCellContent.Black:
                    return new SolidColorBrush(Colors.Red);
                case EConnectFourCellContent.White:
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}