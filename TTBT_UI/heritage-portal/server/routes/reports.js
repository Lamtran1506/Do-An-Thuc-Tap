const express = require('express');
const router = express.Router();
const { requireAuth, requirePermission } = require('../middleware/auth');
const reportService = require('../services/reportService');

/**
 * Tất cả các route dưới đây đều yêu cầu đăng nhập (requireAuth)
 * và quyền tương ứng (requirePermission). Quyền được gán theo role
 * trong bảng role_permissions (xem database/schema.sql).
 *
 * Mặc định trả về dữ liệu của năm hiện tại nếu không truyền ?year=
 */

// GET /api/reports/overview - Tổng quan thu chi
router.get(
  '/overview',
  requireAuth,
  requirePermission('view_report_overview'),
  async (req, res, next) => {
    try {
      const year = Number(req.query.year) || new Date().getFullYear();
      const data = await reportService.getOverviewReport(year);
      res.json(data);
    } catch (err) {
      next(err);
    }
  }
);

// GET /api/reports/ticket-revenue - Doanh thu bán vé
router.get(
  '/ticket-revenue',
  requireAuth,
  requirePermission('view_report_ticket'),
  async (req, res, next) => {
    try {
      const year = Number(req.query.year) || new Date().getFullYear();
      const data = await reportService.getTicketRevenueReport(year);
      res.json(data);
    } catch (err) {
      next(err);
    }
  }
);

// GET /api/reports/other-revenue - Doanh thu dịch vụ khác
router.get(
  '/other-revenue',
  requireAuth,
  requirePermission('view_report_other'),
  async (req, res, next) => {
    try {
      const year = Number(req.query.year) || new Date().getFullYear();
      const data = await reportService.getOtherRevenueReport(year);
      res.json(data);
    } catch (err) {
      next(err);
    }
  }
);

// GET /api/reports/expenses - Chi phí
router.get(
  '/expenses',
  requireAuth,
  requirePermission('view_report_expense'),
  async (req, res, next) => {
    try {
      const year = Number(req.query.year) || new Date().getFullYear();
      const data = await reportService.getExpensesReport(year);
      res.json(data);
    } catch (err) {
      next(err);
    }
  }
);

// GET /api/reports/projects - Báo cáo dự án bảo tồn
router.get(
  '/projects',
  requireAuth,
  requirePermission('view_report_project'),
  async (req, res, next) => {
    try {
      const data = await reportService.getProjectsReport();
      res.json(data);
    } catch (err) {
      next(err);
    }
  }
);

module.exports = router;
