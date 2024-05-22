CREATE TABLE Tags (
    TagId TEXT,
    Color INTEGER,
    Name TEXT,
    Activate INTEGER,
    PRIMARY KEY (TagId)
);

CREATE TABLE DaysOfWeek (
    TagId TEXT,
    DayOfWeek INTEGER,
    Activate INTEGER,
    PRIMARY KEY (TagId, DayOfWeek)
);

CREATE TABLE SpecificDates (
    TagId TEXT,
    SpecificDay TEXT,
    Activate INTEGER,
    PRIMARY KEY (TagId, SpecificDay)
);