CREATE TABLE parent (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    title INTEGER NOT NULL CHECK (title IN (0, 1, 2, 3, 4, 5)),

    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,

    gender INTEGER NOT NULL CHECK (gender IN (0, 1)), -- 0 = Male, 1 = Female

    date_of_birth DATE NOT NULL,


    home_telephone VARCHAR(20) NULL,
    mobile_telephone VARCHAR(20) NOT NULL,

    notes TEXT NULL,

    address_1 VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE parent_student (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    parent_id INTEGER NOT NULL REFERENCES parent(id),
    student_id INTEGER NOT NULL REFERENCES student(id),

    relationship INTEGER NOT NULL CHECK (relationship IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 9)),

    -- Putting unique here means no student at all can have the same val
    contact_priority INTEGER NOT NULL,

    -- Make sure the student doesnt have two people on the same priority order
    CONSTRAINTEGER parent_student_order
        UNIQUE (student_id, contact_priority),

    CONSTRAINTEGER parent_student_unique
        UNIQUE (parent_id, student_id),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);