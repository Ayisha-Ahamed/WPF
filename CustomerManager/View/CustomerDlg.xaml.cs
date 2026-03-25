using CustomerManager.Models;
using CustomerManager.ViewModel;
using System.Windows;

namespace CustomerManager.View {
   /// <summary>
   /// Interaction logic for AddCustomer.xaml
   /// </summary>
   public partial class AddCustomer : Window {
      bool mEditMode;
      public AddCustomer (int? index = null) {
         InitializeComponent ();
         if (index != null) {
            mEditMode = true;
            CustomerDlgViewModel editCustomerViewModel = new (this, index);
            BtnExecute.Content = "Edit";
            DataContext = editCustomerViewModel;
         } else {
            CustomerDlgViewModel addCustomerViewModel = new (this);
            DataContext = addCustomerViewModel;
         }
      }

      void OnClickedCancel (object sender, RoutedEventArgs e) => Close ();
   }
}