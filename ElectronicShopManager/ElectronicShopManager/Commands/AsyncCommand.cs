using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectronicShopManager.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _execute;
        private readonly Predicate<object> _canExecute;
        public AsyncCommand(Func<Task> execute,Predicate<object> canExecute)
        {
            if(execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public override Task ExecuteAsync(object parameter)
        {
            return _execute();
        }
    }
   
}
