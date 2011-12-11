using System;

using System.Text;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	//[MessageFormatter(FormatSource = false)]
	public class SocNetMessageFormatter : IMessageFormatter
	{
		//private const string _script =
		//  "<script type='text/javascript' src='http://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-4ee097440f437ad2'></script>";
		private const string _script =
			@"<script type='text/javascript'>
(function() {
  var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
  po.src = 'https://apis.google.com/js/plusone.js';
  var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
})();
</script>";

		private const string _buttonTemplate = "<a class='{0}' addthis:url='http://rsdn.ru{{0}}'></a>";

		private static readonly string _buttonsTemplate =
			//_buttonTemplate.FormatStr(_buttonTemplate, "addthis_button_compact")
			//  + _buttonTemplate.FormatStr(_buttonTemplate, "addthis_button_google_plusone")
			//  + _buttonTemplate.FormatStr(_buttonTemplate, "addthis_button_facebook_like");
			"<div><g:plusone annotation='inline' href='http://rsdn.ru'></g:plusone></div>";

		public string FormatSource(string source)
		{
			throw new NotImplementedException();
		}

		public string FormatHtml(string html)
		{
			var scriptIdx = html.IndexOf("</head>");
			var footerIdx = html.IndexOf("<script type=\"text/javascript\">\r\nfunction showMedia()");
			var sb = new StringBuilder(html);
			sb.Insert(scriptIdx, _script);
			footerIdx += _script.Length;
			sb.Insert(footerIdx, _buttonsTemplate);

			return sb.ToString();
		}
	}
}