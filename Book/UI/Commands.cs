using System.Windows.Input;

namespace Nexus.UI;

#region class Commands -----------------------------------------------------------------------------
public static class Commands {
   public static readonly RoutedUICommand Add = new ("Add", "Add", typeof (Commands), [new KeyGesture (Key.N, ModifierKeys.Control)]);

   public static readonly RoutedUICommand Edit = new ("Edit", "Edit", typeof (Commands), [new KeyGesture (Key.E, ModifierKeys.Control)]);

   public static readonly RoutedUICommand Delete = new ("Delete", "Delete", typeof (Commands), [new KeyGesture (Key.D, ModifierKeys.Control)]);
}
#endregion
