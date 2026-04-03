// ---------------------------------------------------------------------------------------
// Nexus ~ Separation of Concerns Demo
// Copyright (c) Trumpf Metamation India.
// ---------------------------------------------------------------------------------------
// User.cs
// The User class
// ---------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Nexus.Core;

#region class User ---------------------------------------------------------------------------------
/// <summary>The User class</summary>
public class User : IEntity {
   /// <summary>The ID of the user</summary>
   public int ID { get; set; }
   /// <summary>The first name of the user</summary>
   [Required]
   public string FirstName { get; set; }
   /// <summary>The last name of the user</summary>
   [Required]
   public string LastName { get; set; }
   /// <summary>The age of the user</summary>
   [Required]
   [Range (1, 120)]
   public int Age { get; set; }
   /// <summary>The email address of the user</summary>
   [Required]
   [EmailAddress]
   public string Email { get; set; }
   /// <summary>The phone number of the user</summary>
   [Required]
   [RegularExpression (@"^\d{10}$", ErrorMessage = "Phone number must contain 10 digits")]
   public string Phone { get; set; }
}
#endregion