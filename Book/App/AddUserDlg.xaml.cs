using System.Windows;
using Nexus.Core;
using Nexus.Data;

namespace Nexus.App;

// <summary>Interaction logic for AddUserDlg.xaml</summary>
public partial class AddUserDlg : Window {
   public AddUserDlg (UserVM vm, Hub<User> h) {
      InitializeComponent ();
      Title = $"{(h.Get (vm.ID) != null ? "Edit" : "Add")} User";
      BtnOK.Click += (_, _) => DialogResult = true;
      BtnCancel.Click += (_, _) => { DialogResult = false; Close (); };
      DataContext = vm;
   }
}