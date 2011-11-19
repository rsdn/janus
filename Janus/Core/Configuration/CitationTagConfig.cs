using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for CitationTagConfig.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class CitationTagConfig
	{
		public override string ToString()
		{
			return string.Empty;
		}

		[JanusDisplayName(SR.Config.TagLine.Citation.Citations.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.Citation.Citations.DescriptionResourceName)]
		public string[] Citations { get; set; }

		public int LastPosition;

		private const CitationQueryType _defaultQueryType = CitationQueryType.Random;
		private CitationQueryType _queryType = _defaultQueryType;
		[JanusDisplayName(SR.Config.TagLine.Citation.QueryType.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.Citation.QueryType.DescriptionResourceName)]
		[DefaultValue(_defaultQueryType)]
		public CitationQueryType QueryType
		{
			get {return _queryType;}
			set {_queryType = value;}
		}
	}
}
