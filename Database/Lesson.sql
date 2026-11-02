CREATE TABLE location (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id) ,

    Name VARCHAR(50) NOT NULL,

    headteacher_id INT NULL REFERENCES teacher(id) ON DELETE SET NULL,
    deputy_id INT NULL REFERENCES teacher(id) ON DELETE SET NULL,
    supervisor_id INT NULL REFERENCES teacher(id) ON DELETE SET NULL,

    -- In code, this should default to org settings
    telephone VARCHAR(13) NULL,
    email VARCHAR(50) NULL,
    address_1 VARCHAR(255) NULL,
    website VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modifed TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE offering (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id) ,

    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modifed TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);


-- Link of the above two
CREATE TABLE lesson (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id),

    locationid INT NOT NULL REFERENCES location(id),
    offeringid INT NOT NULL REFERENCES offering(id),

    notes TEXT NULL,

    days_of_week INT NOT NULL CHECK (days_of_week < 128), -- 7 bits for 7 days of the week

    room VARCHAR(50) NULL,

    default_pay DECIMAL(8, 2) NULL CHECK (default_pay >= 0),

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modifed TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);