using System;

namespace ManagedLibrary
{
	public class Calculator
	{
		private decimal operand;

		public Calculator()
		{
			Clear();
		}

		public decimal PreviousOperand { get; private set; }
		public Operation Operation { get; private set; }

		public decimal Operand
		{
			get { return operand; }
			set
			{
				operand = value;
				HasOperand = true;
			}
		}

		public bool HasOperand { get; set; }

		public void PerformOperation(Operation op)
		{
			// we can only handle a few operations
			if (!Enum.IsDefined(typeof(Operation), op))
			{
				return;
			}

			// unary
			if (IsUnaryOperation(op))
			{
				switch (op)
				{
					case Operation.Invert:
						Operand *= -1;
						break;
					case Operation.Square:
						Operand *= Operand;
						break;
					case Operation.SquareRoot:
						Operand = (decimal)Math.Sqrt((double)Operand);
						break;
				}
			}

			// binary
			if (IsBinaryOperation(op) || op == Operation.Equals)
			{
				if (!HasOperand)
				{
					// there is only a single operand, so re-use it
					Operand = PreviousOperand;
				}

				if (Operation == Operation.None)
				{
					// this is the first operation
					PreviousOperand = Operand;
				}
				else
				{
					switch (Operation)
					{
						case Operation.Add:
							PreviousOperand = PreviousOperand + Operand;
							break;
						case Operation.Subtract:
							PreviousOperand = PreviousOperand - Operand;
							break;
						case Operation.Multiply:
							PreviousOperand = PreviousOperand * Operand;
							break;
						case Operation.Divide:
							PreviousOperand = PreviousOperand / Operand;
							break;
					}
				}

				// the equals operation is not a real operation
				if (Operation == Operation.Equals)
				{
					op = Operation.None;
				}

				// prepare for the next operation
				Operation = op;
				Operand = decimal.Zero;
				HasOperand = false;
			}
		}

		public void Clear()
		{
			PreviousOperand = decimal.Zero;
			Operation = Operation.None;
			Operand = decimal.Zero;
			HasOperand = false;
		}

		public static bool IsUnaryOperation(Operation op)
		{
			return
				op == Operation.Invert ||
				op == Operation.Square ||
				op == Operation.SquareRoot;
		}

		public static bool IsBinaryOperation(Operation op)
		{
			return
				op == Operation.Add ||
				op == Operation.Subtract ||
				op == Operation.Multiply ||
				op == Operation.Divide;
		}
	}
}
