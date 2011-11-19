// gourl.h : Declaration of the CGoUrl

#ifndef __GOURL_H_
#define __GOURL_H_

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CGoUrl
class ATL_NO_VTABLE CGoUrl : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CGoUrl, &CLSID_gourl>,
	public IDispatchImpl<Igourl, &IID_Igourl, &LIBID_GOJANUSLib>
{
public:
	CGoUrl()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_GOURL)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CGoUrl)
	COM_INTERFACE_ENTRY(Igourl)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()

// Igourl
public:
	STDMETHOD(SendURLToJanus)(LONG messageID, BSTR linkText);
	STDMETHOD(ShowMessageInJanus)(LONG messageID);
private :
	HRESULT writeToPipe(LPTSTR);
};

#endif //__GOURL_H_
