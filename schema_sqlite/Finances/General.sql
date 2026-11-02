CREATE TABLE general_entry_type (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NULL,

    -- C# Needs to look credit and debit against an enum
    -- Debit = -1, Credit = 1
    -- Multiply the amount of this value to the amount in the related entry
    -- Then sum all entries up to get total a teacher owes (Overpayment) or is owed
    affect_on_balance VARCHAR(6) NOT NULL CHECK (affect_on_balance IN ('Debit', 'Credit')),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE general_ledger (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    amount DECIMAL(9, 2) NOT NULL,

    name VARCHAR(50) NOT NULL,
    description VARCHAR(255) NULL,

    -- Could be due to a cash injection, general expenses, an investment in capital
    
    entry_type_id INTEGER NULL REFERENCES general_entry_type(id),

    -- Rather than being deleted, store the rectification of an error as a rectification
    is_rectification BOOLEAN NOT NULL DEFAULT FALSE,
    rectification_of INTEGER NULL REFERENCES general_ledger(id),

    note TEXT NULL,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,

    -- Cant have a rectification with an entry type
    CONSTRAINT check_rectification_correct CHECK (
        (is_rectification = TRUE AND entry_type_id IS NULL AND rectification_of IS NOT NULL)
        OR
        (is_rectification = FALSE AND entry_type_id IS NOT NULL AND rectification_of IS NULL)
    )
);