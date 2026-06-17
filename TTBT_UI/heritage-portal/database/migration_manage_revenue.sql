-- ============================================================
-- MIGRATION: Thêm quyền "manage_revenue" (Quản lý doanh thu)
-- Chỉ chạy file này nếu bạn ĐÃ tạo database từ schema.sql cũ
-- (trước khi có menu "Quản lý doanh thu" trên web).
-- Nếu bạn mới tạo database lần đầu bằng schema.sql mới nhất,
-- thì KHÔNG cần chạy file này (đã có sẵn).
-- ============================================================

USE heritage_portal;

INSERT INTO permissions (code, description)
SELECT 'manage_revenue', 'Quản lý doanh thu (thêm/sửa/xóa)'
WHERE NOT EXISTS (SELECT 1 FROM permissions WHERE code = 'manage_revenue');

-- admin: được quyền này
INSERT INTO role_permissions (role_id, permission_id)
SELECT 1, id FROM permissions WHERE code = 'manage_revenue'
AND NOT EXISTS (
  SELECT 1 FROM role_permissions WHERE role_id = 1
  AND permission_id = (SELECT id FROM permissions WHERE code = 'manage_revenue')
);

-- accountant (kế toán): được quyền này
INSERT INTO role_permissions (role_id, permission_id)
SELECT 2, id FROM permissions WHERE code = 'manage_revenue'
AND NOT EXISTS (
  SELECT 1 FROM role_permissions WHERE role_id = 2
  AND permission_id = (SELECT id FROM permissions WHERE code = 'manage_revenue')
);
