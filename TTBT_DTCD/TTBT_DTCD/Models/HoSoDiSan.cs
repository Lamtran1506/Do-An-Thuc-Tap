using System.ComponentModel.DataAnnotations;
namespace TTBT_DTCD.Models
{
    public class HoSoDiSan
    {
        [Key] public int Id { get; set; }
        [Required] public string MaHoSo { get; set; } = string.Empty;
        [Required] public string TenDiTich { get; set; } = string.Empty;
        [Required] public string LoaiDiTich { get; set; } = string.Empty;
        public string? NienDai { get; set; }
        public string? ViTri { get; set; }
        [Required] public string TinhTrang { get; set; } = string.Empty;
        public string? MoTa { get; set; }
    }
}
