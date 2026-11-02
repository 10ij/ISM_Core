-- Postgres

CREATE TABLE master (
    -- GENERATED ALWAYS AS IDENTITY does the increments for me
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    -- Orgid is the id that is exposed to the user
    -- So they don't know how many tenants are stored in the system
    org_id TEXT NOT NULL UNIQUE,
    Name VARCHAR(50) NOT NULL,

    address_1 VARCHAR(255) NULL,
    address_2 VARCHAR(255) NULL,
    address_3 VARCHAR(255) NULL,

    telephone VARCHAR(20) NULL,
    email VARCHAR(50) NULL,

    website VARCHAR(255) NULL,

    reporting_period_start DATE DEFAULT CURRENT_DATE,
    reporting_period_length INTEGER DEFAULT 3,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);