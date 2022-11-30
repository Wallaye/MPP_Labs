using System.ComponentModel;
using System.Windows.Input;
using AssemblyAnalyzer.Data;

namespace AssemblyBrowser.ViewModel;

public class Viewmodel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private AssemblyBrowser.Model.Model model = new();

    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public AssemblyData SelectedAssemblyInfo { get; set; }

    public ICommand SelectAssembly { get; }

    public Viewmodel()
    {
        this.SelectAssembly = new RelayCommand<object>((parameter) =>{ SelectedAssemblyInfo = model.GetAssemblyInfo() ?? SelectedAssemblyInfo; }, ()=>true);
    }
}