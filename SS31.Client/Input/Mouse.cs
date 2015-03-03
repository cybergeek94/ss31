﻿using System;
using Microsoft.Xna.Framework.Input;

namespace SS31.Client
{
	[Flags]
	public enum MouseButton
	{
		None = 0x00,
		Left = 0x01,
		Middle = 0x02,
		Right = 0x04,
		X1 = 0x08,
		X2 = 0x10
	}

	public static class MouseButtonExtensions
	{
		public static ButtonState GetMappedState(this MouseButton button, MouseState state)
		{
			switch (button)
			{
				case MouseButton.Left:
					return state.LeftButton;
				case MouseButton.Middle:
					return state.MiddleButton;
				case MouseButton.Right:
					return state.RightButton;
				case MouseButton.X1:
					return state.XButton1;
				case MouseButton.X2:
					return state.XButton2;
				default:
					throw new Exception("Invalid Mouse Button.");
			}
		}

		public static ButtonState GetMappedState(this MouseState state, MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:
					return state.LeftButton;
				case MouseButton.Middle:
					return state.MiddleButton;
				case MouseButton.Right:
					return state.RightButton;
				case MouseButton.X1:
					return state.XButton1;
				case MouseButton.X2:
					return state.XButton2;
				default:
					throw new Exception("Invalid Mouse Button");
			}
		}
	}
}