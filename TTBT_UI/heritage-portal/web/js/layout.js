/* ============================================================
   layout.js - Render topbar + tabs dùng chung cho mọi trang,
   tự ẩn/hiện menu theo quyền (permissions) của user đăng nhập.
   ============================================================ */

// Menu chính (các tab ngoài cùng)
const MENU_ITEMS = [
  { key: "dashboard", label: "Dashboard", path: "index.html", permission: "view_dashboard" },
  { key: "revenue", label: "Quản lý doanh thu", path: "revenue.html", permission: "manage_revenue" },
  { key: "tickets", label: "Quản lý vé", path: "tickets.html", permission: "manage_tickets" },
  { key: "heritage", label: "Sổ hồ sơ di sản", path: "heritage.html", permission: "manage_heritage_records" },
];

// Menu con của "Báo cáo tổng"
const REPORT_SUBMENU = [
  { key: "report-overview", label: "Tổng quan thu chi", path: "report-overview.html", permission: "view_report_overview" },
  { key: "report-tickets", label: "Doanh thu bán vé", path: "report-tickets.html", permission: "view_report_ticket" },
  { key: "report-other", label: "Doanh thu dịch vụ khác", path: "report-other.html", permission: "view_report_other" },
  { key: "report-expenses", label: "Chi phí bảo tồn / vận hành", path: "report-expenses.html", permission: "view_report_expense" },
  { key: "report-projects", label: "Báo cáo dự án bảo tồn", path: "report-projects.html", permission: "view_report_project" },
];

// Tên hiển thị cho từng role
const ROLE_LABELS = {
  admin: "Admin",
  accountant: "Kế toán",
  staff: "Nhân viên",
  user: "Người dùng",
};

/**
 * Render topbar + tabs vào #app-header, dựa theo user đang đăng nhập
 * và đánh dấu tab/menu con đang active theo activeKey.
 *
 * Trả về user (đã đăng nhập), hoặc null nếu chưa đăng nhập (đã redirect).
 */
async function renderLayout(activeKey) {
  const user = await requireLogin();
  if (!user) return null;

  const hasPermission = (code) => (user.permissions || []).includes(code);

  // ----- Tabs chính -----
  const tabsHtml = MENU_ITEMS.filter((item) => hasPermission(item.permission))
    .map((item) => {
      const activeCls = item.key === activeKey ? " active" : "";
      return `<a href="${item.path}" class="tab${activeCls}">${item.label}</a>`;
    })
    .join("");

  // ----- Menu con "Báo cáo tổng" -----
  const visibleReportItems = REPORT_SUBMENU.filter((item) => hasPermission(item.permission));
  let reportGroupHtml = "";
  if (visibleReportItems.length > 0) {
    const isReportActive = visibleReportItems.some((item) => item.key === activeKey);
    const submenuHtml = visibleReportItems
      .map((item) => {
        const activeCls = item.key === activeKey ? " active" : "";
        return `<a href="${item.path}" class="${activeCls.trim()}">${item.label}</a>`;
      })
      .join("");

    reportGroupHtml = `
      <div class="tab-group">
        <span class="tab${isReportActive ? " active" : ""}">
          Báo cáo tổng <i class="ti ti-chevron-down" style="font-size: 12px"></i>
        </span>
        <div class="tab-submenu">${submenuHtml}</div>
      </div>
    `;
  }

  // ----- Topbar -----
  const roleLabel = ROLE_LABELS[user.role] || user.role;

  const headerHtml = `
    <div class="topbar">
      <div class="topbar-left">
        <div class="topbar-logo"><i class="ti ti-building-arch"></i></div>
        <div>
          <div class="topbar-name">CỔNG THÔNG TIN ĐIỆN TỬ</div>
          <div class="topbar-sub">TRUNG TÂM BẢO TỒN DI TÍCH CỐ ĐÔ HUẾ</div>
        </div>
      </div>
      <div class="topbar-right">
        <i class="ti ti-bell topbar-bell"></i>
        <div class="admin-wrap">
          <div class="admin-btn">
            <i class="ti ti-user-circle"></i>
            <span>${user.full_name || user.username} (${roleLabel})</span>
            <i class="ti ti-chevron-down" style="font-size: 13px; opacity: 0.6"></i>
          </div>
          <div class="dropdown">
            <div class="dropdown-item" id="logout-btn">
              <i class="ti ti-logout"></i> Đăng xuất
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="tabs">
      ${tabsHtml}
      ${reportGroupHtml}
    </div>
  `;

  const headerEl = document.getElementById("app-header");
  headerEl.innerHTML = headerHtml;

  document.getElementById("logout-btn").addEventListener("click", () => {
    logoutRequest();
  });

  return user;
}

/**
 * Định dạng số tiền theo kiểu Việt Nam: 500.000.000 đồng
 */
function formatCurrency(value) {
  return new Intl.NumberFormat("vi-VN").format(value || 0) + " đồng";
}

/**
 * Hiển thị thông báo "không có quyền" trong vùng nội dung báo cáo
 * và dừng việc tải dữ liệu của trang báo cáo.
 */
function showAccessDenied(containerId) {
  document.getElementById(containerId).innerHTML = `
    <div class="access-denied">
      <i class="ti ti-lock"></i>
      Bạn không có quyền xem báo cáo này. Vui lòng liên hệ quản trị viên nếu cần.
    </div>
  `;
}
