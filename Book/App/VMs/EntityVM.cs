// ---------------------------------------------------------------------------------------
// Nexus ~ Separation of Concerns Demo
// Copyright (c) Trumpf Metamation India.
// ---------------------------------------------------------------------------------------
// EntityVM.cs
// Abstract ViewModel for entities of type T.
// ---------------------------------------------------------------------------------------
using Nexus.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Nexus.Data;
using System.Reflection;

namespace Nexus.App;

#region class EntityVM<T> --------------------------------------------------------------------------
/// <summary>Abstract ViewModel for managing entities of type T. Supports CRUD operations,
/// validations and property change notifications.</summary>
public abstract class EntityVM<T> : INotifyPropertyChanged, IDataErrorInfo where T : class, IEntity, new() {

   #region Constructor -----------------------------------------------
   public EntityVM (T ent, Hub<T> h) {
      mEnt = ent;
      mHub = h;
   }
   #endregion

   #region Properties ------------------------------------------------
   /// <summary>The entity ID</summary>
   public int ID => mEnt.ID;
   #endregion

   #region Implementation --------------------------------------------
   /// <summary>Creates a deep copy of entity type T.</summary>
   public T Clone () {
      var c = new T ();
      Copy (mEnt, c);
      return c;
   }

   /// <summary>Copies all property values from one to another.</summary>
   /// <param name="s">Source</param>
   /// <param name="t">Target</param>
   void Copy (T s, T t) {
      foreach (var prop in sProps) {
         if (prop.CanWrite) prop.SetValue (t, prop.GetValue (s));
      }
   }

   /// <summary>Removes an entity using the Hub</summary>
   public void Delete () => mHub.Delete (mEnt);

   /// <summary>Saves the entity.</summary>
   public void Save () => mHub.Save (mEnt);

   /// <summary>Updates the entity's properties from another instance and notifies changes.</summary>
   /// <param name="src">The source entity to copy values from.</param>
   public void UpdateFrom (T src) {
      Copy (src, mEnt);
      foreach (var prop in sProps) Notify (prop.Name);
   }
   #endregion

   #region IDataErrorInfo Implementation -----------------------------
   /// <summary>Error message for the entity if any.</summary>
   public string Error => null;

   /// <summary>Error message from a specific property</summary>
   public string this[string name] {
      get {
         var prop = sProps.FirstOrDefault (prop => prop.Name == name);
         if (prop == null) return null;
         ValidationContext ctx = new (mEnt) { MemberName = name };
         List<ValidationResult> res = [];
         return Validator.TryValidateProperty (prop.GetValue(mEnt), ctx, res) ? null : res.FirstOrDefault ()?.ErrorMessage;
      }
   }
   #endregion

   #region INotifyPropertyChanged Implementation ---------------------
   /// <summary>Raises the PropertyChanged event for a property.</summary>
   protected void Notify ([CallerMemberName] string name = null)
         => PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (name));

   /// <summary>The event triggered when a property changes.</summary>
   public event PropertyChangedEventHandler PropertyChanged;
   #endregion

   #region Private Data ----------------------------------------------
   readonly T mEnt;
   readonly Hub<T> mHub;
   static readonly PropertyInfo[] sProps = typeof (T).GetProperties ();
   #endregion
}
#endregion