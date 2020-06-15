
// Deinstallations Methode
// ------------------------
public static Boolean Uninstall(string serviceName)
{
    // .... oeffne SC-Manager(=sCtrlHandler); Siehe Install Methode! ....
    
    int DELETE = 0x10000; // Delete 
    // Oeffne den ServiceHandler
    IntPtr serviceHandler = OpenService(sCtrlHandler, serviceName, DELETE);
    if (serviceHandler != IntPtr.Zero)
    { // ServiceHandler konnte geoeffnet werden 
        if (DeleteService(serviceHandler) != 0){ //HINWEIS! Der ServiceHandler stoppt automatisch den Dienst, beim deinstallieren!
            // Schliesse den SC-Manager 
            CloseServiceHandle(sCtrlHandler);
            return true; // Der Dienst konnte erfolgreich entfernt werden
        }
        else
        {
            // Schliesse den SC-Manager
            CloseServiceHandle(sCtrlHandler);
            return false; // Der Dienst konnte NICHT entfernt werden!
        }
    }
    else
        return false;
}