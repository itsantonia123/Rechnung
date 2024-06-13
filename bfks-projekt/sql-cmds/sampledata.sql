--Stadt Daten
INSERT INTO Stadt(PLZ, Name1)
VALUES ('12345', 'Neuheim'),
    ('34056', 'Mauerbach');
--Kunden Daten
INSERT INTO Kunde(KundenNr, PLZ, Straße, Vorname, Nachname)
VALUES (
        1025637,
        '34056',
        'Dinkelstrasse 39',
        'Manfred',
        'Heller'
    );
--Bank Daten
INSERT INTO Bank(BLZ, Name1)
VALUES ('7712344', 'Reutsche Bank');
--Verkäufer Daten
INSERT INTO Verkaeufer(
        VerkaeuferID,
        PLZ,
        BLZ,
        KontoNr,
        Name1,
        Straße,
        Geschäftsführung,
        Amtsgericht,
        HBR,
        Fax,
        Telefon,
        Steuernummer,
        UStIDNr
    )
VALUES (
        1,
        '12345',
        '7712344',
        '76532xxx',
        'Winkler AG',
        'Schulstraße 1',
        'Müller',
        'Amtsgericht Karlsbund',
        '87xxx',
        '(+49) xxxx 300701',
        '(+49) xxxx 300700',
        '1425xxxx',
        '98632xxx'
    );
--Artikel Daten
INSERT INTO Artikel(ArtikelNr, Einzelpreis, Bezeichnung)
VALUES (
        11020351,
        40.00,
        '2048MB DDR (400) PC-3200 DIMM 184-pin Kit (2 x 1024MB)'
    ),
    (
        11020346,
        195.00,
        'Intel Core 2 Duo E8500, 2 x 3166 MHz, 1333MHz FSB, Box'
    );
--Rechnung Daten
INSERT INTO Rechnung(
        AuftragsNr,
        VerkaeuferID,
        KundenNr,
        Datum,
        Fälligkeitsdatum,
        Bearbeiter,
        Bruttowert,
        Nettowert,
        Bestellungsart,
        Zahlungsvermerkung
    )
VALUES (
        201002001,
        1,
        1025637,
        '2019-02-20',
        '2010-03-20',
        'Last',
        550.00,
        462.18,
        'Online',
        'Überweisung'
    );
--Position Daten
INSERT INTO Position(
        PositionsId,
        PositionNr,
        ArtikelNr,
        AuftragsNr,
        Menge,
        Gesamtpreis
    )
VALUES (
        1,
        1,
        11020351,
        201002001,
        4,
        160.00
    ),
    (
        2,
        2,
        11020346,
        201002001,
        2,
        390.00
    );