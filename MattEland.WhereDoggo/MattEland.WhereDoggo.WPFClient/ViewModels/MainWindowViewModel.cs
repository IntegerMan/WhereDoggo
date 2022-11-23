namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title => "WhereDoggo by Matt Eland (@IntegerMan)";
}
