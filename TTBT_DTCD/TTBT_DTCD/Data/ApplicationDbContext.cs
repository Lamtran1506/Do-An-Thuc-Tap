using Microsoft.EntityFrameworkCore;
using TTBT_DTCD.Models;
using System;

namespace TTBT_DTCD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User>      Users      { get; set; }
        public DbSet<DoanhThu>  DoanhThus  { get; set; }
        public DbSet<Ve>        Ves        { get; set; }
        public DbSet<HoSoDiSan> HoSoDiSans { get; set; }
        public DbSet<BaoCao>    BaoCaos    { get; set; }
        public DbSet<WidgetConfig> WidgetConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {

            b.Entity<DoanhThu>().HasData(
                new DoanhThu { Id=1, Ngay=new DateTime(2026,6,20), LoaiDoanhThu="Bán vé",          SoTien=12000000, DiemThu="Đại Nội Huế",      NhanVienPhuTrach="Nguyễn Văn A", NhanVienLap="Mỹ" },
                new DoanhThu { Id=2, Ngay=new DateTime(2026,6,21), LoaiDoanhThu="Bán vé",          SoTien=15000000, DiemThu="Lăng Khải Định",  NhanVienPhuTrach="Lê Thị B",    NhanVienLap="Mỹ" },
                new DoanhThu { Id=3, Ngay=new DateTime(2026,6,22), LoaiDoanhThu="Bán vé",          SoTien=11000000, DiemThu="Lăng Minh Mạng",  NhanVienPhuTrach="Trần Đình C",  NhanVienLap="Mỹ" },
                new DoanhThu { Id=4, Ngay=new DateTime(2026,6,23), LoaiDoanhThu="Dịch vụ thuyết minh", SoTien=4000000,  DiemThu="Hoàng Thành Huế", NhanVienPhuTrach="Phạm Văn D",  NhanVienLap="Mỹ" }
            );

            b.Entity<Ve>().HasData(
                new Ve { Id=1, MaVe="VE-2026-001", LoaiVe="Người lớn", GiaVe=150000, NgayBan=new DateTime(2026,6,23), DiemBan="Đại Nội Huế",      NhanVienBan="Nguyễn Văn A", TrangThai="Chưa sử dụng" },
                new Ve { Id=2, MaVe="VE-2026-002", LoaiVe="Trẻ em",    GiaVe=30000,  NgayBan=new DateTime(2026,6,23), DiemBan="Lăng Khải Định",   NhanVienBan="Trần Thị B",   TrangThai="Đã sử dụng"   },
                new Ve { Id=3, MaVe="VE-2026-003", LoaiVe="Khách Quốc tế", GiaVe=200000, NgayBan=new DateTime(2026,6,22), DiemBan="Lăng Minh Mạng",   NhanVienBan="Lê Văn C",     TrangThai="Chưa sử dụng" },
                new Ve { Id=4, MaVe="VE-2026-004", LoaiVe="Ưu tiên",   GiaVe=75000,  NgayBan=new DateTime(2026,6,21), DiemBan="Đại Nội Huế",      NhanVienBan="Phạm Thị D",   TrangThai="Đã sử dụng"  }
            );

            b.Entity<HoSoDiSan>().HasData(
                new HoSoDiSan { Id=1, MaHoSo="DS-ĐN-001", TenDiTich="Ngọ Môn (Đại Nội)",   LoaiDiTich="Kiến trúc Cung đình", NienDai="Triều Nguyễn (1829)", ViTri="Kinh thành Huế",   TinhTrang="Ổn định / Đã số hóa",    MoTa="" },
                new HoSoDiSan { Id=2, MaHoSo="DS-KT-014", TenDiTich="Điện Kiến Trung",      LoaiDiTich="Kiến trúc Cung đình", NienDai="Khải Định (1923)",       ViTri="Tử Cấm Thành",     TinhTrang="Đã phục dựng / Đang TH", MoTa="" },
                new HoSoDiSan { Id=3, MaHoSo="DS-HL-005", TenDiTich="Hiển Lâm Các",         LoaiDiTich="Kiến trúc Cung đình", NienDai="Minh Mạng (1822)",  ViTri="Hoàng Thành Huế",  TinhTrang="Đang TH số hóa",         MoTa="" },
                new HoSoDiSan { Id=4, MaHoSo="DS-LT-022", TenDiTich="Lăng vua Thiệu Trị",   LoaiDiTich="Lăng tẩm",           NienDai="Thiệu Trị (1848)",            ViTri="Xã Thủy Bằng, Huế", TinhTrang="Chờ phê duyệt",          MoTa="" }
            );

            b.Entity<BaoCao>().HasData(
                new BaoCao { Id=1, Ngay=new DateTime(2026,6,20), SoVeBanRa=345, LuotKhachCamera=3890, DoanhThu=51750000, GhiChu="" },
                new BaoCao { Id=2, Ngay=new DateTime(2026,6,21), SoVeBanRa=412, LuotKhachCamera=4620, DoanhThu=61800000, GhiChu="" },
                new BaoCao { Id=3, Ngay=new DateTime(2026,6,22), SoVeBanRa=298, LuotKhachCamera=3120, DoanhThu=44700000, GhiChu="" },
                new BaoCao { Id=4, Ngay=new DateTime(2026,6,23), SoVeBanRa=150, LuotKhachCamera=15880, DoanhThu=22500000, GhiChu="" } 
            );
        }
    }
}