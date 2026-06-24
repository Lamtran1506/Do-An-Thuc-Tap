using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTBT_DTCD.Data;
using TTBT_DTCD.Models;
using System.Diagnostics;

namespace TTBT_DTCD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        bool IsAuth() => !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
        bool IsAdminOrStaff() { var r = HttpContext.Session.GetString("Role"); return r == "Admin" || r == "Staff"; }

        public IActionResult Index()
        {
            if (!IsAuth()) return RedirectToAction("Login", "User");
            return View();
        }

        public IActionResult ChinhSuaDanhMuc()
        {
            if (!IsAuth()) return RedirectToAction("Login", "User");
            if (!IsAdminOrStaff()) return RedirectToAction("Index");
            return View();
        }

        public async Task<IActionResult> Revenue()
        {
            if (!IsAuth()) return RedirectToAction("Login", "User");
            if (!IsAdminOrStaff()) return RedirectToAction("Index");
            var list = await _db.DoanhThus.OrderByDescending(x => x.Ngay).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Ticket()
        {
            if (!IsAuth()) return RedirectToAction("Login", "User");
            if (!IsAdminOrStaff()) return RedirectToAction("Index");
            var list = await _db.Ves.OrderByDescending(x => x.NgayBan).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Heritage()
        {
            if (!IsAuth()) return RedirectToAction("Login", "User");
            if (!IsAdminOrStaff()) return RedirectToAction("Index");
            var list = await _db.HoSoDiSans.OrderBy(x => x.MaHoSo).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Report()
        {
            if (!IsAuth()) return RedirectToAction("Login", "User");
            if (!IsAdminOrStaff()) return RedirectToAction("Index");
            var list = await _db.BaoCaos.OrderByDescending(x => x.Ngay).ToListAsync();
            return View(list);
        }

        public IActionResult CreateDoanhThu()
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            return View(new DoanhThu { Ngay = DateTime.Today });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoanhThu(DoanhThu m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.DoanhThus.Add(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã thêm doanh thu thành công!";
            return RedirectToAction("Revenue");
        }

        public async Task<IActionResult> EditDoanhThu(int id)
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            var item = await _db.DoanhThus.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditDoanhThu(DoanhThu m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.DoanhThus.Update(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã cập nhật doanh thu!";
            return RedirectToAction("Revenue");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDoanhThu(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") { TempData["Error"] = "Chỉ Admin mới có quyền xóa!"; return RedirectToAction("Revenue"); }
            var item = await _db.DoanhThus.FindAsync(id);
            if (item != null) { _db.DoanhThus.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Message"] = "Đã xóa!";
            return RedirectToAction("Revenue");
        }

        public IActionResult CreateVe()
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            var nextId = (_db.Ves.Any() ? _db.Ves.Max(v => v.Id) : 0) + 1;
            return View(new Ve { NgayBan = DateTime.Now, MaVe = $"VE-{DateTime.Now.Year}-{nextId:D3}" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateVe(Ve m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Ves.Add(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã thêm vé thành công!";
            return RedirectToAction("Ticket");
        }

        public async Task<IActionResult> EditVe(int id)
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            var item = await _db.Ves.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditVe(Ve m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Ves.Update(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã cập nhật vé!";
            return RedirectToAction("Ticket");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVe(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") { TempData["Error"] = "Chỉ Admin mới có quyền xóa!"; return RedirectToAction("Ticket"); }
            var item = await _db.Ves.FindAsync(id);
            if (item != null) { _db.Ves.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Message"] = "Đã xóa vé!";
            return RedirectToAction("Ticket");
        }

        public IActionResult CreateHoSo()
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            return View(new HoSoDiSan());
        }

        [HttpPost]
        public async Task<IActionResult> CreateHoSo(HoSoDiSan m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.HoSoDiSans.Add(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã thêm hồ sơ di sản!";
            return RedirectToAction("Heritage");
        }

        public async Task<IActionResult> EditHoSo(int id)
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            var item = await _db.HoSoDiSans.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditHoSo(HoSoDiSan m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.HoSoDiSans.Update(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã cập nhật hồ sơ!";
            return RedirectToAction("Heritage");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHoSo(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") { TempData["Error"] = "Chỉ Admin mới có quyền xóa!"; return RedirectToAction("Heritage"); }
            var item = await _db.HoSoDiSans.FindAsync(id);
            if (item != null) { _db.HoSoDiSans.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Message"] = "Đã xóa hồ sơ!";
            return RedirectToAction("Heritage");
        }

        public IActionResult CreateBaoCao()
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            return View(new BaoCao { Ngay = DateTime.Today });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBaoCao(BaoCao m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.BaoCaos.Add(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã thêm báo cáo!";
            return RedirectToAction("Report");
        }

        public async Task<IActionResult> EditBaoCao(int id)
        {
            if (!IsAuth() || !IsAdminOrStaff()) return RedirectToAction("Index");
            var item = await _db.BaoCaos.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditBaoCao(BaoCao m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.BaoCaos.Update(m); await _db.SaveChangesAsync();
            TempData["Message"] = "Đã cập nhật báo cáo!";
            return RedirectToAction("Report");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBaoCao(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") { TempData["Error"] = "Chỉ Admin mới có quyền xóa!"; return RedirectToAction("Report"); }
            var item = await _db.BaoCaos.FindAsync(id);
            if (item != null) { _db.BaoCaos.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Message"] = "Đã xóa báo cáo!";
            return RedirectToAction("Report");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}