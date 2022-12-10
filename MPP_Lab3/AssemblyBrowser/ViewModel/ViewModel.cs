using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AssemblyAnalyzer.Data;

namespace AssemblyBrowser.ViewModel;

class Viewmodel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private Model.Model _model = new Model.Model();

    protected void OnPropertyChanged([CallerMemberName]string propertyName = "") => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private AssemblyData _assemblyData;
    public AssemblyData SelectedAssemblyInfo {
        get => _assemblyData;
        set
        {
            _assemblyData = value;
            OnPropertyChanged(nameof(SelectedAssemblyInfo));
        } 
    }

    public ICommand SelectAssembly { get; }

    public Viewmodel()
    {
        SelectAssembly = new RelayCommand<object>((parameter) => { SelectedAssemblyInfo = _model.GetAssemblyInfo() ?? SelectedAssemblyInfo; }, null);
    }
}