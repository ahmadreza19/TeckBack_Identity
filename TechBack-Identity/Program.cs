using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechBack_Identity.Data;
using TechBack_Identity.Helpers;
using TechBack_Identity.Models.Entitys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ????? ???? DbContext
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders()
    ///می تونیم ارور رو شخصی سازی کنیم
     .AddErrorDescriber<CustomIdentityError>()
     .AddPasswordValidator<MyPasswordValidetor>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>,AddMyClaims>();
// تنظیمات هویت کاربران
builder.Services.Configure<IdentityOptions>(option =>
{
    //UserSetting - تنظیمات کاربر
    //option.User.AllowedUserNameCharacters = "abcd123"; // کامنت شده: محدود کردن کاراکترهای نام کاربری
    option.User.RequireUniqueEmail = true; // الزام ایمیل منحصر به فرد

    //Password Setting - تنظیمات رمز عبور
    option.Password.RequireDigit = false; // عدم نیاز به استفاده از اعداد در رمز عبور
    option.Password.RequireLowercase = false; // عدم نیاز به استفاده از حروف کوچک در رمز عبور
    option.Password.RequireNonAlphanumeric = false; // عدم نیاز به استفاده از کاراکترهای خاص مانند !@#$%^&*()_+
    option.Password.RequireUppercase = false; // عدم نیاز به استفاده از حروف بزرگ در رمز عبور
    option.Password.RequiredLength = 8; // حداقل طول رمز عبور: ۸ کاراکتر
    option.Password.RequiredUniqueChars = 1; // حداقل تعداد کاراکترهای منحصر به فرد: ۱

    //Lokout Setting - تنظیمات قفل شدن حساب
    option.Lockout.MaxFailedAccessAttempts = 3; // قفل شدن حساب پس از ۳ بار تلاش ناموفق
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(10); // مدت زمان قفل شدن: ۱۰ میلی‌ثانیه

    //SignIn Setting - تنظیمات ورود به سیستم
    option.SignIn.RequireConfirmedAccount = false; // عدم نیاز به تأیید حساب
    option.SignIn.RequireConfirmedEmail = false; // عدم نیاز به تأیید ایمیل
    option.SignIn.RequireConfirmedPhoneNumber = false; // عدم نیاز به تأیید شماره تلفن
});

// تنظیمات کوکی برنامه
builder.Services.ConfigureApplicationCookie(option =>
{
    // cookie setting - تنظیمات کوکی
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10); // مدت زمان انقضای کوکی: ۱۰ دقیقه
    option.LoginPath = "/account/login"; // مسیر صفحه ورود
    option.AccessDeniedPath = "/Account/AccessDenied"; // مسیر صفحه عدم دسترسی
    option.SlidingExpiration = true; // تمدید خودکار زمان انقضای کوکی در صورت فعالیت کاربر
});

var app = builder.Build();

// ???? ??????? middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
).WithStaticAssets();


app.Run();
