// ---------------------------------------------------------------------------------------
// Nexus ~ Separation of Concerns Demo
// Copyright (c) Trumpf Metamation India.
// ---------------------------------------------------------------------------------------
// DB.cs
// Data access layer
// ---------------------------------------------------------------------------------------
using Nexus.Core;

namespace Nexus.Data;

#region class DB -----------------------------------------------------------------------------------
public static class DB {
   /// <summary>The type of data storage (File/Database)</summary>
   public static EDB Type => EDB.File;

   /// <summary>Creates a new instance of database implementation for entity type T.</summary>
   /// <typeparam name="T">The type of entity to be managed by the database.</typeparam>
   public static IDB<T> Get<T> () where T : class, IEntity, new() {
      return Type switch {
         EDB.File => new FileDB<T> (),
         _ => throw new NotSupportedException ($"DB type {Type} is not supported.")
      };
   }
}
#endregion

#region enum EDB -----------------------------------------------------------------------------------
/// <summary>The supported database types.</summary>
public enum EDB { File, Sql, Mongo }
#endregion