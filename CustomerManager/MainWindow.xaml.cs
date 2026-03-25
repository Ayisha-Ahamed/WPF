using CustomerManager.Models;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows;

namespace CustomerManager;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class MainWindow : Window {
   public ObservableCollection<Customer> Customers { get; set; }

   public MainWindow () {
      InitializeComponent ();
      this.DataContext = this;
      Customers = CustomerDB.Customers;
   }

   #region Event Handlers -------------------------------------------
   void OnClickedDelete (object sender, RoutedEventArgs e) {
      int index = CustomerList.SelectedIndex;
      if (index != -1) CustomerDB.Customers.RemoveAt (index);
   }

   void OnClickedAdd (object sender, RoutedEventArgs e) {
      EditDBDlg addCustomerDlg = new ();
      addCustomerDlg.ShowDialog ();
   }

   void OnClickedEdit (object sender, RoutedEventArgs e) {
      int index = CustomerList.SelectedIndex;
      if (index != -1) {
         EditDBDlg editCustomerDlg = new (index);
         editCustomerDlg.ShowDialog ();
      }
   }

   void OnClosed (object sender, EventArgs e) => CustomerDB.SaveFile ();
   #endregion
}