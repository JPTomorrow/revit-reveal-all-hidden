namespace JPMorrow.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.CompilerServices;
    using System.ComponentModel;
    using System.Windows.Input;

    public partial class MainViewModel : Presenter
    {
        public ObservableCollection<ItemPresenter> Items { get; }
            = new ObservableCollection<ItemPresenter>
        {
            new ItemPresenter("A"),
            new ItemPresenter("B"),
            new ItemPresenter("C"),
            new ItemPresenter("D")
        };

        public ICommand DoStuffCommand => new Command(param => { TemplateCommand(/* ADD PARAMETERS TO BUTTOM CLICK HERE */); });
    }
    
     public class ItemPresenter : Presenter
    {
        private readonly string _value;

        /// <summary>
        /// Default Presenter: Just Presents a string value as a listbox item, 
        /// can replace with an object for more complex listbox databindings
        /// </summary>
        /// <param name="value"></param>
        public ItemPresenter(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }

    #region aux
    public abstract class Presenter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Command : ICommand
    {
        private readonly Action<object> _action;

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public Command(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }
    }
    #endregion
    
}