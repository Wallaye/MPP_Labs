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

        string typeNameGenArgs = TypenameBuilder.BuildTypename(m.Info.ReturnType.Name, m.Info.ReturnType.GetGenericArguments(),true);
        string nameGenArgs = TypenameBuilder.BuildTypename(m.Info.Name, m.Info.GetGenericArguments(),false);
        string parameters = '(' + string.Join(",", m.Info.GetParameters().Select(p => TypenameBuilder.BuildTypename(p.ParameterType.Name, p.ParameterType.GetGenericArguments(), true) + ' ' + p.Name)) + ')';
        string ext = m.IsExtension ? "(extension) " : null;
        return ext + typeNameGenArgs + ' ' + nameGenArgs + parameters;
            
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}