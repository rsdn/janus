using System;

namespace Rsdn.Janus
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ChangePropertyAttribute : Attribute 
	{
		private readonly ChangeActionType _action;

		public ChangePropertyAttribute( ChangeActionType actionType)
		{
			_action = actionType;
		}

		public ChangeActionType ActionType
		{
			get { return _action; }
		}
	}
}
