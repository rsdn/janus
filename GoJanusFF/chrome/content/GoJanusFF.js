// add initializer
window.addEventListener("load", function(e) { GoJanusFFMenu.init(e); }, false);

function dump2(s)
{
	dump(s + "\n");
}
 
var GoJanusFFMenu =
{
	init: function()
	{
		dump2("> init");
		var janusMenu = document.getElementById("gojanusff-menu");
		var menu = janusMenu.parentNode;

		if (menu)
			menu.addEventListener("popupshowing", function(e) { return GoJanusFFMenu.doContext(e); }, false);
		
		this.sbErrors = Components.classes["@mozilla.org/intl/stringbundle;1"]
						.getService(Components.interfaces.nsIStringBundleService)
						.createBundle("chrome://gojanusff/locale/errors.properties");

		dump2("< init");
	},
	doContext: function()
	{
		dump2("> doContext");

		// default - no hide menu
		var hideMenu = false;

		// check if menu shall be hidden on non RSDN links
		var prefs = Components.classes["@mozilla.org/preferences-service;1"].
					getService(Components.interfaces.nsIPrefBranch);
		
		if(prefs.getBoolPref("gojanusff.showOnlyOnRsdnLinks") == true)
		{
			// check link
			var topicID = GoJanusFFMenu.getRsdnTopicID();
			hideMenu = (topicID == null);
		}
		
		// hide
		dump2("hiding menu=" + hideMenu);
		
		document.getElementById("gojanusff-sep").hidden = hideMenu;
		document.getElementById("gojanusff-menu").hidden = hideMenu;

		dump2("< doContext");

		return true;
	},
	getRsdnTopicID: function()
	{
		if(!gContextMenu.onLink)
			return null;

		return GoJanusFFMenu.getRsdnTopicIDFromURL(gContextMenu.link);
	},
	getRsdnTopicIDFromURL: function(url)
	{
		dump2("GetTopicID from url=" + url);
		try
		{
			if(url == null)
				return;
				
			var regex1 = new RegExp("http://(gzip\\.|www\\.)?rsdn\\.ru/forum/(message/|\\?action=message&gid=\\d+&mid=|\\?mid=|message\\.aspx?\\?mid=|\\?gid=\\d+&mid=|message\.)(\\d+)(\\.aspx)?","gi");
			if(regex1.test(url) == false)
				return null;

			dump2("Extracted TopicID=" + RegExp.$3);

			return parseInt(RegExp.$3);
		}
		catch(e)
		{
			dump2("getRsdnTopicIDFromURL.exception: " + e);
		}
		return null;
	},
	doOpen: function()
	{
		GoJanusFFMenu.doCmdForContextLink('go');
	},
	doDownload: function()
	{
		GoJanusFFMenu.doCmdForContextLink('download');
	},
	doDownloadAll: function()
	{
		try
		{
			var commands = [];
			
			var anchors = gContextMenu.link.ownerDocument.getElementsByTagName("A");
			
			for ( var i = 0; i < anchors.length; i++ )
			{
				var topicID = GoJanusFFMenu.getRsdnTopicIDFromURL(anchors[i].href);
				if(topicID == null)
					continue;
				
				// add topic to download
				commands.push("download" + topicID);
			}
			
			if(commands.length == 0)
				throw Components.Exception("Nothing to download");

			GoJanusFFMenu.sendCommand(commands);

			alert(this.sbErrors.GetStringFromName("ok"));
		}
		catch(e)
		{
			alert(e);
		}
	},
	doCmdForContextLink: function(cmd)
	{
		dump2("doCmdForContextLink: " + cmd);

		try{
			var topicID = GoJanusFFMenu.getRsdnTopicID();
			if(topicID == null)
			{
				alert(this.sbErrors.GetStringFromName("unknown.url"));
				return;
			}
			
			GoJanusFFMenu.sendCommand([cmd + topicID]);
			
			if(cmd == 'download')
				alert(this.sbErrors.GetStringFromName("ok"));
		}
		catch(e)
		{
			alert(e);
		}
	},
	sendCommand: function(args)
	{
		// get 'components' directory
		var file = Components.classes['@rsdn.ru/janus/mylocation;1']
			.createInstance(Components.interfaces.nsISupports)
			.wrappedJSObject.GetMyLocation().parent;

		file.append("GoJanusCmd.exe");

		var process = Components.classes['@mozilla.org/process/util;1']
			.getService(Components.interfaces.nsIProcess);
		
		process.init(file);
		process.run(true, args, args.length);
		
		switch(process.exitValue)
		{
			case 0:
				break;
			case 1:
				throw this.sbErrors.GetStringFromName("generic");
			case 2:
				throw this.sbErrors.GetStringFromName("noJanus");
			case 3:
			default:
				throw this.sbErrors.GetStringFromName("generic");
		}
	}
}
