/* ============================================================
   auth.js - Xử lý đăng nhập / đăng xuất / kiểm tra phiên làm việc
   Dùng chung cho toàn bộ các trang.
   ============================================================ */

// Đổi địa chỉ này khi deploy backend thật
const API_URL = "http://localhost:5000/api";

/**
 * Lấy thông tin user đang đăng nhập (qua session/cookie).
 * Trả về null nếu chưa đăng nhập.
 */
async function getCurrentUser() {
  try {
    const res = await fetch(`${API_URL}/auth/me`, { credentials: "include" });
    if (!res.ok) return null;
    const data = await res.json();
    return data.user;
  } catch (err) {
    console.error("Không thể kết nối tới server:", err);
    return null;
  }
}

/**
 * Dùng ở đầu mỗi trang (trừ login.html): nếu chưa đăng nhập thì
 * chuyển về trang đăng nhập. Trả về thông tin user nếu đã đăng nhập.
 */
async function requireLogin() {
  const user = await getCurrentUser();
  if (!user) {
    window.location.href = "login.html";
    return null;
  }
  return user;
}

/**
 * Gửi yêu cầu đăng nhập. Ném lỗi nếu sai tên đăng nhập/mật khẩu.
 */
async function loginRequest(username, password) {
  const res = await fetch(`${API_URL}/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify({ username, password }),
  });

  if (!res.ok) {
    const err = await res.json();
    throw new Error(err.message || "Đăng nhập thất bại");
  }

  const data = await res.json();
  return data.user;
}

/**
 * Đăng xuất và chuyển về trang đăng nhập.
 */
async function logoutRequest() {
  try {
    await fetch(`${API_URL}/auth/logout`, {
      method: "POST",
      credentials: "include",
    });
  } finally {
    window.location.href = "login.html";
  }
}

/**
 * Gọi API có kèm session cookie. Dùng cho các trang lấy dữ liệu báo cáo...
 * Nếu API trả về 401 (hết phiên), tự động chuyển về trang đăng nhập.
 */
async function apiFetch(path, options = {}) {
  const res = await fetch(`${API_URL}${path}`, {
    credentials: "include",
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (res.status === 401) {
    window.location.href = "login.html";
    throw new Error("Phiên đăng nhập đã hết hạn");
  }

  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error(err.message || `Lỗi gọi API: ${res.status}`);
  }

  return res.json();
}
