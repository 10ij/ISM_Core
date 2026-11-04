CREATE TABLE security_users (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id),

    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,

    teacher_id INT NULL REFERENCES teacher(id),

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modified TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMPTZ NULL,
);

-- No point in adding advanced RBAC now like the docker app
-- too much work as it is
-- Do a table alter when it comes to that
-- And make a seperate table for RBAC