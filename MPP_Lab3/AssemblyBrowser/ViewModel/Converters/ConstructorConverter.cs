using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace AssemblyBrowser.ViewModel.Converters;

public class ConstructorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ConstructorInfo c = value as ConstructorInfo;
        string accesors = IdentificatorFormatter.GetCtorIdentificator(c);
        string nameGenArgs = TypeNameCreator.BuildTypename(c.DeclaringType.Name, c.DeclaringType.GetGenericArguments(),true);

        string parameters = '(' + string.Join(",", c.GetParameters().Select(p => TypeNameCreator.BuildTypename(p.ParameterType.Name, p.ParameterType.GetGenericArguments(),true) + ' ' + p.Name)) + ')';
        return accesors + " " +  nameGenArgs + parameters;           
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}