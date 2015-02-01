using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using KinectPowerPointControl.Model;

namespace KinectPowerPointControl.Converters
{
    class GestureToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KinectPowerPointControl.Model.KinectGestureProcessor.GestureState state = (KinectPowerPointControl.Model.KinectGestureProcessor.GestureState)value;
            return state.ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
