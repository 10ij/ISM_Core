CREATE TABLE teaching_assignment (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    teacher_id INTEGER NOT NULL REFERENCES teacher(id),
    lesson_id INTEGER NOT NULL REFERENCES lesson(id),

    is_substitute BOOLEAN NOT NULL DEFAULT FALSE,
    substitute_date DATE NULL,

    date_started DATE NULL,
    date_ended DATE NULL,

    -- Substitutes have their own date
    CONSTRAINTEGER check_schedule_dates CHECK (
        (is_substitute = FALSE AND date_started IS NOT NULL AND substitute_date IS NULL)
        OR
        (is_substitute = TRUE AND date_started IS NULL AND date_ended IS NULL AND substitute_date IS NOT NULL)
    ),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE teaching_assignment_pay (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tenant_id INTEGER NOT NULL REFERENCES master(id),

    teaching_assignment_id INTEGER NOT NULL REFERENCES teaching_assignment(id),

    -- The default for this will be handled in C#
    lesson_rate DECIMAL(8, 2) NOT NULL CHECK (lesson_rate >= 0),

    date_started DATE NOT NULL DEFAULT CURRENT_DATE,
    date_ended DATE NULL,

    CONSTRAINTEGER chk_teaching_assignment_pay_dates
        CHECK (date_ended IS NULL OR date_ended > date_started),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);