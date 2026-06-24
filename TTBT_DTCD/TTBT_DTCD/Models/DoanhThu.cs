using System.ComponentModel.DataAnnotations;
namespace TTBT_DTCD.Models
{
    public class DoanhThu
    {
        [Key] public int Id { get; set; }
        [Required] public DateTime Ngay { get; set; } = DateTime.Today;
        [Required] public string LoaiDoanhThu { get; set; } = string.Empty;
        [Required][Range(0, double.MaxValue)] public decimal SoTien { get; set; }
        [Required] public string DiemThu { get; set; } = string.Empty;
        public string? NhanVienPhuTrach { get; set; }
        public string? NhanVienLap { get; set; }
    }
}
