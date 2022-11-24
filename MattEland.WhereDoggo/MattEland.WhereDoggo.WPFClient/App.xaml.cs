using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MattEland.WhereDoggo.WPFClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        string message = $"{e.Exception.Message} ({e.Exception.GetType().Name}){Environment.NewLine}{e.Exception.StackTrace}";

        MessageBox.Show(message, "An unhandled exception occurred", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
}
