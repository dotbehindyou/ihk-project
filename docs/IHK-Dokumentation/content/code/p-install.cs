// Installations Methode
// ----------------------
public static Boolean Install(string servicePath, string serviceName, string serviceDisplayName)
{
    //.... Default werden erstellen und zuweisen .....

    // ServiceController-Manager wird geoeffnet
    IntPtr sCtrlHandler = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
    // Konnte der SC-Manager geoeffnet werden?
    if (sCtrlHandler != IntPtr.Zero)
    { // Ja
        IntPtr sv_handle = CreateService(sCtrlHandler, servicePath, serviceName, SERVICE_ALL_ACCESS, SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, serviceDisplayName, null, 0, null, null, null);
        
        // Konnte der Dienst zum Dienstpool hinzugefuegt werden?
        if (sv_handle == IntPtr.Zero)
        {// Konnte nicht hinzugefuegt werden
            // Schliesse den SC-Manager
            CloseServiceHandle(sCtrlHandler);
            return false; // Dienst konnte nicht zum Dienstpool hinzugefuegt werden!
        }
        else
        { // Wurde hinzugefuegt
            // Hier wird der Dienst gestartet
            if (StartService(sCtrlHandler, 0, null) == 0)
            {// Wenn 0 zurueck gegeben wird, konnte der Dienst nicht gestartet werden!
                return false; // Dienst konnte nicht gestartet werden!
            }
            
            // Schliesse den SC-Manager
            CloseServiceHandle(sCtrlHandler);
            return true; // Dienst konnte erfolgreich hinzugefuegt und gestartet werden!
        }
    }
    else
        return false; // SC-Manager konnte nicht geoeffnet werden!
}
