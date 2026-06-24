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

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {

            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {

                var existingUser = _context.Users.Any(u => u.Username.ToLower() == user.Username.ToLower());
                if (existingUser)
                {
                    ViewBag.Error = "Tên đăng nhập này đã tồn tại trên hệ thống!";
                    return View(user);
                }

                var existingEmail = _context.Users.Any(u => u.Email.ToLower() == user.Email.ToLower());
                if (existingEmail)
                {
                    ViewBag.Error = "Địa chỉ Email này đã được đăng ký tài khoản khác!";
                    return View(user);
                }

                user.Role = "Customer";

                _context.Users.Add(user);
                _context.SaveChanges(); 

                TempData["Message"] = "🎉 Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Thông tin nhập vào không hợp lệ, vui lòng kiểm tra kỹ lại!";
            return View(user);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tên tài khoản và mật khẩu!";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {

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

            ViewBag.Error = "❌ Tên đăng nhập hoặc mật khẩu không chính xác!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }
    }
}