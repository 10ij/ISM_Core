CREATE TABLE location (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    Name VARCHAR(50) NOT NULL,

    headteacher_id INTEGER NULL REFERENCES teacher(id),
    deputy_id INTEGER NULL REFERENCES teacher(id),
    supervisor_id INTEGER NULL REFERENCES teacher(id),

    -- In code, this should default to org settings
    telephone VARCHAR(20) NULL,
    email VARCHAR(50) NULL,
    address_1 VARCHAR(255) NULL,
    website VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE offering (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);


-- Link of the above two
CREATE TABLE lesson (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    location_id INTEGER NOT NULL REFERENCES location(id),
    offering_id INTEGER NOT NULL REFERENCES offering(id),

    notes TEXT NULL,

    days_of_week INTEGER NOT NULL CHECK (days_of_week < 128 AND days_of_week >= 1), -- 7 bits for 7 days of the week

    room VARCHAR(50) NULL,

    default_pay DECIMAL(8, 2) NULL CHECK (default_pay >= 0),

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);