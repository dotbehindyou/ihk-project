// Importieren der DLL AdvAPI32
// -----------------------------
[DllImport("advapi32.dll")]
// Funktion um SC-Manager zu oeffnen, damit der Zugriff auf Dienstpool gewaehrt wird
public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
[DllImport("advapi32.dll")]
// Funktion um Dienste hinzuzufuegen im Dienstpool
public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
[DllImport("advapi32.dll")]
// Schliesse den SC-Manager
public static extern void CloseServiceHandle(IntPtr SCHANDLE);
[DllImport("advapi32.dll")]
// Starte den Diensts
public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);
[DllImport("advapi32.dll")]
// Oeffne den Dienst vom Dienstpool, um Dienst zu bearbeiten
public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
[DllImport("advapi32.dll")]
// Entferne den Dienst vom Dienstpool
public static extern int DeleteService(IntPtr SVHANDLE);
