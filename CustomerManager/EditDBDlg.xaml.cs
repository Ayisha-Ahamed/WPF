using CustomerManager.ViewModel;
using System.Windows;

namespace CustomerManager;
#region class CustomerDlg -------------------------------------------
/// <summary>Interaction logic for EditDbDlg.xaml</summary>
public partial class EditDBDlg : Window {
   #region Constructor ----------------------------------------------
   public EditDBDlg (int? index = null) {
      InitializeComponent ();
      if (index != null) {
         mEditMode = true;
         EditDBDlgViewModel editCustomerViewModel = new (this, index);
         BtnExecute.Content = "Edit";
         DataContext = editCustomerViewModel;
      } else {
         EditDBDlgViewModel addCustomerViewModel = new (this);
         DataContext = addCustomerViewModel;
      }
   }
   #endregion

   #region Private Data ---------------------------------------------
   bool mEditMode;
   void OnClickedCancel (object sender, RoutedEventArgs e) => Close ();
   #endregion
}
#endregion