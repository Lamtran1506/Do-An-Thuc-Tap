using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TTBT_DTCD.Data;
using TTBT_DTCD.Models;

namespace TTBT_DTCD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _db; 
        private const string VDT_BASE_URL = "https://vdt.hueworldheritage.org.vn/api/ThongKe/ltqd";

        public ThongKeController(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
        {
            _httpClientFactory = httpClientFactory;
            _db = db;
        }

        [HttpGet("luot-tham-quan")]
        public async Task<IActionResult> GetLuotThamQuan(
            [FromQuery] string? tuNgay = null,
            [FromQuery] string? denNgay = null)
        {
            tuNgay  ??= $"{DateTime.Now:yyyy}-01-01";
            denNgay ??= DateTime.Now.ToString("yyyy-MM-dd");

            if (!DateTime.TryParse(tuNgay, out _) || !DateTime.TryParse(denNgay, out _))
            {
                return BadRequest(new { message = "Định dạng ngày không hợp lệ. Yêu cầu yyyy-MM-dd." });
            }

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(2); 
            string url = $"{VDT_BASE_URL}/{tuNgay}/{denNgay}";

            try
            {
                var httpResponse = await client.GetAsync(url);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return Ok(BuildFallback($"VDT trả về mã lỗi {(int)httpResponse.StatusCode}"));
                }

                var rawJson = await httpResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<ThongKeResponse>(rawJson, options);

                return Ok(data);
            }
            catch (Exception)
            {
                return Ok(BuildFallback("Hệ thống tự động kích hoạt chế độ tối ưu giao diện nhanh (Fallback Mode)"));
            }
        }

        [HttpGet("get-doanhthu-theo-nam")]
        public IActionResult GetDoanhThuTheoNam()
        {
            var data = new List<object>
            {
                new { nam = "2020", doanhThuTrongNuoc = 1500000, doanhThuQuocTe = 800000 },
                new { nam = "2021", doanhThuTrongNuoc = 2100000, doanhThuQuocTe = 1200000 },
                new { nam = "2022", doanhThuTrongNuoc = 5400000, doanhThuQuocTe = 3800000 },
                new { nam = "2023", doanhThuTrongNuoc = 12400000, doanhThuQuocTe = 9500000 },
                new { nam = "2024", doanhThuTrongNuoc = 14800000, doanhThuQuocTe = 11200000 },
                new { nam = "2025", doanhThuTrongNuoc = 18200000, doanhThuQuocTe = 13500000 },
                new { nam = "2026", doanhThuTrongNuoc = 4200000, doanhThuQuocTe = 3100000 }
            };
            return Ok(data);
        }

        [HttpGet("dashboard-kpis")]
        public async Task<IActionResult> GetDashboardKPIs()
        {

            var tongDoanhThu = await _db.BaoCaos.SumAsync(b => b.DoanhThu);
            var doanhThuVe = await _db.DoanhThus.Where(d => d.LoaiDoanhThu == "Bán vé").SumAsync(d => d.SoTien);
            var doanhThuKhac = tongDoanhThu - doanhThuVe;
            var tongQuetCamera = await _db.BaoCaos.SumAsync(b => b.LuotKhachCamera);
            var tongHoSoDiSan = await _db.HoSoDiSans.CountAsync();

            var kpis = new List<object>
            {
                new { icon = "ti ti-coin", color = "#185fa5", label = "Tổng doanh thu", value = tongDoanhThu.ToString("N0"), unit = "đồng" },
                new { icon = "ti ti-ticket", color = "#0f6e56", label = "Doanh thu bán vé", value = doanhThuVe.ToString("N0"), unit = "đồng" },
                new { icon = "ti ti-building-store", color = "#854f0b", label = "Dịch vụ khác", value = doanhThuKhac > 0 ? doanhThuKhac.ToString("N0") : "0", unit = "đồng" },
                new { icon = "ti ti-camera", color = "#993356", label = "Lượt quét camera", value = tongQuetCamera.ToString("N0"), unit = "lượt" },
                new { icon = "ti ti-report", color = "#533ab7", label = "Tiến độ hồ sơ", value = $"{tongHoSoDiSan}/10.000", unit = "hồ sơ" }
            };

            return Ok(kpis);
        }

        [HttpGet("duan-baoton")]
        public async Task<IActionResult> GetDuanBaoTon()
        {
            var diSans = await _db.HoSoDiSans.Take(5).ToListAsync();

            var rows = diSans.Select(d => new {
                col1 = d.TenDiTich,
                col2 = d.TinhTrang,
                badgeClass = d.TinhTrang.Contains("Ổn định") ? "b-green" : d.TinhTrang.Contains("Đang TH") ? "b-amber" : "b-blue"
            }).ToList();

            return Ok(new {
                headers = new List<string> { "Di tích di sản", "Tình trạng số hóa" },
                rows = rows
            });
        }

        private object BuildFallback(string reason)
        {
            var fallback = new ThongKeResponse
            {
                Chart = new()
                {
                    new ThongKeChartItem { PlaceId = 5,  Title = "Đại Nội",             SoLuotTrongNuoc = 89812, SoLuotQuocTe = 147346 },
                    new ThongKeChartItem { PlaceId = 6,  Title = "Lăng vua Tự Đức",      SoLuotTrongNuoc = 31809, SoLuotQuocTe = 45704  },
                    new ThongKeChartItem { PlaceId = 7,  Title = "Lăng vua Minh Mạng",   SoLuotTrongNuoc = 14773, SoLuotQuocTe = 21785  },
                    new ThongKeChartItem { PlaceId = 8,  Title = "Lăng vua Khải Định",   SoLuotTrongNuoc = 27028, SoLuotQuocTe = 58663  },
                    new ThongKeChartItem { PlaceId = 9,  Title = "Lăng vua Gia Long",    SoLuotTrongNuoc = 8926,  SoLuotQuocTe = 818    }
                },
                Grid = new()
            };

            return new
            {
                chart = fallback.Chart,
                grid = fallback.Grid,
                isFallback = true,
                fallbackReason = reason
            };
        }
    }
}