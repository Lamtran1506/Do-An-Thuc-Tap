/**
 * Script tạo tài khoản mẫu cho từng role.
 * Chạy: npm run seed (sau khi đã chạy database/schema.sql)
 *
 * Tài khoản tạo ra:
 *  - admin    / admin123     (role: admin)
 *  - ketoan   / ketoan123    (role: accountant - kế toán)
 *  - nhanvien / nhanvien123  (role: staff)
 *  - user     / user123      (role: user)
 *
 * LƯU Ý: Đây là tài khoản DEMO, khi đưa lên môi trường thật
 * hãy đổi mật khẩu / xoá các tài khoản mẫu này.
 */

const bcrypt = require('bcrypt');
const pool = require('./config/db');

const users = [
  { username: 'admin', password: 'admin123', full_name: 'Quản trị viên', role: 'admin' },
  { username: 'ketoan', password: 'ketoan123', full_name: 'Nhân viên Kế toán', role: 'accountant' },
  { username: 'nhanvien', password: 'nhanvien123', full_name: 'Nhân viên', role: 'staff' },
  { username: 'user', password: 'user123', full_name: 'Người dùng', role: 'user' },
];

async function seed() {
  for (const u of users) {
    const [roleRows] = await pool.query('SELECT id FROM roles WHERE name = ?', [u.role]);
    if (roleRows.length === 0) {
      console.log(`Không tìm thấy role "${u.role}", bỏ qua user "${u.username}"`);
      continue;
    }

    const hash = await bcrypt.hash(u.password, 10);

    await pool.query(
      `INSERT INTO users (username, password_hash, full_name, role_id)
       VALUES (?, ?, ?, ?)
       ON DUPLICATE KEY UPDATE password_hash = ?, full_name = ?, role_id = ?`,
      [u.username, hash, u.full_name, roleRows[0].id, hash, u.full_name, roleRows[0].id]
    );

    console.log(`Đã tạo/cập nhật user: ${u.username} (role: ${u.role}, pass: ${u.password})`);
  }

  process.exit(0);
}

seed().catch((err) => {
  console.error('Lỗi khi tạo seed data:', err);
  process.exit(1);
});
