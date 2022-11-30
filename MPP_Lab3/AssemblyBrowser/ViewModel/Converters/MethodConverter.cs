using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AssemblyAnalyzer.Data;
namespace AssemblyBrowser.ViewModel.Converters;

public class MethodConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        MethodData m = value as MethodData;
        string accesors = IdentificatorFormatter.GetMethodIdentificator(m.Info);
        string typeNameGenArgs = TypeNameCreator.BuildTypename(m.Info.ReturnType.Name, m.Info.ReturnType.GetGenericArguments(),true);
        string nameGenArgs = TypeNameCreator.BuildTypename(m.Info.Name, m.Info.GetGenericArguments(),false);
        string parameters = '(' + string.Join(",", m.Info.GetParameters().Select(p => TypeNameCreator.BuildTypename(p.ParameterType.Name, p.ParameterType.GetGenericArguments(), true) + ' ' + p.Name)) + ')';
        string ext = m.IsExtension ? "(extension) " : null;
        return ext + accesors + " " + typeNameGenArgs + ' ' + nameGenArgs + parameters;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}