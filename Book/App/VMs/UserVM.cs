// ---------------------------------------------------------------------------------------
// Nexus ~ Separation of Concerns Demo
// Copyright (c) Trumpf Metamation India.
// ---------------------------------------------------------------------------------------
// UserVM.cs
// View Model for the User
// ---------------------------------------------------------------------------------------
using Nexus.Core;
using Nexus.Data;

namespace Nexus.App;

#region Class UserVM -------------------------------------------------------------------------------
/// <summary>View Model class for the User</summary>
public class UserVM : EntityVM<User> {

   #region Constructor -----------------------------------------------
   public UserVM (User u, Hub<User> m) : base (u, m) { mUser = u; }
   #endregion

   #region Properties ------------------------------------------------
   /// <summary>First Name of the user</summary>
   public string FirstName {
      get => mUser.FirstName;
      set {
         if (mUser.FirstName != value) {
            mUser.FirstName = value;
            Notify ();
         }
      }
   }

   /// <summary>Last Name of the user</summary>
   public string LastName {
      get => mUser.LastName;
      set {
         if (mUser.LastName != value) {
            mUser.LastName = value;
            Notify ();
         }
      }
   }

   /// <summary>Age of the user</summary>
   public int Age {
      get => mUser.Age;
      set {
         if (mUser.Age != value) {
            mUser.Age = value;
            Notify ();
         }
      }
   }

   /// <summary>User's email address</summary>
   public string Email {
      get => mUser.Email;
      set {
         if (mUser.Email != value) {
            mUser.Email = value;
            Notify ();
         }
      }
   }

   /// <summary>Phone number of the user</summary>
   public string Phone {
      get => mUser.Phone;
      set {
         if (mUser.Phone != value) {
            mUser.Phone = value;
            Notify ();
         }
      }
   }
   #endregion

   #region Private Data ----------------------------------------------
   User mUser;
   #endregion

}
#endregion