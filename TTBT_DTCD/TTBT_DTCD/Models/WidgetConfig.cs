using System.ComponentModel.DataAnnotations;

namespace TTBT_DTCD.Models
{
    public class WidgetConfig
    {
        [Key]
        public string WidgetId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public bool IsDeleted { get; set; }
    }
}