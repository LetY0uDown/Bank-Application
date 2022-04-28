using System.ComponentModel;

namespace Bank.Core.Objects.Abstract;

public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
}