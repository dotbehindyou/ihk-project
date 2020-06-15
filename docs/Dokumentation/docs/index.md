# How-To

## Dienst anlegen

1. Gehen Sie auf die Dienstübersicht und drücken Sie dann auf "Dienst hinzufügen"-Knopf.
   <img src=".\assets\image-20200528083243019.png" alt="image-20200528083243019" style="zoom:50%;" />

2. Geben Sie in das Eingabefeld den gewünschten Namen ein. Drücken Sie danach auf den "Erstellen"-Knopf.
   <img src=".\assets\image-20200528083321794.png" alt="image-20200528083321794" style="zoom:50%;" />

## Version hinzufügen 

1. Gehen Sie in den gewünschten Dienst. Und drücken unter der Versionsliste, auf "Version hinzufügen"
   <img src=".\assets\image-20200528083519166.png" alt="image-20200528083519166" style="zoom:50%;" />

2. Befüllen Sie die Felder, mit den entsprechenden Werten. Und drücken Sie danach auf "Speichern"
   
	- **Version**: Was für eine Version wollen Sie hinzufügen?
	- **Veröffentlicht**: Wann wurde die Version veröffentlicht?
	- **Versionsdatei (ZIP)**: Die aktuellen Dateien der Version, als ZIP komprimiert
	- **Dateiname der Konfig**: Mit welchem Dateiname (+ Endung) soll die Konfig gespeichert werden.
	- **Konfig Format**: Was für ein Format verwendet die Konfig? Ist für Syntax highlighting zuständig.
	- **Konfig (Inhalt)**: Ist die eigentliche **Muster**-Konfig. 

   <img src=".\assets\image-20200528083834092.png" alt="image-20200528083834092" style="zoom:50%;" />

3. Nach dem erfolgreichen Speichern ist der Eintrag in der Versionsliste hinzugefügt, beim jeweiligen Dienst.
![image-20200528084049589](.\assets\image-20200528084049589.png)

## Kunden initialisieren / bearbeiten

1. Um Kunden zu initialisieren / bearbeiten, drücken Sie bei der Kundenübersicht, auf das grüne Plus-Zeichen oder blaues Editiert-Zeichen. Sollte das Blau sein, so wurde der Kunde bereits initialisiert und kann bearbeitet werden. ACHTUNG: Das bedeutet nur, dass die Sätze für die Datenbank initialisiert wurden! NICHT das die Ressourcen auf Kundenserver installiert sind.
   ![image-20200528091618923](.\assets\image-20200528091618923.png)

2. Nach dem erfolgreichen Initialisieren oder wenn Sie bearbeiten gedrückt haben, bekommen Sie die Kundenverwaltung. 
	- **Authentifizierung-Token**: Dieser Token muss beim Installieren des Verwaltungsdienst in der Konfig hinterlegt werden!
	- **Installierte Dienste**: alle Dienste die beim Kunden hinzugefügt wurden, mit dem jeweiligen Status
   ![image-20200528092258941](.\assets\image-20200528092258941.png)

## Dienst einem Kunden hinzufügen

Beachten Sie bitte, dass die benötigten Ressourcen beim Kunden installiert sein muss!

1. Bearbeiten Sie den jeweiligen Kunden und drücken Sie auf "Dienst hinzufügen". 
   ![](.\assets\image-20200528084827996.png)
2. Wählen Sie den Dienst aus, über den grünen Knopf, der Installiert werden soll.
   ![image-20200528084920161](.\assets\image-20200528084920161.png)
3. Wählen Sie die entsprechende Version aus, mit dem grünen Knopf.
   ![image-20200528084942535](.\assets\image-20200528084942535.png)
4. Wenn Sie die Version ausgewählt haben, wird die jeweilige Muster-Konfig geladen. Diese Konfig **müssen** Sie anpassen. Danach können Sie auf "Hinzufügen" drücken.
   ![image-20200528085233240](.\assets\image-20200528085233240.png)
5. Beim Kunden wird der Dienst jetzt Installiert. Sie sehen den Status "INIT", dass bedeutet das der Dienst beim Kunden initialisiert wird. Beim erfolgreichen Installieren wird der Status mit "Running" ersetzt.
   ![image-20200528085312164](.\assets\image-20200528085312164.png) 

## Einzelne Funktionen

- Wie lösche ich ein Dienst?
  ![image-20200528085550838](.\assets\image-20200528085550838.png)
- Wie lösche ich eine Version?![image-20200528085824210](.\assets\image-20200528085824210.png)
- Wie entferne ich einen Dienst beim Kunden?![image-20200528085853602](.\assets\image-20200528085853602.png)

