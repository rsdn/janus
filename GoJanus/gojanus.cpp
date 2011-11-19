// gojanus.cpp : Implementation of DLL Exports.


// Note: Proxy/Stub Information
//      To build a separate proxy/stub DLL, 
//      run nmake -f gojanusps.mk in the project directory.

#include "stdafx.h"
#include "resource.h"
#include <initguid.h>
#include "gojanus.h"

#include "gojanus_i.c"
#include "gourl.h"


CComModule _Module;

BEGIN_OBJECT_MAP(ObjectMap)
OBJECT_ENTRY(CLSID_gourl, CGoUrl)
END_OBJECT_MAP()

void DeletePrevInstance()
{
	TCHAR* janusguid = _T("{63C4751A-5B1B-41DD-862B-C1230691B403}");
	TCHAR* rootKey = _T("Software\\Microsoft\\Internet Explorer\\MenuExt\\");

	HKEY hCU;
	LONG lResult = RegOpenKeyEx(HKEY_CURRENT_USER,rootKey,
								0,KEY_ENUMERATE_SUB_KEYS | KEY_SET_VALUE | KEY_QUERY_VALUE | KEY_CREATE_SUB_KEY,
								&hCU);

	if(ERROR_SUCCESS == lResult)
	{

		TCHAR buffer[MAX_PATH] = {0};
		DWORD idx = 0;

		TCHAR subkeyName[MAX_PATH] = {0};
		DWORD subkeyLength = MAX_PATH;

		lResult = RegEnumKeyEx(hCU,idx,subkeyName,&subkeyLength,NULL,NULL,NULL,NULL);
		if(ERROR_SUCCESS == lResult)
		{
			do
			{
				lstrcpy(buffer,rootKey);
				lstrcat(buffer,subkeyName);

				
				HKEY hCheckKey;
				lResult = RegOpenKeyEx(HKEY_CURRENT_USER,buffer,0,KEY_QUERY_VALUE,&hCheckKey);
				if(ERROR_SUCCESS == lResult)
				{
					
					DWORD dwTmp;
					lResult = RegQueryValueEx(hCheckKey,janusguid,NULL,NULL,NULL,&dwTmp);
					if(ERROR_SUCCESS == lResult)
					{
						RegCloseKey(hCheckKey);
						//RegCloseKey(hCU);
						//hCU = NULL;

						RegDeleteKey(HKEY_CURRENT_USER,buffer);

						//continue;

					} // if(ERROR_SUCCESS == lResult)
					else
					{
						RegCloseKey(hCheckKey);
					}
				} // if(ERROR_SUCCESS == lResult)


				subkeyLength = MAX_PATH;
				idx++;
			} while(RegEnumKeyEx(hCU,idx,subkeyName,&subkeyLength,NULL,NULL,NULL,NULL) != ERROR_NO_MORE_ITEMS);

		} // if(ERROR_SUCCESS == lResult)

		if(hCU)
			RegCloseKey(hCU);
	} // if(ERROR_SUCCESS == lResult)
}

/////////////////////////////////////////////////////////////////////////////
// DLL Entry Point

extern "C"
BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID /*lpReserved*/)
{
    if (dwReason == DLL_PROCESS_ATTACH)
    {
        _Module.Init(ObjectMap, hInstance, &LIBID_GOJANUSLib);
        DisableThreadLibraryCalls(hInstance);
    }
    else if (dwReason == DLL_PROCESS_DETACH)
        _Module.Term();
    return TRUE;    // ok
}

/////////////////////////////////////////////////////////////////////////////
// Used to determine whether the DLL can be unloaded by OLE

STDAPI DllCanUnloadNow(void)
{
    return (_Module.GetLockCount()==0) ? S_OK : S_FALSE;
}

/////////////////////////////////////////////////////////////////////////////
// Returns a class factory to create an object of the requested type

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
    return _Module.GetClassObject(rclsid, riid, ppv);
}

/////////////////////////////////////////////////////////////////////////////
// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
	DeletePrevInstance();
    // registers object, typelib and all interfaces in typelib
    return _Module.RegisterServer(TRUE);
}

/////////////////////////////////////////////////////////////////////////////
// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{

    HRESULT hr = _Module.UnregisterServer(TRUE);

	DeletePrevInstance();

	return hr;
}


