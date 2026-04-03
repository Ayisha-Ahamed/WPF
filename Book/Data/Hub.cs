// ---------------------------------------------------------------------------------------
// Nexus ~ Separation of Concerns Demo
// Copyright (c) Trumpf Metamation India.
// ---------------------------------------------------------------------------------------
// Hub.cs
// Provides a generic manager for entities of type T, supporting CRUD operations.
// ---------------------------------------------------------------------------------------
using Nexus.Core;

namespace Nexus.Data;

#region class Hub<T> -------------------------------------------------------------------------------
/// <summary>A generic hub for managing entities of type T. Supports CRUD operations.</summary>
public class Hub<T> where T : class, IEntity, new () {

   #region Constructor -----------------------------------------------
   /// <summary>Initializes a new instance of the Hub class using the given database instance.</summary>
   public Hub (IDB<T> db) {
      mDB = db;
      mAll = mDB.GetAll ();
   }
   #endregion

   #region Properties ------------------------------------------------
   /// <summary>List of all items of type T.</summary>
   public List<T> All => mAll;
   #endregion

   #region Implementation --------------------------------------------
   /// <summary>Creates a new instance of type T. ID is auto-generated.</summary>
   public T Create () {
      var ent = new T { ID = mAll.Count == 0 ? 1 : mAll.Max (a => a.ID) + 1 };
      return ent;
   }

   /// <summary>Removes the given entity from the database.</summary>
   public void Delete (T ent) {
      mAll.Remove (ent);
      SaveAll ();
   }

   /// <summary>Gets the entity for the given ID.</summary>
   public T Get (int id) => mAll.FirstOrDefault (e => e.ID == id);

   /// <summary>Saves the entity to the database..</summary>
   public void Save (T ent) {
      var e = mAll.FirstOrDefault (e => e.ID == ent.ID);
      if (e == null) mAll.Add (ent);
      else {
         int idx = mAll.IndexOf (e);
         mAll[idx] = ent;
      }
      SaveAll ();
   }

   /// <summary>Saves all items to the database.</summary>
   void SaveAll () => mDB.SaveAll (mAll);
   #endregion

   #region Private Data ----------------------------------------------
   IDB<T> mDB;
   readonly List<T> mAll;
   #endregion
}
#endregion