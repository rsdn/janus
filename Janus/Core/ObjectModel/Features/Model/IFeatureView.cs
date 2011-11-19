namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Summary description for IFeatureView.
	/// </summary>
	public interface IFeatureView
	{
		void Activate(IFeature feature);
		void Refresh();
		void ConfigChanged();
	}
}
