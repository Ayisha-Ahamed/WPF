using CustomerManager.Commands;
using CustomerManager.Models;
using System.ComponentModel;
using System.Windows;

namespace CustomerManager.ViewModel {
   #region class CustomerDlgViewModel -------------------------------------------------------------
   class EditDBDlgViewModel : IDataErrorInfo {
      #region Constructor -------------------------------------------
      public EditDBDlgViewModel (Window win, int? index = null) {
         mDlg = win;
         ExecuteOpCommand = new RelayCommand (ExecuteBtnClicked);
         if (index != null) {
            mEditMode = true;
            Customer = CustomerDB.Customers[index ?? throw new Exception ()];
         } else Customer = new Customer () { Id = CustomerDB.NextId };
      }
      #endregion

      #region Methods -----------------------------------------------
      // Indexer returns error message for data validation
      public string this[string prop] => Customer[prop];

      // Customer class object to perform add / edit operation
      public Customer Customer { get; set; }

      // Implementation of IDataErrorInfo
      public string Error => throw new NotImplementedException ();

      // Command executes add / edit operation
      public RelayCommand ExecuteOpCommand { get; set; }
      #endregion

      #region Implementation ----------------------------------------
      // Return if the text field has errors
      bool CanExecute () {
         foreach (var prop in typeof (Customer).GetProperties ()) {
            if (!string.IsNullOrEmpty (Customer[prop.Name.ToString ()])) return false;
         }
         return true;
      }

      // Execute Add / Edit button click operation
      void ExecuteBtnClicked (object? obj) {
         if (CanExecute ()) {
            if (!mEditMode)
               CustomerDB.Customers.Add (Customer); // Add customer to database
            mDlg.Close ();
         }
      }
      #endregion

      #region Private Data ------------------------------------------
      readonly Window mDlg;
      bool mEditMode = false;
      #endregion
      #endregion
   }
}