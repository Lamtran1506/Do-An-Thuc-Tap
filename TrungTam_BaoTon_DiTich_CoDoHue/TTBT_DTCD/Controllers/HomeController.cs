using Microsoft.AspNetCore.Mvc;
using TTBT_DTCD.Models;
using System.Diagnostics;

namespace TTBT_DTCD.Controllers
{
    public class HomeController : Controller
    {
        // 1. Dashboard - Trang chủ chính sau khi đăng nhập thành công
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        // 2. Báo cáo tổng hợp - Xử lý định tuyến xem biểu đồ và chênh lệch thống kê
        public IActionResult Report()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        // 3. Quản lý doanh thu - Xử lý định tuyến xem tỷ trọng và bảng lọc tài chính
        public IActionResult Revenue()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        // 4. Quản lý vé - Xử lý định tuyến quản lý danh sách vé tham quan di tích
        public IActionResult Ticket()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        // Action xử lý định tuyến cho trang Sổ hồ sơ di sản
        public IActionResult Heritage()
        {
            // Kiểm tra bảo mật Session đăng nhập
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}