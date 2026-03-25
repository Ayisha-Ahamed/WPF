using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CustomerManager.Models {
   #region Customer -------------------------------------------------------------------------------
   public class Customer : IDataErrorInfo, INotifyPropertyChanged {
      #region Properties --------------------------------------------
      public int Id {
         get => _id;
         set {
            _id = value;
            OnPropertyChanged ();
         }
      }

      public string? FirstName {
         get => _firstName;
         set {
            _firstName = value;
            OnPropertyChanged ();
         }
      }

      public string? LastName {
         get => _lastName;
         set {
            _lastName = value;
            OnPropertyChanged ();
         }
      }

      public string? Email {
         get => _email;
         set {
            _email = value;
            OnPropertyChanged ();
         }
      }

      public string? PhoneNo {
         get => _phoneNo;
         set {
            _phoneNo = value;
            OnPropertyChanged ();
         }
      }

      public event PropertyChangedEventHandler? PropertyChanged;

      public string this[string columnName] {
         get {
            return columnName switch {
               nameof (Id) => !Regex.IsMatch (Id.ToString (), "[0-9]+") ? "Invalid id" : "",
               nameof (FirstName) =>
                           !Regex.IsMatch (FirstName ?? "", "[a-z]+") ? "Please enter alphabetic characters" : "",
               nameof (LastName) =>
                            !Regex.IsMatch (LastName ?? "", "[a-z]+") ? "Please enter alphabetic characters" : "",
               nameof (Email) =>
                            !Regex.IsMatch (Email ?? "", "([a-z]+[0-9]*[a-z]*)@([a-z]+)(.com)") ? "Incorrect email id" : "",
               nameof (PhoneNo) =>
                            !Regex.IsMatch (PhoneNo ?? "", "[0-9]{10}") ? "Please enter 10 digits" : "",
               _ => "",
            };
         }
      }
      #endregion

      #region Methods -----------------------------------------------
      /// <summary>Method to convert class object to csv format</summary>
      public override string ToString () => $"{Id},{FirstName},{LastName},{Email},{PhoneNo}";

      /// <summary>IDataErrorinfo interface implementation</summary>
      public string Error => throw new NotImplementedException ();
      #endregion

      #region Implementation ----------------------------------------
      // Invokes property changed event
      void OnPropertyChanged ([CallerMemberName] string? caller = null) {
         PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (caller));
      }
      #endregion

      #region Private Data ------------------------------------------
      // Backing field for properties
      int _id;
      string? _firstName;
      string? _lastName;
      string? _email;
      string? _phoneNo;
      #endregion
   }
   #endregion
}