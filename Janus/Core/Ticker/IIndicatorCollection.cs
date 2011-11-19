using System;
using System.Collections;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for IIndicatorCollection.
	/// </summary>
	public interface IIndicatorCollection : IList
	{
		void Add(ITickerIndicator ti);
		//new ITickerIndicator this[int idx] {get; set;}
	}
}
