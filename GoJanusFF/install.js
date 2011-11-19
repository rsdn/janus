const APP_NAME = "GoJanusFF";
const APP_PACKAGE = "GoJanusFF@rsdn.ru";
const APP_VERSION = "1.4.1";
const APP_LOCALES = ["en-US", "ru-RU"];
const APP_PREF_FILE = "gojanusff.js"
const APP_DISPLAY_NAME = "GoJanusFF extension"

initInstall(APP_NAME, APP_PACKAGE, APP_VERSION);

var err = addDirectory(APP_PACKAGE, APP_VERSION, "components", getFolder("components"), null);

if (err == SUCCESS)
{
	err = addDirectory(APP_PACKAGE, APP_VERSION, "chrome", getFolder(getFolder("Profile", "chrome"), "GoJanusFF"), null);
}

if (err == SUCCESS)
{
	err = addFile(APP_PACKAGE, APP_VERSION, "defaults/preferences/" + APP_PREF_FILE, 
		getFolder(getFolder(getFolder("Program"),"defaults"),"pref"), null);
}

if (err == SUCCESS)
{
   const chromeFlag = DELAYED_CHROME;
   var dir = getFolder(getFolder("Profile", "chrome"), "GoJanusFF");

   registerChrome(CONTENT | chromeFlag, dir, "content/");
   registerChrome(SKIN | chromeFlag, dir, "skin/");

   for (var i=0 ; i<APP_LOCALES.length ; i++)
   {
	  registerChrome(LOCALE | chromeFlag, dir, "locale/" + APP_LOCALES[i] + "/");
   }

   err = performInstall();
	
   if (err == SUCCESS || err == 999)
   {
   }
   else
   {
	  alert("Install failed! Error code: " + err);
	  cancelInstall(err);
   }
}
else
{
   alert("Failed to install " + APP_DISPLAY_NAME + " " + APP_VERSION);
   cancelInstall(err);
}
