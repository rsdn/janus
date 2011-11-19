using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, привязывающий к элементу редактор.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class OutboxItemEditorAttribute : Attribute
	{
		private readonly Type _editorType;

		public OutboxItemEditorAttribute(Type editorType)
		{
			_editorType = editorType;
		}

		public Type EditorType
		{
			get { return _editorType; }
		}
	}
}
