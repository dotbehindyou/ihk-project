# Installation

## Dateien und Daten

- Downloaden Sie die aktuelle Version des Dienstes 
  [Download Link für die aktuelle Version](https://git.weissedv.de/Mogler/ihk-project) 

- Gehen Sie zum jeweiligen Kunden und zwischen Speichern Sie den Authentifizierung-Token. 
  **HINWEIS**: Sie können den "Kopieren" Knopf drücke, dieser speichert den Token in die zwischen Ablage.

![image-20200528095514859](.\assets\image-20200528095514859.png)

## Vorbereitung 

1. Öffnen Sie eine Remoteverbindung um Zugriff auf den jeweiligen Kundenserver zu bekommen.
   **ACHTUNG**: Sie benötigen Administratoren Rechte!

2. Erstellen Sie einen Ordner mit diesem Pfad: "C:\Weiss\ServiceManager" 

3. Laden Sie die aktuelle Version vom "Service Manager" hoch. ![image-20200528100329933](.\assets\image-20200528100329933.png)

4. Entpacken Sie die Dateien in den erstellten Ordner.  ![image-20200528100437076](.\assets\image-20200528100437076.png)

5. Löschen Sie danach die hochgeladene ZIP-Datei.

6. Passen Sie die "SM.Service.exe.config" an und speichern Sie diese.
	- **ConnectionString**: Connection-String für die Kundendatenbank
	- **auth_token**: Der von der Oberfläche kopierte Authentifizierung-Token.
	- **api**: Die Url zur API. **ACHTUNG**: Nur nach Absprache ändern!
	- **module_store**: Pfad wo die Dienste installiert werden. **ACHTUNG**: Nur nach Absprache ändern! 

    ![image-20200528100748708](.\assets\image-20200528100748708.png)

## Fertigstellung

1. Führen Sie "SM.Service.exe" aus. Wenn eine Meldung kommt, mit Administratoren Abfrage bestätigen Sie mit "Ja".

    ![image-20200528101752573](.\assets\image-20200528101752573.png)

2. Prüfen Sie den Zustand des Dienstes, in dem Sie die Dienstübersicht von Windows Starten. 
   So sollte die Spalten befüllt sein:

    - **Name**: Service Manager
    - **Status**: Wird ausgeführt...
    - **Starttyp**: Automatisch (Verzögert)
    - **Anmelden als**: Lokales System

    ![image-20200528102055804](.\assets\image-20200528102055804.png)
