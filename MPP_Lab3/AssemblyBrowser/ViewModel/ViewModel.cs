#nullable disable

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AssemblyAnalyzer.Data;
using Microsoft.Win32;


namespace AssemblyBrowser.ViewModel;

/*public class Viewmodel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private Model.Model _model = new();

    private AssemblyData _assemblyData;

    public AssemblyData AssemblyData
    {
        get;
        set;
    }
    
    private string _openFile;
    public string OpenFile
    {
        get => _openFile;
        set
        {
            _openFile = value;
            _assemblyData = null;
            try
            {
                _assemblyData = _model.GetAssemblyInfo(_openFile);
            }
            catch (Exception e)
            {
                _openFile = e.Message;
                Console.WriteLine(_openFile);
            }
            OnPropertyChanged(nameof(_assemblyData));
        }
    }

    public ICommand OpenFileCommand => new RelayCommand(OpenAssembly);
    
    private void OpenAssembly()
    {
        var fileDialog = new OpenFileDialog
        {
            Filter = "Assemblies|*.dll;*.exe", Title = "Select assembly", Multiselect = false
        };

        var isOpen = fileDialog.ShowDialog() ;
                
        if (isOpen != null && isOpen.Value)
        {
            OpenFile = fileDialog.FileName;
            OnPropertyChanged(nameof(OpenFile));
        }
    }
    
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}*/

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