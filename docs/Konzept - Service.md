# Konzept des Service

## Datenabruf

1. **Lade die Daten von der API**

   - Baue eine HTTP(s) Verbindung auf
   - Rufe die REST-Url auf 
   - Prüfe die Rückmeldung auf HTTP-Code
   - Serialisiere die JSON in C#-Objekte 

2. **Prüfung der aktuellen Lage**

   - Gehe die einzelnen Objekte der JSON druch
   - Hat sich der Token geändert? Version ändern auf die angegebene Version 
   - Hat sich die Konfig geändert? Config aktualisieren vom Modul
   - Ist der Status als REMOVE markiert worden? Deinstalliere das Modul 

3. **Installation**

   - *Prüfe ob Dienst installiert ist*

   - Downloade die Versionsdatei des Moduls über die API
   - Öffne den Modul-Ordner
   - Erstelle einen neuen Ordner, mit den Name des Moduls und öffne diesen dann
   - Entpacke die Dateien der ZIP, in den geöffneten Ordner
   - Speicher die Konfig-Datei ab, die von der API mit geliefert wurde
   - Installiere das Modul als Dienst
   - Starte den Dienst 

4. **Update**

   - *Prüfe ob Dienst installiert ist*

   - Downloade die Versionsdateien des Moduls über die API
   - Öffne den Modul-Ordner und danach den Ordner mit dem Name des Moduls
   - Stoppe den Dienst
   - Entpacke, die Runtergeladene Modul-ZIP in den geöffneten Ordner, überschreibe vorhandene Dateien
   - Speicher die Konfig-Datei ab, die von der API mit geliefert wurde
   - Starte den Dienst

5. **Deinstallation**

   - *Prüfe ob Dienst installiert ist*

   - Stoppe den Dienst
   - Deinstalliere den Dienst

6. **Statusmeldung**

   - Prüfe den Status des Dienst 
   - Melde die Dienste die nicht laufen

7. **Idle**

   - Nach 60 min. soll die komplette Prüfung wiederholt werden.

## Datenaufbau

```JSON
{
    "meta": { // META-Informationen der API und Datenbank, für spätere erweiterungen 
        ...
    },
    "modules" : [ // Hier ist die komplette List, welche Module sich geändert haben
        {
            "module_ID": {GUID},//  ID des Moduls,
            "moduleName": {String},//  Name des Moduls
            "version": {String},//  aktuelle Version, die installiert sein soll
            "config": { // Konfig
                "fileName": {String}, // der Name der Konfig-Datei (mit Endung!)
                "data": {String}, // Inhalt der Konfig
            },
            "releaseDate": {DateTime},// wann wurde diese Version veröffentlich
            "validationToken": {Byte[](Base64String)},// Validierung ob installierte Version gleich mit zuinstallieren ist
    		"status": {String},// aktuelle Status (INIT, UPDATE, REMOVE)
        },
        ...
    ]
}
```

## REST

- Authentication 

  Die Schnittstelle authentifiziert sich mir dem Auth_Token, dieser ist in der Konfigdatei gespeichert.
  Der Auth-Token kann für den jeweiligen Kunden über die UI kopiert werden.
  Der Auth-Token wird als HTTP-Header mit gegeben.

