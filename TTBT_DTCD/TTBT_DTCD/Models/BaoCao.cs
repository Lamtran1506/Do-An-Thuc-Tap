using System.ComponentModel.DataAnnotations;
namespace TTBT_DTCD.Models
{
    public class BaoCao
    {
        [Key] public int Id { get; set; }
        [Required] public DateTime Ngay { get; set; } = DateTime.Today;
        [Required] public int SoVeBanRa { get; set; }
        [Required] public int LuotKhachCamera { get; set; }
        [Required] public decimal DoanhThu { get; set; }
        public string? GhiChu { get; set; }
    }
}
