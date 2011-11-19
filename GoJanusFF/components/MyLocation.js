// constants
const CLASS_ID = Components.ID("{1C0E8D56-B661-40d0-AE4D-CA012FADF176}");
const CLASS_NAME = "MyLocation Component";
const CONTRACT_ID = "@rsdn.ru/janus/mylocation;1";

//class constructor
function MyLocation()
{
	this.wrappedJSObject = this;
};

//class definition
MyLocation.prototype =
{
	GetMyLocation : function()
	{
		return __LOCATION__;
	},
	
	QueryInterface: function(iid)
	{
		if (!iid.equals(Components.interfaces.nsISupports))
			throw Components.results.NS_ERROR_NO_INTERFACE;
		return this;
	}
};

//class factory
var MyLocationFactory =
{
	createInstance: function (outer, iid)
	{
		if (outer != null)
			throw Components.results.NS_ERROR_NO_AGGREGATION;
		return (new MyLocation()).QueryInterface(iid);
	}
};

//module definition (xpcom registration)
var MyLocationModule =
{
	_firstTime: true,

	registerSelf: function(compMgr, fileSpec, location, type)
	{
		if (this._firstTime)
		{
			this._firstTime = false;
			throw Components.results.NS_ERROR_FACTORY_REGISTER_AGAIN;
		};
		compMgr = compMgr.QueryInterface(Components.interfaces.nsIComponentRegistrar);
		compMgr.registerFactoryLocation(CLASS_ID, CLASS_NAME, CONTRACT_ID, fileSpec, location, type);
	},

	unregisterSelf: function(compMgr, location, type)
	{
		compMgr = compMgr.QueryInterface(Components.interfaces.nsIComponentRegistrar);
		compMgr.unregisterFactoryLocation(CLASS_ID, location);
	},

	getClassObject: function(compMgr, cid, iid)
	{
		if (!iid.equals(Components.interfaces.nsIFactory))
			throw Components.results.NS_ERROR_NOT_IMPLEMENTED;

		if (cid.equals(CLASS_ID))
			return MyLocationFactory;

		throw Components.results.NS_ERROR_NO_INTERFACE;
	},

	canUnload: function(compMgr)
	{
		return true;
	}
};

//module initialization
function NSGetModule(compMgr, fileSpec)
{
	return MyLocationModule;
}
