using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MovieCatalogue.UI.Converters
{
    public class PosterPathToImageSourceConverter : IValueConverter
    {
        private const string BaseUrl = "https://image.tmdb.org/t/p/w200";

        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string posterPath || string.IsNullOrWhiteSpace(posterPath))
                return null;

            try
            {
                return new BitmapImage(new Uri($"{BaseUrl}{posterPath}"));
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}