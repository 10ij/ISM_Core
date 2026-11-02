CREATE TABLE fee_exemption (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),


    fixed_amount DECIMAL(8, 2) NULL,
    percentage_amount DECIMAL(3, 2) NULL,

    is_recurring BOOLEAN NOT NULL DEFAULT FALSE,

    note TEXT NULL,

    date_started DATE NOT NULL DEFAULT CURRENT_DATE,
    date_ended DATE NULL,

    enrollment_id INTEGER NOT NULL REFERENCES enrollment(id),

    -- A fee exemption can be is_archived if it is no longer in use

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    -- Can only have a percentage concession
    -- Or a fixed amount concession, not both
    CONSTRAINTEGER check_fee_exemption_amount CHECK (
        (fixed_amount IS NOT NULL AND percentage_amount IS NULL)
        OR
        (fixed_amount IS NULL AND percentage_amount IS NOT NULL)
    ),

    CHECK (fixed_amount IS NULL OR fixed_amount >= 0),
    CHECK (
        percentage_amount IS NULL
        OR percentage_amount BETWEEN 0 AND 1
    ),

    CHECK (date_ended IS NULL OR date_ended >= date_started),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE student_entry_type (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NULL,

    -- C# Needs to look credit and debit against an enum
    -- Debit = -1, Credit = 1
    -- Multiply the amount of this value to the amount in the related entry
    -- Then sum all entries up to get total a student owes or is owed
    affect_on_balance VARCHAR(10) NOT NULL CHECK (affect_on_balance IN ('Debit', 'Credit')),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE lesson_fee (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    lesson_id INTEGER NOT NULL REFERENCES lesson(id),

    amount DECIMAL(8, 2) NOT NULL CHECK (amount >= 0),

    date_started DATE NOT NULL DEFAULT CURRENT_DATE,
    date_ended DATE NULL,

    CONSTRAINTEGER chk_fee_rate_dates
        CHECK (date_ended IS NULL OR date_ended > date_started),

    CONSTRAINTEGER unique_dates UNIQUE (tenant_id, lesson_id, date_started),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE student_ledger (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),
    student_id INTEGER NOT NULL REFERENCES student(id),

    -- Nullable because it can be non fee
    -- Like making them pay for vandalism or something
    lesson_fee_id INTEGER NULL REFERENCES lesson_fee(id),

    amount DECIMAL(8, 2) NOT NULL,

    -- Could be due to a payment, or a write off, or refund

    entry_type_id INTEGER NULL REFERENCES student_entry_type(id),

    -- Rather than being deleted, store the rectification of an error as a rectification
    is_rectification BOOLEAN NOT NULL DEFAULT FALSE,
    -- The ID of the ledger entry that this is rectifying
    rectification_of INTEGER NULL REFERENCES student_ledger(id),

    note TEXT NULL,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    -- Cannot be modified to stop dodgy stuff

    -- Cant have a rectification with an entry type
    CONSTRAINTEGER check_rectification_correct CHECK (
        (is_rectification = TRUE AND entry_type_id IS NULL AND rectification_of IS NOT NULL)
        OR
        (is_rectification = FALSE AND entry_type_id IS NOT NULL AND rectification_of IS NULL)
    )
);