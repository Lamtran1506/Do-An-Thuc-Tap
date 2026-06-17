/**
 * ============================================================
 * DATA PROVIDER LAYER - BÁO CÁO THU CHI
 * ============================================================
 * Đây là lớp DUY NHẤT chạm vào nguồn dữ liệu thật.
 * Hiện tại: lấy dữ liệu từ MySQL (mô phỏng).
 * Sau này: thay nội dung từng hàm bằng việc gọi API của công ty,
 * miễn là giữ NGUYÊN tên hàm và CẤU TRÚC dữ liệu trả về (mảng object
 * với các field như dưới đây). Như vậy reportService.js và toàn bộ
 * phía trên (routes, frontend) sẽ KHÔNG cần sửa gì.
 * ============================================================
 */

const pool = require('../config/db');

/**
 * Doanh thu bán vé theo năm, trả về theo từng tháng.
 * @returns {Promise<Array<{month, adult_count, adult_revenue, child_count, child_revenue, priority_count, priority_revenue}>>}
 */
async function getTicketRevenue(year) {
  const [rows] = await pool.query(
    `SELECT month, adult_count, adult_revenue, child_count, child_revenue,
            priority_count, priority_revenue
     FROM ticket_revenue
     WHERE year = ?
     ORDER BY month`,
    [year]
  );
  return rows;

  // ---- Khi chuyển sang API công ty, có thể thay bằng: ----
  // const res = await fetch(`${COMPANY_API_URL}/ticket-revenue?year=${year}`, {
  //   headers: { Authorization: `Bearer ${COMPANY_API_KEY}` },
  // });
  // const data = await res.json();
  // return data.map(item => ({ month: item.month, adult_count: item.nguoiLon, ... })); // map field
}

/**
 * Doanh thu dịch vụ khác theo năm.
 * @returns {Promise<Array<{month, category, amount}>>}
 */
async function getOtherRevenue(year) {
  const [rows] = await pool.query(
    `SELECT month, category, amount
     FROM other_revenue
     WHERE year = ?
     ORDER BY month`,
    [year]
  );
  return rows;
}

/**
 * Chi phí (bảo tồn, vận hành, sự kiện...) theo năm.
 * @returns {Promise<Array<{month, category, description, amount}>>}
 */
async function getExpenses(year) {
  const [rows] = await pool.query(
    `SELECT month, category, description, amount
     FROM expenses
     WHERE year = ?
     ORDER BY month`,
    [year]
  );
  return rows;
}

/**
 * Danh sách dự án bảo tồn kèm ngân sách / đã chi.
 * @returns {Promise<Array<{id, name, status, budget, spent}>>}
 */
async function getConservationProjects() {
  const [rows] = await pool.query(
    `SELECT id, name, status, budget, spent FROM conservation_projects`
  );
  return rows;
}

module.exports = {
  getTicketRevenue,
  getOtherRevenue,
  getExpenses,
  getConservationProjects,
};
