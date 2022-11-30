using System;
using System.Globalization;
using System.Windows.Data;
using AssemblyAnalyzer.Data;

namespace AssemblyBrowser.ViewModel.Converters;

public class ClassConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ClassData c = value as ClassData;
        return TypenameBuilder.BuildTypename(c.ClassType.Name, c.ClassType.GetGenericArguments(),true);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}