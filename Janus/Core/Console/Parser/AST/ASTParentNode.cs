using System.Collections;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	internal abstract class ASTParentNode : ASTNode, IEnumerable<ASTNode>
	{
		protected ASTParentNode(int position, int length)
			: base(position, length) { }

		#region Implementation of IEnumerable

		public abstract IEnumerator<ASTNode> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}