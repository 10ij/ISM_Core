CREATE TABLE parent (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id) ,

    title iNT NOT NULL CHECK (title IN (0, 1, 2, 3, 4, 5)),

    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,

    gender INT NOT NULL CHECK (gender IN (0, 1)), -- 0 = Male, 1 = Female

    date_of_birth DATE NOT NULL,

    home_telephone VARCHAR(13) NULL,
    mobile_telephone VARCHAR(13) NOT NULL,

    notes TEXT NULL,

    address_1 VARCHAR(255) NULL,

    is_archived BOOLEAN NOT NULL DEFAULT FALSE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modifed TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE parentstudent (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,

    parent_id INT NOT NULL REFERENCES parent(id) ,
    student_id INT NOT NULL REFERENCES student(id) ,

    relationship INT NOT NULL CHECK (relationship IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 9)),

    -- Putting unique here means no student at all can have the same val
    contact_priority INT NOT NULL,

    -- Make sure the student doesnt have two people on the same priority order
    CONSTRAINT parent_student_order
        UNIQUE (student_id, contact_priority),

    CONSTRAINT parent_student_unique
        UNIQUE (parent_id, student_id),

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modifed TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);