CREATE TABLE security_logs (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id INT NOT NULL REFERENCES master(id),

    user_id INT NOT NULL REFERENCES security_users(id),
    action VARCHAR(50) NOT NULL,
    details TEXT NULL,

    date_created TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);