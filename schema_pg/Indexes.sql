-- 12_Indexes.sql

-- People: "list/search this tenant's students/teachers/parents"
CREATE INDEX idx_student_tenant_name ON student (tenant_id, last_name, first_name);
CREATE INDEX idx_teacher_tenant_name ON teacher (tenant_id, last_name, first_name);
CREATE INDEX idx_parent_tenant_name  ON parent  (tenant_id, last_name, first_name);

-- Ledgers: "balance for this student/teacher" (the queries you'll run most)
CREATE INDEX idx_student_ledger_student ON student_ledger (tenant_id, student_id);
CREATE INDEX idx_teacher_ledger_teacher ON teacher_ledger (tenant_id, teacher_id);
CREATE INDEX idx_general_ledger_tenant  ON general_ledger (tenant_id);

-- Enrollment and related
CREATE INDEX idx_enrollment_student ON enrollment (tenant_id, student_id);
CREATE INDEX idx_enrollment_lesson  ON enrollment (tenant_id, lesson_id);
CREATE INDEX idx_fee_exemption_enrollment ON fee_exemption (tenant_id, enrollment_id);

-- Teaching
CREATE INDEX idx_teaching_assignment_teacher ON teaching_assignment (tenant_id, teacher_id);
CREATE INDEX idx_teaching_assignment_lesson  ON teaching_assignment (tenant_id, lesson_id);
CREATE INDEX idx_teaching_assignment_pay_assignment
    ON teaching_assignment_pay (tenant_id, teaching_assignment_id);

-- Attachments
CREATE INDEX idx_dbs_teacher ON dbs (tenant_id, teacher_id);
CREATE INDEX idx_enrollment_form_student ON enrollment_form (tenant_id, student_id);