using System.Net;
using System.Net.Mail;
using System.Text;

namespace TechBack_Identity.Services
{
    // کلاس سرویس ارسال ایمیل
public class EmailService
{
   // متد اجرای ارسال ایمیل که آدرس گیرنده، متن پیام و موضوع را دریافت می‌کند
   public Task Execute(string UserEmail, string Body, string Subject)
   {
       // ایجاد یک نمونه از کلاینت SMTP برای ارسال ایمیل
       SmtpClient Client = new SmtpClient();
       Client.Port = 587; // تنظیم پورت ارتباطی (پورت استاندارد TLS)
       Client.Host = "smtp.gmail.com"; // تنظیم سرور SMTP جیمیل
       Client.EnableSsl = true; // فعال‌سازی SSL برای ارتباط امن
       Client.Timeout = 10000; // تنظیم زمان انتظار به میلی‌ثانیه (۱۰ ثانیه)
       Client.DeliveryMethod = SmtpDeliveryMethod.Network; // استفاده از شبکه برای ارسال ایمیل
       Client.UseDefaultCredentials = false; // عدم استفاده از اعتبارنامه‌های پیش‌فرض
       Client.Credentials = new NetworkCredential("programmer2023.2004@gmail.com", "Password"); // تنظیم نام کاربری و رمز عبور برای احراز هویت (رمز عبور خالی است)

       // ایجاد پیام ایمیل با مشخص کردن فرستنده، گیرنده، موضوع و متن
       MailMessage message = new MailMessage("Password", UserEmail, Subject, Body);
       message.IsBodyHtml = true; // تنظیم متن پیام به صورت HTML
       message.BodyEncoding = UTF8Encoding.UTF8; // تنظیم کدگذاری متن به UTF-8
       message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess; // درخواست اعلان تحویل در صورت ارسال موفق
       
       // ارسال پیام
       Client.Send(message);
       
       // بازگرداندن یک تسک تکمیل شده (برای پشتیبانی از عملیات ناهمگام)
       return Task.CompletedTask;
   }
}
}
