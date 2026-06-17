/**
 * ============================================================
 * SERVICE LAYER - BÁO CÁO THU CHI
 * ============================================================
 * Nơi chứa logic nghiệp vụ (tính tổng, tổng hợp nhiều nguồn...).
 * Luôn gọi qua providers/reportProvider.js, KHÔNG query DB trực tiếp ở đây.
 * ============================================================
 */

const provider = require('../providers/reportProvider');

/** Báo cáo doanh thu bán vé + tổng cộng */
async function getTicketRevenueReport(year) {
  const data = await provider.getTicketRevenue(year);
  const total = data.reduce(
    (sum, row) =>
      sum +
      Number(row.adult_revenue) +
      Number(row.child_revenue) +
      Number(row.priority_revenue),
    0
  );
  return { year, data, total };
}

/** Báo cáo doanh thu dịch vụ khác + tổng cộng */
async function getOtherRevenueReport(year) {
  const data = await provider.getOtherRevenue(year);
  const total = data.reduce((sum, row) => sum + Number(row.amount), 0);
  return { year, data, total };
}

/** Báo cáo chi phí + tổng cộng */
async function getExpensesReport(year) {
  const data = await provider.getExpenses(year);
  const total = data.reduce((sum, row) => sum + Number(row.amount), 0);
  return { year, data, total };
}

/** Báo cáo tổng quan thu chi: tổng doanh thu, tổng chi phí, lợi nhuận */
async function getOverviewReport(year) {
  const [ticket, other, expense] = await Promise.all([
    getTicketRevenueReport(year),
    getOtherRevenueReport(year),
    getExpensesReport(year),
  ]);

  const totalRevenue = ticket.total + other.total;
  const totalExpense = expense.total;

  return {
    year,
    totalTicketRevenue: ticket.total,
    totalOtherRevenue: other.total,
    totalRevenue,
    totalExpense,
    profit: totalRevenue - totalExpense,
  };
}

/** Báo cáo các dự án bảo tồn */
async function getProjectsReport() {
  return provider.getConservationProjects();
}

module.exports = {
  getTicketRevenueReport,
  getOtherRevenueReport,
  getExpensesReport,
  getOverviewReport,
  getProjectsReport,
};
