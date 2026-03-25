using System.IO;
using System.Reflection;

namespace Wordle.Models {
   #region class WordleModel ----------------------------------------------------------------------
   internal class WordleModel {
      #region class WordleModel--------------------------------------------------------------------
      #region Constructors ------------------------------------------
      /// <summary>Initializes wordle data</summary>
      public WordleModel () {
         // Secret word
         mWord = SelectWord ();
         // Dictionary to check validity of the word
         mDictionary = GetFileContent ("Wordle.Data.dictionary-5.txt");
         // Number of tries taken by the user
         Tries = 0;
         // Frequency of letters in secret word
         mFreq = GetFreq (mWord);
         // Array to store the positional validity of input letters
         Pos = new EPos[5];
      }
      #endregion

      #region Properties --------------------------------------------
      /// <summary>Input string given by the user</summary>
      public string? Input;

      /// <summary>Returns whether the game has reached the maximum number of tries</summary>
      public bool IsGameOver => Tries > 5 && Status is  not EStatus.Found;

      /// <summary>Array to store the positional validity of input letters</summary>
      public EPos[] Pos;

      /// <summary>Returns the result of current input</summary>
      public string? Result;

      /// <summary>Stores whether the word is found</summary>
      public EStatus? Status;
      public int Tries;
      #endregion

      #region Methods -----------------------------------------------
      /// <summary>Returns if the input is the secret word</summary>
      public bool IsFound (string word) => mWord == word;

      /// <summary>Returns if the input is a word</summary>
      public bool IsWord (string word) => mDictionary.Any (a => a == word);


      /// <summary>Evaluates the input word</summary>
      public void Evaluate (string word) {
         Reset ();
         Input = word;
         Status = mWord == word ? EStatus.Found : IsWord (word) ? EStatus.NotFound : EStatus.Invalid;
         if (Status != EStatus.Invalid) {
            Tries++;
            SetPos (Input);
         }
         Result = GetResult ();
      }
      #endregion

      #region Implementation ----------------------------------------
      // Set Pos array for word validity
      void SetPos (string input) {
         var inputFreq = GetFreq (input);
         for (int i = 0; i < 5; i++) {
            EPos pos = EPos.NotUsed; char ch = input[i];
            if (input[i] == mWord[i]) pos = EPos.Correct;
            else if (mFreq.TryGetValue (ch, out int count) && count <= inputFreq[ch])
               pos = EPos.Incorrect;
            inputFreq[ch]--;
            Pos[i] = pos;
         }
      }

      // Gets content from embedded source file
      string[] GetFileContent (string name) {
         using (Stream? stream = Assembly.GetExecutingAssembly ().GetManifestResourceStream (name))
            if (stream != null) return new StreamReader (stream).ReadToEnd ().Split ("\r\n");
         throw new FileNotFoundException ($"Could not find file {name}");
      }

      // Gets frequency of letters in secret word
      Dictionary<char, int> GetFreq (string word) =>
         word.Where (char.IsLetter).GroupBy (c => c).ToDictionary (c => c.Key, c => c.Count ());

      // Gets the result of game with respect to the input string
      string? GetResult () {
         if (IsGameOver) return $"Sorry! The word was {mWord}";
         if (Status != null) {
            return Status switch {
               EStatus.Invalid => $"'{Input}' is not a word",
               EStatus.Found => $"You found the word in {Tries} tries",
               EStatus.NotFound => " ",
               _ => throw new NotImplementedException ()
            };
         }
         return null;
      }

      // Reset fields for next input
      void Reset () { Input = null; Status = null; Result = null; }

      // Select random 5 letter word from file
      string SelectWord () {
         string[] puzzle = GetFileContent ("Wordle.Data.puzzle-5.txt");
         return puzzle[new Random ().Next (0, puzzle.Length)];
      }
      #endregion

      #region Private Data ------------------------------------------
      string mWord;
      string[] mDictionary;
      Dictionary<char, int> mFreq; // Stores the frequency of letters in secret word
      #endregion
      #endregion
   }
   #endregion

   #region enum EStatus ---------------------------------------------------------------------------
   /// <summary>Represents the state of the game</summary>
   public enum EStatus { Found, NotFound, Invalid }
   #endregion

   #region enum EPos ------------------------------------------------------------------------------
   // <summary>Represents the position status of input letter with respect to the secret letter</summary>
   public enum EPos { Correct, Incorrect, NotUsed }
   #endregion
}