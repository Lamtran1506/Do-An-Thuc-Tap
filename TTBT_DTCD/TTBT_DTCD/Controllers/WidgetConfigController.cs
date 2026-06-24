using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TTBT_DTCD.Models;

namespace TTBT_DTCD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetConfigController : ControllerBase
    {

        private static List<WidgetConfig> _widgetConfigs = new List<WidgetConfig>
        {
            new WidgetConfig { WidgetId = "kpi", Title = "TỔNG QUAN CHỈ SỐ", Icon = "ti ti-chart-infographic", Color = "#1a4fa0", Type = "kpi", X = 0, Y = 0, W = 12, H = 3, IsDeleted = false },
            new WidgetConfig { WidgetId = "chart-bar", Title = "LƯỢT THAM QUAN DI TÍCH", Icon = "ti ti-chart-bar", Color = "#185fa5", Type = "bar-chart", X = 0, Y = 3, W = 4, H = 5, IsDeleted = false },
            new WidgetConfig { WidgetId = "chart-line", Title = "DOANH THU THEO THÁNG", Icon = "ti ti-chart-line", Color = "#0f6e56", Type = "line-chart", X = 4, Y = 3, W = 4, H = 5, IsDeleted = false },
            new WidgetConfig { WidgetId = "chart-pie", Title = "TỶ LỆ VÉ BÁN", Icon = "ti ti-chart-pie", Color = "#854f0b", Type = "pie-chart", X = 8, Y = 3, W = 4, H = 5, IsDeleted = false },
            new WidgetConfig { WidgetId = "sukien", Title = "SỰ KIỆN SẮP DIỄN RA", Icon = "ti ti-calendar-event", Color = "#185fa5", Type = "table", X = 0, Y = 8, W = 6, H = 5, IsDeleted = false },
            new WidgetConfig { WidgetId = "duan", Title = "DỰ ÁN BẢO TỒN", Icon = "ti ti-building", Color = "#854f0b", Type = "table", X = 6, Y = 8, W = 6, H = 5, IsDeleted = false },
            new WidgetConfig { WidgetId = "camera", Title = "CAMERA LƯỢT QUÉT THEO CỔNG", Icon = "ti ti-camera", Color = "#993356", Type = "camera", X = 0, Y = 13, W = 6, H = 5, IsDeleted = false },
            new WidgetConfig { WidgetId = "nhanvien", Title = "CHUYÊN VIÊN & THU CHI", Icon = "ti ti-users", Color = "#533ab7", Type = "staff-finance", X = 6, Y = 13, W = 6, H = 5, IsDeleted = false }
        };

        [HttpGet("get-all-settings")]
        public IActionResult GetAllSettings()
        {
            return Ok(_widgetConfigs);
        }

        [HttpGet("get-active-layout")]
        public IActionResult GetActiveLayout()
        {
            var active = _widgetConfigs.Where(w => !w.IsDeleted).ToList();
            return Ok(active);
        }

        [HttpPost("save-layout")]
        public IActionResult SaveLayout([FromBody] List<WidgetConfig> updatedList)
        {
            if (updatedList == null || !updatedList.Any()) return BadRequest();

            foreach (var item in updatedList)
            {
                var match = _widgetConfigs.FirstOrDefault(w => w.WidgetId == item.WidgetId);
                if (match != null)
                {
                    match.X = item.X; 
                    match.Y = item.Y;
                    match.W = item.W; 
                    match.H = item.H;
                    match.Title = item.Title; 
                    match.IsDeleted = item.IsDeleted;
                }
            }
            return Ok(new { success = true });
        }
    }
}