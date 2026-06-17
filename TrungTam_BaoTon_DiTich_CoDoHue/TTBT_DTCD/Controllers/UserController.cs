using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TTBT_DTCD.Data;
using TTBT_DTCD.Models;
using System.Linq;

namespace TTBT_DTCD.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Giao diện Đăng ký
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Username đã tồn tại chưa để tránh lỗi trùng lặp
                var existingUser = _context.Users.Any(u => u.Username == user.Username);
                if (existingUser)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                    return View(user);
                }

                // Gán vai trò mặc định là khách hàng (phân quyền mặc định)
                user.Role = "Customer";

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Message"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // 2. Giao diện Đăng nhập
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Lưu thông tin đăng nhập & phân quyền vào Session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("FullName", user.FullName ?? user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetInt32("UserId", user.UserId);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        // 3. Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
