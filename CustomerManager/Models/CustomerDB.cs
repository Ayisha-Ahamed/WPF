using System.Collections.ObjectModel;
using System.IO;

namespace CustomerManager.Models;
#region class CustomerDB --------------------------------------------------------------------------
public class CustomerDB {
   #region Properties -----------------------------------------------
   /// <summary>Stores customer data from csv file</summary>
   public static ObservableCollection<Customer> Customers;

   /// <summary>Returns the next usable id</summary>
   public static int NextId => Customers.Last ().Id + 1;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Save customer objects to csv file</summary>
   internal static void SaveFile () {
      List<string> lines = [];
      foreach (var entry in Customers) lines.Add (entry.ToString ());
      File.WriteAllLines ("C:\\Work\\WPF\\CustomerManager\\Data\\Customer.csv", lines);
   }
   #endregion

   #region Implementation -------------------------------------------
   // Initializes Customers collection
   static CustomerDB () {
      Customers = LoadCustomers ("Data\\Customer.csv");
   }

   // Loads content from csv file
   static ObservableCollection<Customer> LoadCustomers (string filePath) {
      ObservableCollection<Customer> customers = [];
      var lines = File.ReadAllLines (filePath).ToList ();
      // Remove the header row
      lines.RemoveAt (0);
      foreach (var line in lines) {
         var data = line.Split (',');
         if (data.Length < 5) throw new Exception ("Incorrect data");
         Customer customer = new () {
            Id = int.Parse (data[0]),
            FirstName = data[1],
            LastName = data[2],
            Email = data[3],
            PhoneNo = data[4]
         };
         customers.Add (customer);
      }
      return customers;
      #endregion
   }
}
#endregion