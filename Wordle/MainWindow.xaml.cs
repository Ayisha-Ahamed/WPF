using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Wordle.Models;
using Wordle.ViewModels;
using Color = System.Windows.Media.Color;

namespace Wordle;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

   Label[,] mGrid;

   Dictionary<char, Button> mLetters = [];

   public MainWindow () {
      MinHeight = 500;
      MinWidth = 800;
      SizeToContent = SizeToContent.WidthAndHeight;
      Title = "Wordle";
      InitializeComponent ();

      string[] alphabets = [ "Q","W","E","R","T","Y","U","I","O","P","A","S","D",
                             "F","G","H","J","K","L","Z","X","C","V","B","N","M" ];
      mGrid = new Label[6, 5];
      for (int i = 0; i < 6; i++) {
         for (int j = 0; j < 5; j++) {
            Label lb = new ();
            gInput.Children.Add (lb);
            mGrid[i, j] = lb;
         }
      }

      foreach (var letter in alphabets) gAlphabets.Children.Add (new Button () { Content = letter });

      foreach (var item in gAlphabets.Children) {
         if (item is Button button)
            mLetters.Add (((string)button.Content)[0], button);
      }

      ViewWordle viewWordle = new (mGrid);
      this.DataContext = viewWordle;
   }

   void Button_OnClicked (object sender, RoutedEventArgs e) {
      var vm = (ViewWordle)DataContext; char ch;
      if (e.OriginalSource is ButtonBase button && vm.ButtonClicked.CanExecute (null)) {
         ch = ((string)button.Content)[0];
         vm.ButtonClicked.Execute (ch);
      }
   }

   SolidColorBrush GetColor (EPos pos) => pos switch {
      EPos.Incorrect => new SolidColorBrush (Color.FromArgb (255, 255, 215, 0)),
      EPos.Correct => new SolidColorBrush (Color.FromArgb (255, 34, 139, 34)),
      _ => new SolidColorBrush (Color.FromArgb (255, 105, 105, 105)),
   };

   void Key_OnPressed (object sender, KeyEventArgs e) {
      var vm = (ViewWordle) DataContext; char ch;
      if (vm.IsGameOver) Close ();
      if (e.Key is Key key) {
         switch (key) {
            case >= Key.A and <= Key.Z:
               if (vm.ButtonClicked.CanExecute (null)) {
                  ch = (char)(key - Key.A + 'A'); vm.ButtonClicked.Execute (ch);
                  e.Handled = true;
               }
               break;
            case Key.Enter:
               if (vm.EnteredInput.CanExecute (null)) {
                  vm.EnteredInput.Execute (null);
                  if (vm.Pos != null) SetColor (vm.Pos, vm.Row, vm.Input);
                  TbResult.Text = vm.GetResult ?? " ";
               }
               break;
            case Key.Back:
               if (vm.BackSpace.CanExecute (null)) vm.BackSpace.Execute (null);
               break;
         }
      }
   }

   void SetColor (EPos[] Pos, int rowNo, string input) {
      int row = rowNo - 1;
      for (int i = 0; i < 5; i++) {
         SolidColorBrush bg = GetColor (Pos[i]);
         mGrid[row, i].IsEnabled = false;
         if (mLetters.TryGetValue (input[i], out Button? button)) {
            mGrid[row, i].Foreground = button.Foreground = new SolidColorBrush (Color.FromArgb (255, 255, 255, 255));
            mGrid[row, i].Background = button.Background = bg;
         }
      }
   }
}