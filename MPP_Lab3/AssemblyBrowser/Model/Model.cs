using System;
using System.IO;
using System.Reflection;
using AssemblyAnalyzer.Data;
using AssemblyAnalyzer;
using Microsoft.Win32;

namespace AssemblyBrowser.Model;

public class Model
{
    public AssemblyData GetAssemblyInfo()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Title = "Select assembly to view";
        ofd.InitialDirectory = Directory.GetCurrentDirectory();
        ofd.Filter = ".Net assembly files (*.exe, *.dll) |*.exe;*.dll";
        Assembly asm;
        try
        {
            if (ofd.ShowDialog() == true)
                asm = Assembly.LoadFrom(ofd.FileName);
            else
                return null;
        }
        catch
        {
            return null;
        }
        Analyzer a = new Analyzer();
        a.SetAssembly(asm);
        return a.Analyze();
    }
}

public static class MyExtensions
{
    public static int WordCount(this string str)
    {
        return str.Split(new char[] { ' ', '.', '?' },
            StringSplitOptions.RemoveEmptyEntries).Length;
    }
    public static int Asd()
    {
        return 0;
    }
}