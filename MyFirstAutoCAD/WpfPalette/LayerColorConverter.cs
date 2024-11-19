using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace WpfPalette
{
    /// <summary>
    /// Provides the conversion method to get a layer color.
    /// </summary>
    [ValueConversion(typeof(ICustomTypeDescriptor), typeof(SolidColorBrush))]
    class LayerColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts an ICustomTypeDescriptor object figuring a layer into its color.
        /// </summary>
        /// <param name="value">AutoCAD color to convert.</param>
        /// <param name="targetType">SolidColorBrush type.</param>
        /// <param name="parameter">Unused.</param>
        /// <param name="culture">Unused.</param>
        /// <returns>Instance of SolidColorBrush figuring the layer color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Autodesk.AutoCAD.Colors.Color)
            {
                var acadColor = (Autodesk.AutoCAD.Colors.Color)value;
                var drawingColor = acadColor.ColorValue;
                var mediaColor = Color.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);
                return new SolidColorBrush(mediaColor);
            }
            return null;
        }

        /// <summary>
        /// Inverse converion method (unused).
        /// </summary>
        /// <returns>Always null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
