const express = require('express');
const router = express.Router();
const authService = require('../services/authService');

/**
 * POST /api/auth/login
 * Body: { username, password }
 * Đăng nhập và lưu thông tin user (kèm danh sách quyền) vào session.
 */
router.post('/login', async (req, res, next) => {
  try {
    const { username, password } = req.body;
    if (!username || !password) {
      return res.status(400).json({ message: 'Thiếu tên đăng nhập hoặc mật khẩu' });
    }

    const user = await authService.login(username, password);
    if (!user) {
      return res.status(401).json({ message: 'Sai tên đăng nhập hoặc mật khẩu' });
    }

    req.session.user = user;
    res.json({ user });
  } catch (err) {
    next(err);
  }
});

/**
 * POST /api/auth/logout
 * Xoá session hiện tại.
 */
router.post('/logout', (req, res) => {
  req.session.destroy(() => {
    res.json({ message: 'Đã đăng xuất' });
  });
});

/**
 * GET /api/auth/me
 * Trả về thông tin user đang đăng nhập (dùng để khôi phục trạng thái khi reload trang).
 */
router.get('/me', (req, res) => {
  if (!req.session.user) {
    return res.status(401).json({ message: 'Chưa đăng nhập' });
  }
  res.json({ user: req.session.user });
});

module.exports = router;
