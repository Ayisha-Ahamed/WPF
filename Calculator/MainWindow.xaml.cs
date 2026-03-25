using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Calculator;

#region class MainWindow --------------------------------------------------------------------------
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
   public MainWindow () {
      InitializeComponent ();
      ResetDisplay ();
   }

   #region Implementation -------------------------------------------
   // Applies arithmetic operation
   void ApplyArith (string op) {
      if (mNumbers.Count > 1) {
         double n2 = mNumbers.Pop (); double n1 = mNumbers.Pop (), result;
         (result, txtPrev.Text) = op switch {
            "+" => (n1 + n2, $"{n1} + {n2}"),
            "-" => (n1 - n2, $"{n1} - {n2}"),
            "*" or "x" => (n1 * n2, $"{n1}  x  {n2}"),
            "÷" => (n1 / n2, $"({n1}) / ({n2})"),
            _ => throw new NotImplementedException (),
         };
         txtDisplay.Text = $"{Math.Round (result, 12)}";
      }
   }

   // Apply function
   void ApplyFunc (string func) {
      if (mNumbers.Count > 0) {
         double n1 = mNumbers.Pop (), result;
         if (func == "=") { ResetStack (); return; }
         (result, txtPrev.Text) = func switch {
            "+/-" => (-n1, ""),
            "1/x" => (1 / n1, $"1/({n1})"),
            "x²" => (n1 * n1, $"({n1})²"),
            "²√x" => (Math.Sqrt (n1), $"√({n1})"),
            "%" => (n1 * 0.01, $"{n1 * 0.01}"),
            _ => throw new NotImplementedException (),
         };
         txtDisplay.Text = $"{Math.Round (result, 12)}";
      }
   }

   // Apply operators
   void ApplyOp () {
      if (mOperators.Count > 0) {
         switch (mOperators.Peek ()) {
            case "+" or "-" or "*" or "x" or "÷":
               if (mNumbers.Count > 1) ApplyArith (mOperators.Pop ()); break;
            default: ApplyFunc (mOperators.Pop ()); break;
         }
      }
   }

   // Routed event for button operation
   void Button_OnClicked (object sender, RoutedEventArgs e) {
      if (e.OriginalSource is ButtonBase button) {
         string content = (string)button.Content;
         BTag tag = FindTag ((string)button.Tag);
         RunControl (tag, content);
      }
   }

   // Routed event for backspace operation
   void Button_OnClickedBackSpace (object sender, RoutedEventArgs e) {
      if (txtDisplay.Text.Length <= 1) {
         ResetDisplay ();
         return;
      }
      txtDisplay.Text = txtDisplay.Text[..^1];
      e.Handled = true;
   }

   // Returns enum BTag of tag associated with the button
   BTag FindTag (string tag) =>
   tag switch {
      "Number" => BTag.Number,
      "Function" => BTag.Function,
      "Arith" => BTag.Arith,
      "UIControl" => BTag.UIControl,
      "Decimal" => BTag.Decimal,
      "Equals" => BTag.Equals,
      _ => BTag.Error,
   };

   // Returns string associated with key operation
   string GetOpStr (Key key) => key switch { Key.Subtract => "-", Key.Multiply => "x", Key.Divide => "÷", _ => "+" };

   void IsNumPressed (string digit) {
      if (mOp != null) { PushOp (mOp); mOp = null; }
      if (LastIn != BTag.Number && LastIn != BTag.Decimal) txtDisplay.Text = "";
      txtDisplay.Text += digit;
   }

   // Clear operation implementation
   void IsUIControlPressed (string ctrl) {
      ResetDisplay ();
      if (ctrl == "CE") {
         txtPrev.Text = " ";
         ResetStack ();
         LastIn = null;
      }
   }

   // Returns the integer associated with key operation
   int KeyToInt (Key key) => key switch {
      >= Key.D0 and <= Key.D9 => (int)key - (int)Key.D0,
      >= Key.NumPad0 and <= Key.NumPad9 => (int)key - (int)Key.NumPad0,
      _ => throw new Exception ("Unable to convert key to integer"),
   };

   void OnKeyPressed (object sender, KeyEventArgs e) {
      Key key = e.Key;
      BTag tag = BTag.Error;
      switch (key) {
         case (>= Key.D0 and <= Key.D9) or (>= Key.NumPad0 and <= Key.NumPad9):
            RunControl (tag = BTag.Number, KeyToInt (key).ToString ());
            break;
         case Key.Add or Key.Subtract or Key.Multiply or Key.Divide:
            RunControl (tag = BTag.Arith, GetOpStr (key)); break;
         case Key.Return: RunControl (tag = BTag.Function, "="); break;
         case Key.Back: Button_OnClickedBackSpace (sender, e); return;
         default: MessageBox.Show (key.ToString ()); break;
      }
      SetTag (tag);
   }

   // Pushes display number to stack
   void PushNum () {
      mNumbers.Push (double.Parse (txtDisplay.Text));
      ApplyOp ();
   }

   // Pushes entered operation to stack
   void PushOp (string op) {
      mOperators.Push (op);
      ApplyOp ();
   }

   // Reset display
   void ResetDisplay () => txtDisplay.Text = "0";

   // Reset Stack
   void ResetStack () {
      mOperators.Clear ();
      mNumbers.Clear ();
   }

   // Applies operation to operands
   void RunControl (BTag tag, string content) {
      if (tag == BTag.Number) IsNumPressed (content);
      //else if (LastIn != BTag.Number) return;
      switch (tag) {
         case BTag.Arith: mOp = content; break;
         case BTag.Function:
            PushNum (); PushOp (content); tag = BTag.Number;
            break;
         case BTag.Decimal:
            if (LastIn == BTag.Number) {
               Decimal.IsEnabled = false;
               txtDisplay.Text += ".";
            }
            break;
         case BTag.UIControl: IsUIControlPressed (content); return;
      }
      SetTag (tag);

   }

   // Update LastIn tag
   void SetTag (BTag currentTag) {
      // If the input changes from digits to operator, store the current numeric value
      if (LastIn is BTag.Number && currentTag != LastIn && currentTag != BTag.Decimal) {
         PushNum ();
         Decimal.IsEnabled = true;
      }
      if (currentTag == BTag.Arith) txtPrev.Text = $"{txtDisplay.Text} {mOp} ";
      LastIn = currentTag;
   }
   #endregion

   #region Private Data ---------------------------------------------
   enum BTag { Number, UIControl, Function, Arith, Decimal, Error, Equals };
   BTag? LastIn; // Store the last operation carried out by the calculator
   Stack<double> mNumbers = new ();
   string? mOp;
   Stack<string> mOperators = new ();
   double mResult => mNumbers.Count > 0 ? mNumbers.Peek () : 0;
   #endregion
   #endregion
}