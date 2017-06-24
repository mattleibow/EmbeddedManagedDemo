using Foundation;

using ManagedLibrary;

namespace ManagediOSLibrary
{
	public class CalculatorProxy : NSObject
	{
		private Calculator calculator;

		public CalculatorProxy(Calculator calculator)
		{
			this.calculator = calculator;
		}

		[Export("previousOperand")]
		public NSDecimalNumber PreviousOperand => new NSDecimalNumber(calculator.PreviousOperand);

		[Export("operation")]
		public Operation Operation => calculator.Operation;

		[Export("operand")]
		public NSDecimalNumber Operand => new NSDecimalNumber(calculator.Operand);

		[Export("hasOperand")]
		public bool HasOperand => calculator.HasOperand;
	}
}
