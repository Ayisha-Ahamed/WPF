using Nexus.App.VMs;
using Nexus.Core;
using Nexus.Data;
using Nexus.UI;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nexus.App;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class MainWindow : Window {
   #region Constructor -----------------------------------------------
   public MainWindow () {
      InitializeComponent ();
      IDB<Book> s = DB.Get<Book> ();
      mHub = new (s);
      foreach (var u in mHub.All) Books.Add (new (u, mHub));   // Create UserVM collection
      Init<Book> ();
   }
   #endregion

   #region Properties ------------------------------------------------
   // The currently selected user in the ListView
   BookVM SelectedBook => Lst.SelectedItem as BookVM;

   // The UserVM collection bound to listview
   ObservableCollection<BookVM> Books { get; set; } = [];
   #endregion

   #region Implementation --------------------------------------------
   void Init<T>() where T : class {
      // Set up the ListView columns
      var cols = typeof(T).GetProperties();
      GridView g = new ();
      for (int i = 0; i < cols.Length; i++) {
         GridViewColumn gc = new () { Header = cols[i].Name, DisplayMemberBinding = new Binding (cols[i].Name.Replace (" ", "")), Width = 100 };
         g.Columns.Add (gc);
      }
      Lst.View = g;
      Lst.ItemsSource = Books;

      // Commands
      CommandBindings.Add (new CommandBinding (Commands.Add, (_, _) => DoAddEditBook ()));
      CommandBindings.Add (new CommandBinding (Commands.Edit, (_, _) => DoAddEditBook (SelectedBook), CanExcecute));
      CommandBindings.Add (new CommandBinding (Commands.Delete, (_, _) => DoRemoveBook (), CanExcecute));
   }


   void DoAddEditBook (BookVM vm = null) {
      bool iNew = vm == null;
      var u = iNew ? mHub.Create () : vm.Clone ();
      BookVM wvm = new (u, mHub);
      AddBookDlg dlg = new (wvm, mHub) { Owner = this };
      if (dlg.ShowDialog () == true) {
         if (iNew) {
            wvm.Save ();
            Books.Add (wvm);
         } else {
            vm.UpdateFrom (u);
            vm.Save ();
            Lst.Items.Refresh ();
         }
      }
   }

   void DoRemoveBook () {
      if (SelectedBook is null) return;
      if (MessageBox.Show ($"Are you sure you want to delete {SelectedBook.Title} ?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
         SelectedBook.Delete ();
         Books.Remove (SelectedBook);
      }
   }

   void CanExcecute (object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = SelectedBook != null;
   #endregion

   #region Private Data ----------------------------------------------
   readonly Hub<Book> mHub;
   #endregion
}