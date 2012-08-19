using System;

using Junior.Common;

using TextAdventure.Engine.Common;
using TextAdventure.Engine.Game.Events;
using TextAdventure.Engine.Game.World;

namespace TextAdventure.Engine.Objects
{
	public class ActorInstance : Sprite, INamedObject, IDescribedObject
	{
		private readonly Guid _actorId;
		private readonly Guid _boardId;
		private readonly EventHandlerCollection _eventHandlerCollection;
		private readonly Guid _id;
		private string _description;
		private string _name;

		public ActorInstance(
			Guid id,
			string name,
			string description,
			Guid actorId,
			Guid boardId,
			Coordinate coordinate,
			Character character,
			EventHandlerCollection eventHandlerCollection = null)
			: base(coordinate, character)
		{
			name.ThrowIfNull("name");
			description.ThrowIfNull("description");

			_id = id;
			Name = name;
			Description = description;
			_actorId = actorId;
			_boardId = boardId;
			_eventHandlerCollection = eventHandlerCollection;
		}

		public Guid ActorId
		{
			get
			{
				return _actorId;
			}
		}

		public Guid BoardId
		{
			get
			{
				return _boardId;
			}
		}

		protected internal EventHandlerCollection EventHandlerCollection
		{
			get
			{
				return _eventHandlerCollection;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			protected internal set
			{
				value.ThrowIfNull("value");

				_description = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			protected internal set
			{
				value.ThrowIfNull("value");

				_name = value;
			}
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
		}

		protected internal bool ChangeCoordinate(Board board, Player player, Coordinate destinationCoordinate)
		{
			board.ThrowIfNull("board");
			player.ThrowIfNull("player");

			if (board.ActorInstanceLayer[Coordinate] != this)
			{
				throw new ArgumentException("Board's actor instance layer does not contain this actor instance.", "board");
			}

			ActorInstance destinationActorInstance = board.ActorInstanceLayer[destinationCoordinate];
			Sprite foregroundSprite = board.ForegroundLayer[destinationCoordinate];

			if (destinationActorInstance != null || foregroundSprite != null || player.Coordinate == destinationCoordinate)
			{
				return destinationActorInstance == this;
			}

			board.ActorInstanceLayer.MoveTile(Coordinate, destinationCoordinate);

			Coordinate = destinationCoordinate;

			return true;
		}

		protected internal virtual EventResult OnCreated(EventContext context, ActorInstanceCreatedEvent @event)
		{
			context.ThrowIfNull("context");
			@event.ThrowIfNull("event");

			return _eventHandlerCollection.SafeInvoke(context, @event);
		}

		protected internal virtual EventResult OnDestroyed(EventContext context, ActorInstanceDestroyedEvent @event)
		{
			context.ThrowIfNull("context");
			@event.ThrowIfNull("event");

			return _eventHandlerCollection.SafeInvoke(context, @event);
		}

		protected internal virtual EventResult OnMoved(EventContext context, ActorInstanceMovedEvent @event)
		{
			context.ThrowIfNull("context");
			@event.ThrowIfNull("event");

			return _eventHandlerCollection.SafeInvoke(context, @event);
		}

		protected internal virtual EventResult OnTouchedByActorInstance(EventContext context, ActorInstanceTouchedActorInstanceEvent @event)
		{
			context.ThrowIfNull("context");
			@event.ThrowIfNull("event");

			return _eventHandlerCollection.SafeInvoke(context, @event);
		}

		protected internal virtual EventResult OnTouchedByPlayer(EventContext context, PlayerTouchedActorInstanceEvent @event)
		{
			context.ThrowIfNull("context");
			@event.ThrowIfNull("event");

			return _eventHandlerCollection.SafeInvoke(context, @event);
		}
	}
}