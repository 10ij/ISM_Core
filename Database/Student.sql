CREATE TABLE Student (
    ID INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    TenantID INT NOT NULL REFERENCES Master(ID) ,

    Title iNT NULL CHECK (Title IN (0, 1, 2, 3, 4, 5)),

    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,

    Gender INT NULL CHECK (Gender IN (0, 1)), -- 0 = Male, 1 = Female

    DoB DATE NULL,

    Notes TEXT NULL,

    Allergies TEXT NULL,

    MedicalConditions TEXT NULL,

    Telephone VARCHAR(13) NULL,

    DateCreated TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DateModified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);