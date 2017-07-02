using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Mihailik.InternetExplorer
{
    public static class NonAdminComRegistration
    {
        public static void Unregister<T>()
            where T : new()
        {
            try
            {
                AdminUnregister<T>();
            }
            catch (UnauthorizedAccessException)
            {
                NonAdminUnregister<T>();
            }
        }

        static void AdminUnregister<T>()
            where T : new()
        {
            string clsid = typeof(T).GUID.ToString("b");

            string clsidKeyPath = @"CLSID\" + typeof(T).GUID.ToString("B");
            Registry.ClassesRoot.DeleteSubKeyTree(clsidKeyPath);

            string progIdPath = typeof(T).FullName;
            Registry.ClassesRoot.DeleteSubKeyTree(progIdPath);
        }

        static void NonAdminUnregister<T>()
            where T : new()
        {
            string clsid = typeof(T).GUID.ToString("b");

            string clsidKeyPath = @"Software\Classes\CLSID\" + typeof(T).GUID.ToString("B");
            Registry.CurrentUser.DeleteSubKeyTree(clsidKeyPath);

            string progIdPath = @"Software\Classes\" + typeof(T).FullName;
            Registry.CurrentUser.DeleteSubKeyTree(progIdPath);
        }

        public static void Register<T>(AssemblyRegistrationFlags registrationFlags)
            where T : new()
        {
            try
            {
                AdminRegister<T>(registrationFlags);
            }
            catch(UnauthorizedAccessException)
            {
                NonAdminRegister<T>(registrationFlags);
            }
        }

        static void AdminRegister<T>(AssemblyRegistrationFlags registrationFlags)
            where T : new()
        {
            string clsid = typeof(T).GUID.ToString("b");
            string clsidKeyPath = @"CLSID\" + typeof(T).GUID.ToString("B");
            using (RegistryKey clsidKey = Registry.ClassesRoot.CreateSubKey(clsidKeyPath))
            {
                clsidKey.SetValue(null, typeof(T).FullName);

                using (RegistryKey inproc32Key = clsidKey.CreateSubKey("InprocServer32"))
                {
                    inproc32Key.SetValue(null, "mscoree.dll");

                    inproc32Key.SetValue("Assembly", typeof(T).Assembly.FullName);
                    inproc32Key.SetValue("Class", typeof(T).FullName);
                    inproc32Key.SetValue("RuntimeVersion", typeof(T).Assembly.ImageRuntimeVersion);
                    inproc32Key.SetValue("ThreadingModel", "Both");

                    if (registrationFlags == AssemblyRegistrationFlags.SetCodeBase)
                    {
                        inproc32Key.SetValue("CodeBase", typeof(T).Assembly.CodeBase);
                    }
                    else
                    {
                        if (inproc32Key.GetValue("CodeBase") != null)
                            inproc32Key.DeleteValue("CodeBase");
                    }
                }

                using (RegistryKey implementedCategoriesKey = clsidKey.CreateSubKey("Implemented Categories"))
                {
                    implementedCategoriesKey.CreateSubKey("{62C8FE65-4EBB-45E7-B440-6E39B2CDBF29}");
                }

                using (RegistryKey progIdKey = clsidKey.CreateSubKey("ProgId"))
                {
                    progIdKey.SetValue(null, typeof(T).FullName);
                }
            }

            string progIdPath = typeof(T).FullName;
            using (RegistryKey progIdKey = Registry.ClassesRoot.CreateSubKey(progIdPath))
            {
                using (RegistryKey clsidKey = progIdKey.CreateSubKey("CLSID"))
                {
                    clsidKey.SetValue(null, clsid);
                }
            }
        }

        static void NonAdminRegister<T>(AssemblyRegistrationFlags registrationFlags)
            where T : new()
        {
            string clsid = typeof(T).GUID.ToString("b");
            string clsidKeyPath = @"Software\Classes\CLSID\" + typeof(T).GUID.ToString("B");
            using (RegistryKey clsidKey = Registry.CurrentUser.CreateSubKey(clsidKeyPath))
            {
                clsidKey.SetValue(null, typeof(T).FullName);

                using (RegistryKey inproc32Key = clsidKey.CreateSubKey("InprocServer32"))
                {
                    inproc32Key.SetValue(null, "mscoree.dll");

                    inproc32Key.SetValue("Assembly", typeof(T).Assembly.FullName);
                    inproc32Key.SetValue("Class", typeof(T).FullName);
                    inproc32Key.SetValue("RuntimeVersion", typeof(T).Assembly.ImageRuntimeVersion);
                    inproc32Key.SetValue("ThreadingModel", "Both");

                    if (registrationFlags == AssemblyRegistrationFlags.SetCodeBase)
                    {
                        inproc32Key.SetValue("CodeBase", typeof(T).Assembly.CodeBase);
                    }
                    else
                    {
                        if( inproc32Key.GetValue("CodeBase")!=null )
                            inproc32Key.DeleteValue("CodeBase");
                    }
                }

                using (RegistryKey implementedCategoriesKey = clsidKey.CreateSubKey("Implemented Categories"))
                {
                    implementedCategoriesKey.CreateSubKey("{62C8FE65-4EBB-45E7-B440-6E39B2CDBF29}");
                }

                using (RegistryKey progIdKey = clsidKey.CreateSubKey("ProgId"))
                {
                    progIdKey.SetValue(null, typeof(T).FullName);
                }
            }

            string progIdPath = @"Software\Classes\" + typeof(T).FullName;
            using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey(progIdPath))
            {
                using (RegistryKey clsidKey = progIdKey.CreateSubKey("CLSID"))
                {
                    clsidKey.SetValue(null, clsid);
                }
            }
        }
    }
}
