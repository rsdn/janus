using System.Resources;

namespace Rsdn.Janus
{
// ReSharper disable InconsistentNaming
	static partial class SR
// ReSharper restore InconsistentNaming
	{
		static SR()
		{
			_resourceManager = new ResourceManager("Rsdn.Janus.SR", typeof (Janus).Assembly);
		}
	}
}
