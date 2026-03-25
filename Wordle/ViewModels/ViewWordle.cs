using System.Windows.Controls;
using Wordle.Models;

namespace Wordle.ViewModels {
   #region class ViewWordle -----------------------------------------------------------------------
   internal class ViewWordle {

      #region Constructor -------------------------------------------
      public ViewWordle (Label[,] boxes) {
         mGrid = boxes;
         ButtonClicked = new RelayCommand (ExecuteButtonClicked, CanExecuteButtonClicked);
         mInput = new char[5];
         mWordle = new WordleModel ();
         EnteredInput = new RelayCommand (ExecuteEvaluate, CanExecuteEvaluate);
         BackSpace = new RelayCommand (ExecuteBackSpace, CanExecuteBackSpace);
      }
      #endregion

      #region Properties --------------------------------------------
      /// <summary>Stores the position validity of input string</summary>
      public EPos[]? Pos;

      /// <summary>Stores the row position of the input</summary>
      public int Row => mRow;

      /// <summary>Stores the input string</summary>
      public string Input => mString;

      /// <summary>Stores the result corresponding to the input/summary>
      public string? GetResult => mWordle.Result;

      public bool IsGameOver => mWordle.IsGameOver;
      #endregion

      #region Methods -----------------------------------------------
      public RelayCommand ButtonClicked { get; set; }
      public RelayCommand EnteredInput { get; set; }
      public RelayCommand BackSpace { get; set; }
      #endregion

      #region Implementation ----------------------------------------
      bool CanExecuteButtonClicked (object? obj) => !mIsGameOver && mCol <= 4;

      bool CanExecuteBackSpace (object? obj) => !mIsGameOver && mCol > 0;

      bool CanExecuteEvaluate (object? obj) => !mIsGameOver && mString.Length == 5;

      void ExecuteBackSpace (object? obj) {
         mGrid[mRow, --mCol].Content = string.Empty;
      }

      void ExecuteButtonClicked (object? obj) {
         if (obj is not null) mInput[mCol] = (char)obj;
         mGrid[mRow, mCol].Content = $"{mInput[mCol]}";
         mCol++;
      }

      void ExecuteEvaluate (object? obj) {
         mWordle.Evaluate (mString); // Evaluate word
         if (mWordle.Status != EStatus.Invalid) {
            Pos = mWordle.Pos;
            mRow = mWordle.Tries;
            mCol = 0;
         } else Pos = null;
      }
      #endregion

      #region Private Data ------------------------------------------
      int mCol = 0, mRow;
      Label[,] mGrid;
      char[] mInput;
      bool mIsGameOver => mWordle.IsGameOver;
      string mString => new (mInput);
      readonly WordleModel mWordle;
      #endregion
   }
   #endregion
}