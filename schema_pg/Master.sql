-- Postgres

CREATE TABLE master (
    -- GENERATED ALWAYS AS IDENTITY does the increments for me
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    -- Orgid is the id that is exposed to the user
    -- So they don't know how many tenants are stored in the system
    -- org_id UUID NOT NULL UNIQUE DEFAULT gen_random_uuid(),
    -- Instead of a GUID, I can use some cheap encryption algorithm 
    -- Before a Primary Key or Foreign Key is sent to the client over http
    -- Its encrypted and then decrypted app side when running a query
    Name VARCHAR(50) NOT NULL, 

    address_1 VARCHAR(255) NULL,
    address_2 VARCHAR(255) NULL,
    address_3 VARCHAR(255) NULL,

    telephone VARCHAR(20) NULL,
    email VARCHAR(50) NULL,

    website VARCHAR(255) NULL,

    reporting_period_start DATE DEFAULT CURRENT_DATE,
    reporting_period_length INT DEFAULT 3,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);