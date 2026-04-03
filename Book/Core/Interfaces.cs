// ---------------------------------------------------------------------------------------
// Nexus ~ Separation of Concerns Demo
// Copyright (c) Trumpf Metamation India.
// ---------------------------------------------------------------------------------------
// Interfaces.cs
// Defines core interfaces for entities and data access.
// ---------------------------------------------------------------------------------------
namespace Nexus.Core;

#region Interface IEntity --------------------------------------------------------------------------
/// <summary>Represents a base entity with a unique identifier.</summary>
public interface IEntity {
   /// <summary>The unique ID of the entity</summary>
   int ID { get; set; }
}
#endregion

#region Interface IDB<T> ---------------------------------------------------------------------------
/// <summary>Defines generic data access for entities.</summary>
public interface IDB<T> where T : IEntity {
   /// <summary>Gets all the entities from the data source.</summary>
   List<T> GetAll ();
   /// <summary>Saves all items in the list to the underlying data store.</summary>
   void SaveAll (List<T> items);
}
#endregion