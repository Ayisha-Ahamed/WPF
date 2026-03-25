using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CustomerManager.Models {
   #region Customer -------------------------------------------------------------------------------
   public class Customer : IDataErrorInfo, INotifyPropertyChanged {
      #region Properties --------------------------------------------
      public int Id {
         get => mId;
         set {
            mId = value;
            OnPropertyChanged ();
         }
      }

      public string? FirstName {
         get => mFirstName;
         set {
            mFirstName = value;
            OnPropertyChanged ();
         }
      }

      public string? LastName {
         get => mLastName;
         set {
            mLastName = value;
            OnPropertyChanged ();
         }
      }

      public string? Email {
         get => mEmail;
         set {
            mEmail = value;
            OnPropertyChanged ();
         }
      }

      public string? PhoneNo {
         get => mPhoneNo;
         set {
            mPhoneNo = value;
            OnPropertyChanged ();
         }
      }

      public event PropertyChangedEventHandler? PropertyChanged; // INotifyPropertyChanged

      public string this[string columnName] { // IDataErrorInfo
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

      public string Error => throw new NotImplementedException ();  // IDataErrorInfo
      #endregion

      #region Implementation ----------------------------------------
      // Invokes property changed event
      void OnPropertyChanged ([CallerMemberName] string? caller = null) {
         PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (caller));
      }
      #endregion

      #region Private Data ------------------------------------------
      // Backing field for properties
      int mId;
      string? mFirstName;
      string? mLastName;
      string? mEmail;
      string? mPhoneNo;
      #endregion
   }
   #endregion
}