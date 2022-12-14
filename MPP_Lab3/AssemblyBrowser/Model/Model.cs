using System;
using System.IO;
using System.Reflection;
using AssemblyAnalyzer.Data;
using AssemblyAnalyzer;
using Microsoft.Win32;
using System.Collections.Generic;

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
        var result = a.Analyze();
        return result;
    }
}