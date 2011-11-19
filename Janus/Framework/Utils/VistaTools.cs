using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

//sample by Tim Anderson http://www.itwriting.com/blog
//Email: tim@itwriting.com

//THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED
//TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.

//This sample is a C# implementation of some of the functions in 
//Andrei Belogortseff [ http://www.tweak-uac.com ]
//though IsReallyVista is nothing to do with Andrei

//The intention is to make it easy for .NET developers to discover whether or not 
//UAC is enabled and/or the current process is elevated

namespace Rsdn.Janus
{
	[StructLayout(LayoutKind.Sequential)]
	public struct TOKEN_ELEVATION
	{
		public uint TokenIsElevated;
	}

	public enum TOKEN_ELEVATION_TYPE
	{
		TokenElevationTypeDefault = 1,
		TokenElevationTypeFull = 2,
		TokenElevationTypeLimited = 3
	}

	public enum TOKEN_INFORMATION_CLASS
	{
		TokenUser = 1,
		TokenGroups = 2,
		TokenPrivileges = 3,
		TokenOwner = 4,
		TokenPrimaryGroup = 5,
		TokenDefaultDacl = 6,
		TokenSource = 7,
		TokenType = 8,
		TokenImpersonationLevel = 9,
		TokenStatistics = 10,
		TokenRestrictedSids = 11,
		TokenSessionId = 12,
		TokenGroupsAndPrivileges = 13,
		TokenSessionReference = 14,
		TokenSandBoxInert = 15,
		TokenAuditPolicy = 16,
		TokenOrigin = 17,
		TokenElevationType = 18,
		TokenLinkedToken = 19,
		TokenElevation = 20,
		TokenHasRestrictions = 21,
		TokenAccessInformation = 22,
		TokenVirtualizationAllowed = 23,
		TokenVirtualizationEnabled = 24,
		TokenIntegrityLevel = 25,
		TokenUIAccess = 26,
		TokenMandatoryPolicy = 27,
		TokenLogonSid = 28,
		// should always be the last enum
		MaxTokenInfoClass = 29
	}

	[Flags]
	public enum AccessMask : uint
	{
		/// <summary>
		/// The right to delete the object.
		/// </summary>
		DELETE = 0x00010000,
		/// <summary>
		/// The right to read the information in the object's security descriptor,
		/// not including the information in the SACL.
		/// </summary>
		READ_CONTROL = 0x00020000,
		/// <summary>
		/// The right to modify the DACL in the object's security descriptor.
		/// </summary>
		WRITE_DAC = 0x00040000,
		/// <summary>
		/// The right to change the owner in the object's security descriptor.
		/// </summary>
		WRITE_OWNER = 0x00080000,
		/// <summary>
		/// The right to use the object for synchronization.
		/// This enables a thread to wait until the object is in the signaled state.
		/// Some object types do not support this access right.
		/// </summary>
		SYNCHRONIZE = 0x00100000,

		STANDARD_RIGHTS_EXECUTE = READ_CONTROL,
		STANDARD_RIGHTS_READ = READ_CONTROL,
		STANDARD_RIGHTS_WRITE = READ_CONTROL,
		STANDARD_RIGHTS_REQUIRED = DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER,
		STANDARD_RIGHTS_ALL = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE,

		GENERIC_READ = 0x80000000,
		GENERIC_WRITE = 0x40000000,
		GENERIC_EXECUTE = 0x20000000,
		GENERIC_ALL = 0x10000000,

		//
		// Token Specific Access Rights.
		//

		/// <summary>
		/// Required to attach a primary token to a process.
		/// The SE_ASSIGNPRIMARYTOKEN_NAME privilege is also required to accomplish this task.
		/// </summary>
		TOKEN_ASSIGN_PRIMARY = 0x00000001,
		/// <summary>
		/// Required to duplicate an access token.
		/// </summary>
		TOKEN_DUPLICATE = 0x00000002,
		/// <summary>
		/// Required to attach an impersonation access token to a process.
		/// </summary>
		TOKEN_IMPERSONATE = 0x00000004,
		/// <summary>
		/// Required to query an access token.
		/// </summary>
		TOKEN_QUERY = 0x00000008,
		/// <summary>
		/// Required to query the source of an access token.
		/// </summary>
		TOKEN_QUERY_SOURCE = 0x00000010,
		/// <summary>
		/// Required to change the default owner, primary group, or DACL of an access token.
		/// </summary>
		TOKEN_ADJUST_DEFAULT = 0x00000080,
		/// <summary>
		/// Required to adjust the attributes of the groups in an access token.
		/// </summary>
		TOKEN_ADJUST_GROUPS = 0x00000040,
		/// <summary>
		/// Required to enable or disable the privileges in an access token.
		/// </summary>
		TOKEN_ADJUST_PRIVILEGES = 0x00000020,
		/// <summary>
		/// Required to adjust the session ID of an access token.
		/// The SE_TCB_NAME privilege is required.
		/// </summary>
		TOKEN_ADJUST_SESSIONID = 0x00000100,

		TOKEN_EXECUTE = STANDARD_RIGHTS_EXECUTE | TOKEN_IMPERSONATE,
		TOKEN_READ = STANDARD_RIGHTS_READ | TOKEN_QUERY,
		TOKEN_ALL_ACCESS =
			STANDARD_RIGHTS_REQUIRED |
				TOKEN_ASSIGN_PRIMARY |
					TOKEN_DUPLICATE |
						TOKEN_IMPERSONATE |
							TOKEN_QUERY |
								TOKEN_QUERY_SOURCE |
									TOKEN_ADJUST_PRIVILEGES |
										TOKEN_ADJUST_GROUPS |
											TOKEN_ADJUST_DEFAULT
	}

	public class VistaTools
	{
		[DllImport("advapi32.dll", SetLastError = true)]
		[return : MarshalAs(UnmanagedType.Bool)]
		private static extern bool OpenProcessToken(IntPtr ProcessHandle, AccessMask DesiredAccess,
			out IntPtr TokenHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetCurrentProcess();

		[DllImport("advapi32.dll", SetLastError = true)]
		[return : MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetTokenInformation(
			IntPtr TokenHandle,
			TOKEN_INFORMATION_CLASS TokenInformationClass,
			IntPtr TokenInformation,
			uint TokenInformationLength,
			out uint ReturnLength);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return : MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = false)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern IntPtr GetProcAddress(IntPtr hmodule, string procName);


		public static bool IsVista()
		{
			return Environment.OSVersion.Version.Major >= 6;
		}

		public static bool IsReallyVista()
		{
			var hmodule = LoadLibrary("kernel32");
			if (hmodule.ToInt32() != 0)
			{
				//just any old API function that happens only to exist on Vista and higher
				var hProc = GetProcAddress(hmodule, "CreateThreadpoolWait");
				if (hProc.ToInt32() != 0)
					return true;
			}

			return false;
		}

		public static bool IsAdmin()
		{
			var id = WindowsIdentity.GetCurrent();
			if (id != null)
			{
				var p = new WindowsPrincipal(id);
				return p.IsInRole(WindowsBuiltInRole.Administrator);
			}
			return false;
		}

		/// <summary>
		/// The possible values are:
		/// TRUE - the current process is elevated.
		/// This value indicates that either UAC is enabled, and the process was elevated by 
		/// the administrator, or that UAC is disabled and the process was started by a user 
		/// who is a member of the Administrators group.
		/// FALSE - the current process is not elevated (limited).
		/// This value indicates that either UAC is enabled, and the process was started normally, 
		/// without the elevation, or that UAC is disabled and the process was started by a standard user. 
		/// </summary>
		/// <returns>Bool indicating whether the current process is elevated</returns>
		public static bool IsElevated()
		{
			if (!IsReallyVista())
				throw new VistaToolsException("Function requires Vista or higher");

			var hProcess = GetCurrentProcess();
			if (hProcess == IntPtr.Zero)
				throw new VistaToolsException("Error getting current process handle");

			IntPtr hToken;
			if (!OpenProcessToken(hProcess, AccessMask.TOKEN_QUERY, out hToken))
				throw new VistaToolsException("Error opening process token");

			try
			{
				TOKEN_ELEVATION te;
				te.TokenIsElevated = 0;

				var teSize = Marshal.SizeOf(te);
				var tePtr = Marshal.AllocHGlobal(teSize);
				try
				{
					Marshal.StructureToPtr(te, tePtr, true);

					uint dwReturnLength;
					if (!GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevation,
						tePtr, (uint)teSize, out dwReturnLength) ||
							teSize != dwReturnLength)
						throw new VistaToolsException("Error getting token information");

					te = (TOKEN_ELEVATION)Marshal.PtrToStructure(tePtr, typeof (TOKEN_ELEVATION));
				}
				finally
				{
					Marshal.FreeHGlobal(tePtr);
				}

				return te.TokenIsElevated != 0;
			}
			finally
			{
				CloseHandle(hToken);
			}
		}

		/// <summary>
		/// TokenElevationTypeDefault - User is not using a "split" token. 
		/// This value indicates that either UAC is disabled, or the process is started
		/// by a standard user (not a member of the Administrators group).
		/// The following two values can be returned only if both the UAC is enabled and
		/// the user is a member of the Administrator's group (that is, the user has a "split" token):
		/// TokenElevationTypeFull - the process is running elevated. 
		/// TokenElevationTypeLimited - the process is not running elevated.
		/// </summary>
		/// <returns>TokenElevationType</returns>
		public static TOKEN_ELEVATION_TYPE GetElevationType()
		{
			if (!IsReallyVista())
				throw new VistaToolsException("Function requires Vista or higher");

			var hProcess = GetCurrentProcess();
			if (hProcess == IntPtr.Zero)
				throw new VistaToolsException("Error getting current process handle");

			IntPtr hToken;
			if (!OpenProcessToken(hProcess, AccessMask.TOKEN_QUERY, out hToken))
				throw new VistaToolsException("Error opening process token");

			try
			{
				var tet = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;

				var tetSize = (uint)Marshal.SizeOf((int)tet);
				var tetPtr = Marshal.AllocHGlobal((int)tetSize);
				try
				{
					uint dwReturnLength;
					if (!GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevationType,
						tetPtr, tetSize, out dwReturnLength) ||
							tetSize != dwReturnLength)
						throw new VistaToolsException("Error getting token information");

					tet = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(tetPtr);
				}
				finally
				{
					Marshal.FreeHGlobal(tetPtr);
				}

				return tet;
			}
			finally
			{
				CloseHandle(hToken);
			}
		}
	}

	// Exception class for VistaTools
	public class VistaToolsException : ApplicationException
	{
		// Default constructor
		public VistaToolsException()
		{}

		// Constructor accepting a single string message
		public VistaToolsException(string message)
			: base(message)
		{}

		// Constructor accepting a string message and an 
		// inner exception which will be wrapped by this 
		// custom exception class
		public VistaToolsException(string message, Exception inner)
			: base(message, inner)
		{}
	}
}