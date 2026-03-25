using System.Windows.Input;

namespace CustomerManager.Commands {
   internal class RelayCommand : ICommand {
      public event EventHandler? CanExecuteChanged;

      public bool CanExecute (object? parameter = null) => _CanExecute?.Invoke (parameter) ?? true;

      public void Execute (object? parameter = null) => _Execute (parameter);

      public RelayCommand (Action<object?> action, Predicate<object?>? predicate = null) {
         _Execute = action;
         _CanExecute = predicate;
      }

      Action<object?> _Execute { get; set; }
      Predicate<object?>? _CanExecute { get; set; }
   }
}