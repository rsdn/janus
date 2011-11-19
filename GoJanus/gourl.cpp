// gourl.cpp : Implementation of CGoUrl
#include "stdafx.h"
#include "Gojanus.h"
#include "gourl.h"

//---------------------------------------------------------------------------
// CGoUrl
//---------------------------------------------------------------------------
const TCHAR* lpszPipeName = _T("\\\\.\\pipe\\JanusPipe"); // pipe name
//---------------------------------------------------------------------------
STDMETHODIMP CGoUrl::SendURLToJanus(LONG messageID, BSTR linkText)
{
	USES_CONVERSION;
	if(!linkText)
		return E_POINTER;

	TCHAR* convertedText = OLE2T(linkText);

	if(!convertedText)
		return E_OUTOFMEMORY;


	TCHAR* fmtBegin = _T("<download-topic><message-id>%lu</message-id><hint>");
	TCHAR* fmtEnd = _T("</hint></download-topic>");

	DWORD bufferLength = lstrlen(convertedText) + lstrlen(fmtBegin) 
		+ lstrlen(fmtEnd) + sizeof(TCHAR) * 40;

	if (bufferLength > 5000)
		return E_OUTOFMEMORY;

	int lenInbyte = sizeof(TCHAR) * bufferLength;
	TCHAR* writeBuffer = (TCHAR*)alloca(lenInbyte);

	if(!writeBuffer)
		return E_OUTOFMEMORY;

	wsprintf(writeBuffer, fmtBegin,messageID);
	lstrcat(writeBuffer,convertedText);
	lstrcat(writeBuffer,fmtEnd);
	
	return writeToPipe(writeBuffer);
}
//---------------------------------------------------------------------------
STDMETHODIMP CGoUrl::ShowMessageInJanus(LONG mid)
{
	//USES_CONVERSION;

	TCHAR* fmtBegin = _T("<goto-message><message-id>%lu</message-id></goto-message>");
	DWORD bufferLength = lstrlen(fmtBegin) + getLongLen(mid) + 1;
	if (bufferLength > 5000)
		return E_OUTOFMEMORY;

	int lenInbyte = sizeof(TCHAR) * bufferLength;
	TCHAR* writeBuffer = (TCHAR*)alloca(lenInbyte);

	if(!writeBuffer)
		return E_OUTOFMEMORY;

	wsprintf(writeBuffer, fmtBegin,mid);

	return writeToPipe(writeBuffer);
}
//---------------------------------------------------------------------------
HRESULT CGoUrl::writeToPipe(LPTSTR szMessage)
{
	USES_CONVERSION;

	int lenInChar = lstrlen(szMessage);
	LPWSTR wszData = T2W(szMessage);
	DWORD dataLength = lenInChar * sizeof(WCHAR);

	// Try to open a named pipe; wait for it, if necessary.
	CHandle hPipe;
	while (1) 
	{ 
		HANDLE handle = CreateFile( 
			lpszPipeName,                  // pipe name 
			GENERIC_READ | GENERIC_WRITE,  // read and write access 
			0,                             // no sharing 
			NULL,                          // default security attributes
			OPEN_EXISTING,                 // opens existing pipe 
			0,                             // default attributes 
			NULL);                        // no template file 

		if (handle != INVALID_HANDLE_VALUE) 
		{
			hPipe.Attach(handle);
			break; 
		}
		if (GetLastError() != ERROR_PIPE_BUSY)
		{
			return S_FALSE;
		}
		if (!WaitNamedPipe(lpszPipeName, 5000)) 
		{
			return S_FALSE;
		}
	} 
	// The pipe connected; change to message-read mode. 
	DWORD dwMode = PIPE_READMODE_MESSAGE;
	BOOL fSuccess = SetNamedPipeHandleState( hPipe, &dwMode,	NULL, NULL);
	if (!fSuccess) 
	{
		return S_FALSE;
	}
	DWORD cbWritten = 0;
	// Send a message to the pipe server.
	fSuccess = WriteFile( hPipe, &dataLength, 4, &cbWritten, NULL);
	if (!fSuccess) 
	{
		return S_FALSE;
	}
	fSuccess = WriteFile( hPipe, wszData, dataLength,	&cbWritten, NULL);
	if (!fSuccess) 
	{
		return S_FALSE;
	}
	return S_OK;
}