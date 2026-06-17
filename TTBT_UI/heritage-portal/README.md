# Cổng thông tin điện tử - TT Bảo tồn Di tích Cố đô Huế

Web hoàn chỉnh: **Frontend HTML/CSS/JS thuần + Backend Node.js (Express) + MySQL**,
đăng nhập bằng **session**, phân quyền theo role: `admin`, `accountant` (kế toán),
`staff` (nhân viên), `user`.

## Cấu trúc thư mục

```
heritage-portal/
├── database/
│   ├── schema.sql                  # Tạo DB, bảng, dữ liệu mẫu, phân quyền
│   └── migration_manage_revenue.sql # Chỉ chạy nếu đã tạo DB từ bản cũ
├── server/                          # Backend Node/Express (API)
│   ├── config/db.js
│   ├── middleware/auth.js
│   ├── providers/reportProvider.js  # Lớp dữ liệu - SẼ THAY API CÔNG TY SAU
│   ├── services/
│   ├── routes/                      # auth.js, reports.js
│   ├── app.js, seed.js
│   └── package.json, .env.example
└── web/                              # Frontend HTML/CSS/JS thuần
    ├── css/styles.css                # CSS dùng chung cho mọi trang
    ├── js/auth.js                     # Đăng nhập / đăng xuất / session
    ├── js/layout.js                   # Render topbar + tabs theo phân quyền
    ├── login.html
    ├── index.html                     # Dashboard
    ├── revenue.html                   # Quản lý doanh thu
    ├── tickets.html                   # Quản lý vé
    ├── heritage.html                  # Sổ hồ sơ di sản (camera, gridstack)
    ├── report-overview.html           # Báo cáo tổng > Tổng quan thu chi
    ├── report-tickets.html            # Báo cáo tổng > Doanh thu bán vé
    ├── report-other.html              # Báo cáo tổng > Doanh thu dịch vụ khác
    ├── report-expenses.html           # Báo cáo tổng > Chi phí bảo tồn/vận hành
    ├── report-projects.html           # Báo cáo tổng > Dự án bảo tồn
    └── package.json
```

## 1. Cài đặt database

Mở MySQL (Workbench / phpMyAdmin / CLI) và chạy:

- Nếu **tạo mới** database: chạy toàn bộ `database/schema.sql`
- Nếu **đã từng** chạy schema.sql phiên bản trước (chưa có menu "Quản lý doanh thu"):
  chạy thêm `database/migration_manage_revenue.sql`

`schema.sql` sẽ tạo 4 role (`admin`, `accountant`, `staff`, `user`), các bảng dữ liệu
báo cáo (`ticket_revenue`, `other_revenue`, `expenses`, `conservation_projects`),
dữ liệu mẫu năm 2025 (theo dashboard bạn đã làm), và phân quyền sẵn cho từng role
(xem bảng ở mục 5).

## 2. Chạy backend (API)

```bash
cd server
npm install
cp .env.example .env
```

Sửa `.env`: điền `DB_USER`, `DB_PASSWORD` cho đúng MySQL của bạn.

Tạo 4 tài khoản demo:

```bash
npm run seed
```

| Tài khoản | Mật khẩu    | Role                |
|-----------|-------------|---------------------|
| admin     | admin123    | admin               |
| ketoan    | ketoan123   | accountant (Kế toán)|
| nhanvien  | nhanvien123 | staff               |
| user      | user123     | user                |

Chạy server:

```bash
npm run dev
```

Backend chạy tại `http://localhost:5000`. Kiểm tra: `http://localhost:5000/api/health`.

## 3. Chạy frontend (web)

Vì frontend dùng `fetch()` gọi API kèm cookie session, bạn cần chạy nó qua
**1 web server tĩnh** (không mở trực tiếp file `index.html` bằng cách double-click,
vì cookie sẽ không hoạt động đúng với địa chỉ `file://`).

```bash
cd web
npm install
npm start
```

Mặc định chạy tại `http://localhost:3000` - đúng với `CLIENT_URL` mà backend
đang cho phép (CORS) trong `.env`.

Mở trình duyệt: `http://localhost:3000` → sẽ tự chuyển sang `login.html`
nếu chưa đăng nhập.

## 4. Luồng hoạt động

- `js/auth.js`: chứa các hàm gọi API đăng nhập/đăng xuất, kiểm tra session (`/api/auth/me`),
  và `apiFetch()` dùng chung cho các trang báo cáo.
- `js/layout.js`: mỗi trang gọi `renderLayout("<tên trang>")` để vẽ topbar + tabs.
  Menu chỉ hiển thị các mục mà user đang đăng nhập **có quyền** truy cập
  (dựa vào `user.permissions` trả về từ `/api/auth/me`).
- Nếu chưa đăng nhập, mọi trang (trừ `login.html`) sẽ tự chuyển về `login.html`.
- Nếu phiên đăng nhập hết hạn, `apiFetch()` sẽ tự chuyển về `login.html`.

## 5. Bảng phân quyền (role ↔ permission ↔ trang)

| Trang                                       | Permission yêu cầu        | admin | accountant | staff | user |
|---------------------------------------------|----------------------------|:---:|:---:|:---:|:---:|
| Dashboard                                     | `view_dashboard`             | ✅ | ✅ | ✅ | ✅ |
| Quản lý doanh thu                              | `manage_revenue`               | ✅ | ✅ | ❌ | ❌ |
| Quản lý vé                                     | `manage_tickets`                | ✅ | ❌ | ✅ | ❌ |
| Sổ hồ sơ di sản                                 | `manage_heritage_records`        | ✅ | ❌ | ✅ | ❌ |
| Báo cáo tổng > Tổng quan thu chi                  | `view_report_overview`            | ✅ | ✅ | ❌ | ❌ |
| Báo cáo tổng > Doanh thu bán vé                    | `view_report_ticket`               | ✅ | ✅ | ✅ | ❌ |
| Báo cáo tổng > Doanh thu dịch vụ khác                | `view_report_other`                 | ✅ | ✅ | ❌ | ❌ |
| Báo cáo tổng > Chi phí bảo tồn / vận hành              | `view_report_expense`                | ✅ | ✅ | ❌ | ❌ |
| Báo cáo tổng > Báo cáo dự án bảo tồn                    | `view_report_project`                 | ✅ | ✅ | ❌ | ❌ |

(accountant = Kế toán)

Muốn đổi quyền cho 1 role: chỉ cần sửa dữ liệu trong bảng `role_permissions`,
không cần sửa code. Ví dụ cho Staff xem thêm "Doanh thu dịch vụ khác":

```sql
INSERT INTO role_permissions (role_id, permission_id)
SELECT 3, id FROM permissions WHERE code = 'view_report_other';
```

(role_id: 1 = admin, 2 = accountant, 3 = staff, 4 = user)

## 6. Khi tích hợp API công ty sau này

Chỉ sửa `server/providers/reportProvider.js`:
- Giữ nguyên tên hàm và cấu trúc dữ liệu trả về.
- Thay phần query MySQL bằng gọi API công ty, map dữ liệu về đúng cấu trúc cũ.

→ `services/`, `routes/`, và toàn bộ `web/` KHÔNG cần sửa gì.

## 7. Các bước tiếp theo có thể làm

- Thêm form "Thêm/Sửa/Xóa" cho `revenue.html`, `tickets.html` (hiện tại bảng
  còn dữ liệu mẫu, icon thao tác chưa gắn chức năng).
- Nối dữ liệu KPI/chart ở `index.html` với API thật (hiện đang để số liệu mẫu
  giống thiết kế gốc).
- Trang quản lý người dùng (chỉ Admin) để thêm/sửa tài khoản và đổi role.
- Thêm permission `edit_report` vào UI cho Kế toán (nút "Nhập số liệu" trên
  các trang báo cáo).
