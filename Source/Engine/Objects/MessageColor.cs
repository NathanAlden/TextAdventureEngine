﻿using TextAdventure.Engine.Common;

namespace TextAdventure.Engine.Objects
{
	public class MessageColor : IMessagePart
	{
		private readonly Color _color;

		public MessageColor(Color color)
		{
			_color = color;
		}

		public Color Color
		{
			get
			{
				return _color;
			}
		}
	}
}