using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexus.Core {
   public class Book : IEntity {
      /// <summary>ID of the book</summary>
      public int ID { get; set; }

      /// <summary>Title of the book</summary>
      [Required]
      public string Title { get; set; }

      /// <summary>Author of the book</summary>
      [Required]
      public string Author { get; set; }

      /// <summary>Year of publishing</summary>
      [Required]
      [Range (0, 2025)]
      public int Year { get; set; }
   }
}
