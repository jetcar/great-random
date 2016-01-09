using System;
using System.Windows.Input;

namespace GreatRandom
{
    public class Command<T> : ICommand
    {

        private Action<T> method;
        public Command(Action<T> method)
        {
            this.method = method;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.method.Invoke((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
