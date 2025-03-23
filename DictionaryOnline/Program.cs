using DictionaryOnline.Data;
using DictionaryOnline.Models;
using DictionaryOnline.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Đăng ký ITranslationService với triển khai cụ thể
builder.Services.AddScoped<ITranslationService, GoogleTranslationService>();
// Add services to the container.
// Register HttpClient
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DictionaryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, Role>(options =>
{
    // Cấu hình tùy chọn Identity nếu cần
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<DictionaryDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Thêm middleware Authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
// Thêm dữ liệu mẫu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    SeedUsers(userManager, roleManager).Wait();
    var context = scope.ServiceProvider.GetRequiredService<DictionaryDbContext>();
    context.Database.Migrate(); // Đảm bảo database được tạo/migrate

    // Kiểm tra và thêm dữ liệu mẫu vào Dictionaries nếu chưa có
    if (!context.Dictionaries.Any())
    {
        context.Dictionaries.AddRange(
            new Dictionary
            {
                Name = "Anh - Việt",
                SourceLanguage = "en",
                TargetLanguage = "vi",
                Description = "Từ điển tiếng Anh - tiếng Việt",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Dictionary
            {
                Name = "Việt - Anh",
                SourceLanguage = "vi",
                TargetLanguage = "en",
                Description = "Từ điển tiếng Việt - tiếng Anh",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        );
        context.SaveChanges();
    }
}

app.Run();
async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
{
    Console.WriteLine("Bắt đầu SeedUsers...");
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new Role { Name = "Admin", Description = "Administrator role" });
        Console.WriteLine("Đã tạo role Admin");

    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new Role { Name = "User", Description = "Regular user" });
        Console.WriteLine("Đã tạo role User");

    }

    // Kiểm tra xem User đã tồn tại hay chưa
    var existingUser1 = await userManager.FindByEmailAsync("hoai@example.com");
    if (existingUser1 == null)
    {
        var user1 = new User
        {
            UserName = "Hoai",
            Email = "hoai@example.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            RoleId = 1,
            SecurityStamp = Guid.NewGuid().ToString() // Quan trọng
        };

        var result = await userManager.CreateAsync(user1, "tiendzvd123");
        if (result.Succeeded)
        {
            Console.WriteLine("Đã tạo user Hoai");
            await userManager.AddToRoleAsync(user1, "User");
        }
        else
        {
            Console.WriteLine($"Lỗi tạo user Hoai: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    var existingUser2 = await userManager.FindByEmailAsync("tien@example.com");
    if (existingUser2 == null)
    {
        var user2 = new User
        {
            UserName = "Tien",
            Email = "tien@example.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            RoleId = 2,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user2, "tiendzvd123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user2, "User");
        }
    }
}
