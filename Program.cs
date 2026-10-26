using Microsoft.EntityFrameworkCore;
using THanh1.Data;
using THanh1.Models; // Thay THanh1 bằng tên namespace thực tế của dự án bạn nếu có

var builder = WebApplication.CreateBuilder(args);

// Đăng ký SchoolContext với SQL Server sử dụng ConnectionString đã cấu hình
builder.Services.AddDbContext<SchoolContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// **BỔ SUNG ĐOẠN NÀY ĐỂ TỰ ĐỘNG KHỞI TẠO DỮ LIỆU MẪU (DB INITIALIZER)**
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Hoặc app.MapStaticAssets() tùy theo template VS của bạn

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();