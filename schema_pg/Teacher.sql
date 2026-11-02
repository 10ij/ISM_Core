CREATE TABLE teacher (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id),

    title iNT NULL CHECK (title IN (0, 1, 2, 3, 4, 5)),

    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,

    gender INT NULL CHECK (gender IN (0, 1)), -- 0 = Male, 1 = Female

    date_of_birth DATE NULL,

    home_telephone VARCHAR(20) NULL,
    mobile_telephone VARCHAR(20) NOT NULL,

    notes TEXT NULL,

    address_1 VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);