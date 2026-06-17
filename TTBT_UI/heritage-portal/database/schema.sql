-- ============================================================
-- DATABASE: heritage_portal
-- Mô tả: Database mô phỏng cho Cổng thông tin điện tử
-- TT Bảo tồn Di tích Cố đô Huế
-- ============================================================

CREATE DATABASE IF NOT EXISTS heritage_portal CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE heritage_portal;

-- ===================== PHÂN QUYỀN =====================

CREATE TABLE roles (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(50) UNIQUE NOT NULL,
  description VARCHAR(255)
);

CREATE TABLE permissions (
  id INT AUTO_INCREMENT PRIMARY KEY,
  code VARCHAR(100) UNIQUE NOT NULL,
  description VARCHAR(255)
);

CREATE TABLE role_permissions (
  role_id INT NOT NULL,
  permission_id INT NOT NULL,
  PRIMARY KEY (role_id, permission_id),
  FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE CASCADE,
  FOREIGN KEY (permission_id) REFERENCES permissions(id) ON DELETE CASCADE
);

CREATE TABLE users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(50) UNIQUE NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  full_name VARCHAR(100),
  role_id INT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (role_id) REFERENCES roles(id)
);

-- ===================== DỮ LIỆU BÁO CÁO (MÔ PHỎNG) =====================

CREATE TABLE ticket_revenue (
  id INT AUTO_INCREMENT PRIMARY KEY,
  month INT NOT NULL,
  year INT NOT NULL,
  adult_count INT DEFAULT 0,
  adult_revenue DECIMAL(15,2) DEFAULT 0,
  child_count INT DEFAULT 0,
  child_revenue DECIMAL(15,2) DEFAULT 0,
  priority_count INT DEFAULT 0,
  priority_revenue DECIMAL(15,2) DEFAULT 0
);

CREATE TABLE other_revenue (
  id INT AUTO_INCREMENT PRIMARY KEY,
  month INT NOT NULL,
  year INT NOT NULL,
  category VARCHAR(100),
  amount DECIMAL(15,2) DEFAULT 0
);

CREATE TABLE expenses (
  id INT AUTO_INCREMENT PRIMARY KEY,
  month INT NOT NULL,
  year INT NOT NULL,
  category VARCHAR(100),
  description VARCHAR(255),
  amount DECIMAL(15,2) DEFAULT 0
);

CREATE TABLE conservation_projects (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  status VARCHAR(50),
  budget DECIMAL(15,2) DEFAULT 0,
  spent DECIMAL(15,2) DEFAULT 0
);

-- ===================== DỮ LIỆU MẪU: ROLES =====================
-- id 1 = admin, 2 = accountant (kế toán), 3 = staff, 4 = user

INSERT INTO roles (name, description) VALUES
('admin', 'Quản trị viên - toàn quyền'),
('accountant', 'Kế toán - quản lý báo cáo thu chi'),
('staff', 'Nhân viên - bán vé, quản lý hồ sơ'),
('user', 'Người dùng - xem thông tin công khai');

-- ===================== DỮ LIỆU MẪU: PERMISSIONS =====================

INSERT INTO permissions (code, description) VALUES
('view_dashboard', 'Xem dashboard tổng quan'),
('view_report_overview', 'Xem báo cáo tổng quan thu chi'),
('view_report_ticket', 'Xem báo cáo doanh thu bán vé'),
('view_report_other', 'Xem báo cáo doanh thu dịch vụ khác'),
('view_report_expense', 'Xem báo cáo chi phí'),
('view_report_project', 'Xem báo cáo dự án bảo tồn'),
('edit_report', 'Nhập/sửa dữ liệu báo cáo thu chi'),
('manage_revenue', 'Quản lý doanh thu (thêm/sửa/xóa)'),
('manage_tickets', 'Quản lý bán vé'),
('manage_heritage_records', 'Quản lý sổ hồ sơ di sản'),
('manage_users', 'Quản lý người dùng và phân quyền');

-- ===================== GÁN QUYỀN CHO TỪNG ROLE =====================

-- admin: tất cả quyền
INSERT INTO role_permissions (role_id, permission_id)
SELECT 1, id FROM permissions;

-- accountant (kế toán): dashboard + toàn bộ báo cáo thu chi + được sửa + quản lý doanh thu
INSERT INTO role_permissions (role_id, permission_id)
SELECT 2, id FROM permissions
WHERE code IN (
  'view_dashboard','view_report_overview','view_report_ticket',
  'view_report_other','view_report_expense','view_report_project',
  'edit_report','manage_revenue'
);

-- staff: dashboard + bán vé + hồ sơ di sản + xem báo cáo bán vé
INSERT INTO role_permissions (role_id, permission_id)
SELECT 3, id FROM permissions
WHERE code IN (
  'view_dashboard','manage_tickets','manage_heritage_records','view_report_ticket'
);

-- user: chỉ xem dashboard
INSERT INTO role_permissions (role_id, permission_id)
SELECT 4, id FROM permissions
WHERE code IN ('view_dashboard');

-- ===================== DỮ LIỆU MẪU: BÁO CÁO THU CHI =====================

INSERT INTO ticket_revenue (month, year, adult_count, adult_revenue, child_count, child_revenue, priority_count, priority_revenue) VALUES
(3, 2025, 400, 40000000, 130, 6500000, 30, 1500000),
(4, 2025, 530, 53000000, 220, 11000000, 35, 1750000),
(5, 2025, 470, 47000000, 200, 10000000, 32, 1600000),
(6, 2025, 600, 60000000, 240, 12000000, 38, 1900000),
(7, 2025, 700, 70000000, 310, 15500000, 40, 2000000),
(8, 2025, 580, 58000000, 230, 11500000, 36, 1800000);

INSERT INTO other_revenue (month, year, category, amount) VALUES
(3, 2025, 'Dịch vụ tham quan', 60000000),
(4, 2025, 'Dịch vụ tham quan', 65000000),
(5, 2025, 'Dịch vụ tham quan', 62000000),
(6, 2025, 'Dịch vụ tham quan', 70000000),
(7, 2025, 'Dịch vụ tham quan', 75000000),
(8, 2025, 'Dịch vụ tham quan', 68000000);

INSERT INTO expenses (month, year, category, description, amount) VALUES
(3, 2025, 'Bảo tồn', 'Tu sửa Hiển Lâm Các', 25000000),
(4, 2025, 'Vận hành', 'Chi phí điện, nước, an ninh', 18000000),
(5, 2025, 'Bảo tồn', 'Bảo tồn Lăng Thiệu Trị', 30000000),
(6, 2025, 'Sự kiện', 'Lễ hội Áo Dài', 15000000),
(7, 2025, 'Vận hành', 'Chi phí điện, nước, an ninh', 19000000),
(8, 2025, 'Bảo tồn', 'Số hoá tài liệu Hán Nôm', 12000000);

INSERT INTO conservation_projects (name, status, budget, spent) VALUES
('Tu sửa Hiển Lâm Các', 'Đang thực hiện', 200000000, 80000000),
('Bảo tồn Lăng Thiệu Trị', 'Chờ duyệt', 350000000, 0),
('Phục dựng Điện Kiến Trung', 'Lên kế hoạch', 500000000, 0),
('Số hoá tài liệu Hán Nôm', 'Đang thực hiện', 100000000, 35000000);
