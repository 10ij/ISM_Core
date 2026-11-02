CREATE TABLE student (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id),

    title iNT NULL CHECK (title IN (0, 1, 2, 3, 4, 5)),

    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,

    gender INT NULL CHECK (gender IN (0, 1)), -- 0 = Male, 1 = Female

    date_of_birth DATE NULL,

    notes TEXT NULL,

    allergies TEXT NULL,
    
    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    medical_conditions TEXT NULL,

    telephone VARCHAR(20) NULL,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);