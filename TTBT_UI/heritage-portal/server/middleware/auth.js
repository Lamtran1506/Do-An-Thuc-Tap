/**
 * Middleware kiểm tra người dùng đã đăng nhập (có session) hay chưa.
 */
function requireAuth(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ message: 'Bạn cần đăng nhập để thực hiện thao tác này' });
  }
  next();
}

/**
 * Middleware kiểm tra người dùng có quyền (permission code) cụ thể hay không.
 * Quyền được nạp vào session.user.permissions khi đăng nhập (xem authService.js).
 *
 * Cách dùng: router.get('/path', requireAuth, requirePermission('view_report_overview'), handler)
 */
function requirePermission(permissionCode) {
  return (req, res, next) => {
    if (!req.session.user) {
      return res.status(401).json({ message: 'Bạn cần đăng nhập để thực hiện thao tác này' });
    }

    const permissions = req.session.user.permissions || [];
    if (!permissions.includes(permissionCode)) {
      return res.status(403).json({ message: 'Bạn không có quyền truy cập chức năng này' });
    }

    next();
  };
}

module.exports = { requireAuth, requirePermission };
