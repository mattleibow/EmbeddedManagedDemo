using System;
using System.Collections.Generic;

namespace ManagedLibrary
{
	public class Calculator
	{
		private static readonly Dictionary<Operation, string> symbols = new Dictionary<Operation, string>
		{
			{ Operation.Add, "+" },
			{ Operation.Subtract, "-" },
			{ Operation.Multiply, "×" },
			{ Operation.Divide, "÷" },
			{ Operation.Invert, "±" },
			{ Operation.Square, "²" },
			{ Operation.SquareRoot, "√" },
			{ Operation.Equals, "=" },
		};

		private double operand;

		public Calculator()
		{
			Clear();
		}

		public double PreviousOperand { get; private set; }
		public Operation Operation { get; private set; }

		public double Operand
		{
			get { return operand; }
			set
			{
				operand = value;
				HasOperand = true;
			}
		}

		public bool HasOperand { get; set; }
		public string EX { get; private set; }

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
						Operand = Math.Sqrt(Operand);
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
				else if (HasOperand)
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
				if (op == Operation.Equals)
				{
					op = Operation.None;
				}

				// prepare for the next operation
				Operation = op;
				Operand = 0.0;
				HasOperand = false;
			}
		}

		public void Clear()
		{
			PreviousOperand = 0.0;
			Operation = Operation.None;
			Operand = 0.0;
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

		public static Operation GetOperation(string symbol)
		{
			symbol = symbol?.Trim() ?? string.Empty;

			foreach (var pair in symbols)
			{
				if (pair.Value.Equals(symbol, StringComparison.OrdinalIgnoreCase))
				{
					return pair.Key;
				}
			}

			return Operation.None;
		}

		public static string GetSymbol(Operation op)
		{
			if (op == Operation.None)
				return "<none>";

			string symbol;
			if (symbols.TryGetValue(op, out symbol))
				return symbol;

			return "<unknown>";
		}

		public string HandleSymbol(string display, string symbol)
		{
			if (string.IsNullOrEmpty(symbol))
				return display;

			if (char.IsDigit(symbol[0]))
			{
				// handle numbers

				// first "real", non-zero digit replaces
				if (!HasOperand || display.Equals("0", StringComparison.OrdinalIgnoreCase))
					display = symbol;
				else
					display += symbol;

				// update calulator
				Operand = double.Parse(display);
			}
			else if (symbol.Equals(".", StringComparison.OrdinalIgnoreCase))
			{
				// handle the dot

				// the dot is the first "digit"
				if (!HasOperand)
				{
					Operand = 0;
					display = "0";
				}

				// the dot can just be added on
				if (!display.Contains("."))
					display += symbol;
			}
			else if (symbol.Equals("C", StringComparison.OrdinalIgnoreCase))
			{
				Clear();
				display = Operand.ToString();
			}
			else
			{
				var op = Calculator.GetOperation(symbol);
				if (op != Operation.None)
				{
					// handle the operations
					PerformOperation(op);

					// update UI
					if (HasOperand)
						display = Operand.ToString();
					else
						display = PreviousOperand.ToString();
				}
			}

			return display;
		}
	}
}
