using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCP_Chat.Commands
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
        private readonly Func<Task> _command;
        private readonly Predicate<object> _canExecute;
        public AsyncCommand(Func<Task> command,Predicate<object> canExecute)
        {
            if(command == null)
            {
                throw new ArgumentNullException("execute");
            }
            _command = command;
            _canExecute = canExecute;
        }
        public override bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public override Task ExecuteAsync(object parameter)
        {
            return _command();
        }
    }
   
}
