using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Wordle.Models {
   internal class RelayCommand : ICommand {
      public event EventHandler? CanExecuteChanged;
      public bool CanExecute (object? parameter) => _CanExecute (parameter);
      public void Execute (object? parameter) => _Execute (parameter);

      public RelayCommand (Action<object?> execute, Predicate<object?> canExecute) {
         _Execute = execute;
         _CanExecute = canExecute;
      }

      Action<object?> _Execute;
      Predicate<object?> _CanExecute;
   }
}