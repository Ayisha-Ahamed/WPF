using Nexus.Core;
using Nexus.Data;

namespace Nexus.App.VMs {
   public class BookVM : EntityVM<Book> {
      public BookVM (Book ent, Hub<Book> h) : base (ent, h) { mBook = ent; }

      #region Properties ------------------------------------------------
      /// <summary>Title of the book</summary>
      public string Title {
         get => mBook.Title;
         set {
            if (mBook.Title != value) {
               mBook.Title = value;
               Notify ();
            }
         }
      }

      /// <summary>Author of the book</summary>
      public string Author {
         get => mBook.Author;
         set {
            if (mBook.Author != value) {
               mBook.Author = value;
               Notify ();
            }
         }
      }

      /// <summary>Year of publishing</summary>
      public int Year {
         get => mBook.Year;
         set {
            if (mBook.Year != value) {
               mBook.Year = value;
               Notify ();
            }
         }
      }
      #endregion

      #region Private Data ----------------------------------------------
      Book mBook;
      #endregion
   }
}