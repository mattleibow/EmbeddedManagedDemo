using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

using ManagedLibrary;

namespace ManagediOSLibrary
{
	public class CalculatorView : UIView
	{
		private static readonly string[] labels = {
			"C", "±", "²", "√",
			"7", "8", "9", "÷",
			"4", "5", "6", "×",
			"1", "2", "3", "-",
			"0", ".", "=", "+",
		};

		private UIButton[] buttons;
		private Calculator calculator;
		private nfloat buttonMargin;
		private nfloat buttonPadding;
		private UILabel display;
		private UIView displayContainer;

		public CalculatorView()
		{
			Initialize();
		}

		public CalculatorView(CGRect frame)
			: base(frame)
		{
			Initialize();
		}

		protected CalculatorView(IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		private void Initialize()
		{
			calculator = new Calculator();

			buttons = labels.Select(l =>
			{
				var btn = new UIButton(UIButtonType.System);
				btn.SetTitle(l, UIControlState.Normal);
				btn.TouchUpInside += OnButtonTapped;
				return btn;
			}).ToArray();
			AddSubviews(buttons);

			display = new UILabel
			{
				TextAlignment = UITextAlignment.Right,
				Text = "0",
				Lines = 1,
				BackgroundColor = UIColor.White.ColorWithAlpha(0.0f)
			};
			displayContainer = new UIView();
			displayContainer.AddSubview(display);
			AddSubview(displayContainer);

			buttonMargin = 12;
			buttonPadding = 6;
			Layer.CornerRadius = 6;
			ButtonCornerRadius = 3;
		}

		[Export("calculator")]
		public CalculatorProxy CalculatorProxy => new CalculatorProxy(calculator);

		[Export("buttonMargin")]
		public nfloat ButtonMargin
		{
			get { return buttonMargin; }
			set
			{
				buttonMargin = value;
				SetNeedsLayout();
			}
		}

		[Export("buttonPadding")]
		public nfloat ButtonPadding
		{
			get { return buttonPadding; }
			set
			{
				buttonPadding = value;
				SetNeedsLayout();
			}
		}

		[Export("buttonCornerRadius")]
		public nfloat ButtonCornerRadius
		{
			get { return buttons[0].Layer.CornerRadius; }
			set
			{
				foreach (var btn in buttons)
				{
					btn.Layer.CornerRadius = value;
				}
				displayContainer.Layer.CornerRadius = value;
			}
		}

		[Export("buttonColor")]
		public UIColor ButtonColor
		{
			get { return buttons[0].BackgroundColor; }
			set
			{
				foreach (var btn in buttons)
				{
					btn.BackgroundColor = value;
				}
				displayContainer.BackgroundColor = value;
			}
		}

		private void OnButtonTapped(object sender, EventArgs e)
		{
			var btn = sender as UIButton;
			var symbol = btn?.Title(UIControlState.Normal)?.Trim();

			display.Text = calculator.HandleSymbol(display.Text, symbol);
		}

		public void Reset()
		{
			calculator.Clear();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			const int across = 4;
			const int down = 5;

			var sizeX = (Bounds.Width - (buttonMargin + (across - 1) * buttonPadding + buttonMargin)) / across;
			var sizeY = (Bounds.Height - (buttonMargin + (down + 1 - 1) * buttonPadding + buttonMargin)) / (down + 1);

			for (int y = 0; y < down; y++)
			{
				for (int x = 0; x < across; x++)
				{
					var pos = new CGPoint(buttonMargin + x * (sizeX + buttonPadding), buttonMargin + (y + 1) * (sizeY + buttonPadding));
					buttons[(across * y) + x].Frame = new CGRect(pos, new CGSize(sizeX, sizeY));
				}
			}

			displayContainer.Frame = new CGRect(buttonMargin, buttonMargin, Bounds.Width - buttonMargin - buttonMargin, sizeY);
			display.Frame = CGRect.FromLTRB(buttonPadding, buttonPadding, displayContainer.Bounds.Width - buttonPadding, displayContainer.Bounds.Height - buttonPadding);
		}
	}
}
