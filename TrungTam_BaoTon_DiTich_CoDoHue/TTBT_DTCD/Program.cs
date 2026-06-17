using Microsoft.EntityFrameworkCore;
using TTBT_DTCD.Data;
using TTBT_DTCD.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình các dịch vụ (Services)
builder.Services.AddControllersWithViews();

// Cấu hình kết nối SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Session (dùng để lưu thông tin đăng nhập & phân quyền)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 2. Cấu hình Database và tài khoản Admin mặc định (Seed Data)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    // Tạo tài khoản Admin mặc định nếu chưa có
    if (!context.Users.Any(u => u.Username == "admin"))
    {
        context.Users.Add(new User
        {
            Username = "admin",
            Password = "123",
            FullName = "Lâm", // ĐÃ ĐỔI: Tên hiển thị mặc định của admin sẽ là Lâm luôn cho phù hợp ý bạn
            Email = "admin@webquanly.local",
            PhoneNumber = "0000000000",
            Role = "Admin"
        });
        context.SaveChanges();
    }
}

// 3. Cấu hình Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    
    // ĐÃ DI CHUYỂN VÀO ĐÂY: Chỉ ép chuyển hướng sang cổng HTTPS khi chạy thực tế (Production)
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();