using System;
using System.Linq;
using System.Xml.Linq;

using Junior.Common;

using TextAdventure.Engine.Objects;

namespace TextAdventure.Engine.Serializers.Xml
{
	public class BoardSerializer
	{
		public static readonly BoardSerializer Instance = new BoardSerializer();

		private BoardSerializer()
		{
		}

		public XElement Serialize(Board board, string elementName = "board")
		{
			board.ThrowIfNull("board");
			elementName.ThrowIfNull("elementName");

			return new XElement(
				elementName,
				SpriteLayerSerializer.Instance.Serialize(board.BackgroundLayer, "backgroundLayer"),
				SpriteLayerSerializer.Instance.Serialize(board.ForegroundLayer, "foregroundLayer"),
				ActorInstanceLayerSerializer.Instance.Serialize(board.ActorInstanceLayer),
				board.Exits.Select(arg => BoardExitSerializer.Instance.Serialize(arg)),
				board.Timers.Select(arg => TimerSerializer.Instance.Serialize(arg)),
				EventHandlerCollectionSerializer.Instance.Serialize(board.EventHandlerCollection),
				new XAttribute("id", board.Id),
				new XAttribute("name", board.Name),
				new XAttribute("description", board.Description),
				new XAttribute("size", SizeSerializer.Instance.Serialize(board.Size)));
		}

		public Board Deserialize(XElement boardElement)
		{
			boardElement.ThrowIfNull("boardElement");

			return new Board(
				(Guid)boardElement.Attribute("id"),
				(string)boardElement.Attribute("name"),
				(string)boardElement.Attribute("description"),
				SizeSerializer.Instance.Deserialize((string)boardElement.Attribute("size")),
				SpriteLayerSerializer.Instance.Deserialize(boardElement.Element("backgroundLayer")),
				SpriteLayerSerializer.Instance.Deserialize(boardElement.Element("foregroundLayer")),
				ActorInstanceLayerSerializer.Instance.Deserialize(boardElement.Element("actorInstanceLayer")),
				boardElement.Elements("boardExit").Select(BoardExitSerializer.Instance.Deserialize),
				boardElement.Elements("timer").Select(TimerSerializer.Instance.Deserialize),
				EventHandlerCollectionSerializer.Instance.Deserialize(boardElement));
		}
	}
}