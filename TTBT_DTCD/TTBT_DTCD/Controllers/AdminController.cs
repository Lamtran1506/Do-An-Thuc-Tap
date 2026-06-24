using Microsoft.AspNetCore.Mvc;
using TTBT_DTCD.Data;
using System.Linq;

namespace TTBT_DTCD.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Users()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToAction("Index", "Home");

            var list = _context.Users.ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult ChangeRole(int id, string newRole)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToAction("Index", "Home");

            if (newRole == "Admin")
            {
                TempData["Error"] = "Hệ thống từ chối! Không được phép cấp thêm quyền Admin.";
                return RedirectToAction("Users");
            }

            var user = _context.Users.Find(id);
            if (user != null)
            {
                if (user.Username.ToLower() == "admin" || user.Role == "Admin")
                {
                    TempData["Error"] = "Không thể thay đổi quyền của tài khoản Quản trị viên!";
                    return RedirectToAction("Users");
                }

                user.Role = newRole;
                _context.Update(user);
                _context.SaveChanges();
                TempData["Message"] = $"Đã cập nhật quyền của tài khoản {user.Username} thành {newRole}!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy tài khoản này trong hệ thống.";
            }

            return RedirectToAction("Users");
        }
    }
}
