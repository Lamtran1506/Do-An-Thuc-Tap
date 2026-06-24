using System.ComponentModel.DataAnnotations;
namespace TTBT_DTCD.Models
{
    public class Ve
    {
        [Key] public int Id { get; set; }
        [Required] public string MaVe { get; set; } = string.Empty;
        [Required] public string LoaiVe { get; set; } = string.Empty;
        [Required][Range(0, double.MaxValue)] public decimal GiaVe { get; set; }
        [Required] public DateTime NgayBan { get; set; } = DateTime.Now;
        [Required] public string DiemBan { get; set; } = string.Empty;
        public string? NhanVienBan { get; set; }
        public string? TrangThai { get; set; } = "Chưa sử dụng";
    }
}
