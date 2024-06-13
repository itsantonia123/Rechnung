-- Tabelle Stadt erstellen
CREATE TABLE IF NOT EXISTS Stadt (
    PLZ CHAR(5) PRIMARY KEY,
    Name1 VARCHAR(255)
);
-- Tabelle Kunde erstellen
CREATE TABLE IF NOT EXISTS Kunde (
    KundenNr INT PRIMARY KEY,
    PLZ CHAR(5),
    Straße VARCHAR(255),
    Vorname VARCHAR(255),
    Nachname VARCHAR(255),
    FOREIGN KEY (PLZ) REFERENCES Stadt(PLZ)
);
-- Tabelle Bank erstellen
CREATE TABLE IF NOT EXISTS Bank (
    BLZ CHAR(8) PRIMARY KEY,
    Name1 VARCHAR(255)
);
-- Tabelle Verkäufer erstellen
CREATE TABLE IF NOT EXISTS Verkaeufer (
    VerkaeuferID INT PRIMARY KEY,
    PLZ CHAR(5),
    BLZ CHAR(8),
    KontoNr CHAR(20),
    Name1 VARCHAR(255),
    Straße VARCHAR(255),
    Geschäftsführung VARCHAR(255),
    Amtsgericht VARCHAR(255),
    HBR VARCHAR(255),
    Fax VARCHAR(20),
    Telefon VARCHAR(20),
    Steuernummer VARCHAR(20),
    UStIDNr VARCHAR(20),
    FOREIGN KEY (PLZ) REFERENCES Stadt(PLZ),
    FOREIGN KEY (BLZ) REFERENCES Bank(BLZ)
);
-- Tabelle Artikel erstellen
CREATE TABLE IF NOT EXISTS Artikel (
    ArtikelNr INT PRIMARY KEY,
    Einzelpreis DECIMAL(10, 2),
    Bezeichnung VARCHAR(255)
);
-- Tabelle Rechnung erstellen
CREATE TABLE IF NOT EXISTS Rechnung (
    AuftragsNr INT PRIMARY KEY,
    VerkaeuferID INT,
    KundenNr INT,
    Datum DATE,
    Fälligkeitsdatum DATE,
    Bearbeiter VARCHAR(255),
    Bruttowert DECIMAL(10, 2),
    Nettowert DECIMAL(10, 2),
    Bestellungsart VARCHAR(50),
    Zahlungsvermerkung TEXT,
    FOREIGN KEY (VerkaeuferID) REFERENCES Verkaeufer(VerkaeuferID),
    FOREIGN KEY (KundenNr) REFERENCES Kunde(KundenNr)
);
-- Tabelle Position erstellen
CREATE TABLE IF NOT EXISTS Position (
    PositionsId INT PRIMARY KEY,
    PositionNr INT,
    ArtikelNr INT,
    AuftragsNr INT,
    Menge INT,
    Gesamtpreis DECIMAL(10, 2),
    FOREIGN KEY (ArtikelNr) REFERENCES Artikel(ArtikelNr),
    FOREIGN KEY (AuftragsNr) REFERENCES Rechnung(AuftragsNr)
);