# IHK-Projekt

##### von Christopher Mogler

## Beschreibung 

ServiceManager ist ein Dienst, der andere Dienste kontrolliert. (SM.Service)
Die Daten werden über das Internet, über eine REST-API gesendet (SM.API)
Gesteuert wird dies über eine Web-Anwendung, wo nicht im Internet laufen soll. (SM.UI)


## Projektstruktur
- *docs/* : Dokumente, Konzepte, Infos, Bilder
- *lib/* : DLLs, Libs, etc.
- *src/* : Quellcode
  - *SM.API/* : REST-API 
  - *SM.UI/*: React Anwendung (mit API)
  - *SM.Service/* : Windows-Dienst 
  - SM.Database/ : SQL, Models, etc.


## Wichtig!

- Service muss eine ZIP sein!
- Service EXE muss den gleichen Name haben, wie das Module!
- Service-Name muss gleich wie vom Modul sein!