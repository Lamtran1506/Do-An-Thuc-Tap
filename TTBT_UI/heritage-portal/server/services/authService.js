const bcrypt = require('bcrypt');
const pool = require('../config/db');

/**
 * Kiểm tra tên đăng nhập / mật khẩu, trả về thông tin user + danh sách quyền
 * (lấy từ role_permissions theo role của user).
 * Trả về null nếu sai tên đăng nhập hoặc mật khẩu.
 */
async function login(username, password) {
  const [users] = await pool.query(
    `SELECT u.id, u.username, u.password_hash, u.full_name,
            r.id AS role_id, r.name AS role_name
     FROM users u
     JOIN roles r ON u.role_id = r.id
     WHERE u.username = ?`,
    [username]
  );

  if (users.length === 0) return null;

  const user = users[0];
  const match = await bcrypt.compare(password, user.password_hash);
  if (!match) return null;

  const [permRows] = await pool.query(
    `SELECT p.code
     FROM role_permissions rp
     JOIN permissions p ON rp.permission_id = p.id
     WHERE rp.role_id = ?`,
    [user.role_id]
  );

  return {
    id: user.id,
    username: user.username,
    full_name: user.full_name,
    role: user.role_name,
    permissions: permRows.map((p) => p.code),
  };
}

module.exports = { login };
