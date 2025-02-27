using System.Net;

namespace TechBack_Identity.Services
{
    // کلاس سرویس ارسال پیامک
    public class SMSService
    {
        // متد ارسال پیامک که شماره تلفن و کد تأیید را دریافت می‌کند
        public void Send(string PhoneNumber, string Code)
        {
            // ایجاد یک نمونه از کلاینت وب برای فراخوانی API
            var client = new WebClient();

            // ساخت URL درخواست به API کاوه‌نگار
            // این URL شامل کلید API، شماره گیرنده، کد تأیید و قالب پیام است
            string url = $"http://api.kavenegar.com/v1/APIkey/verify/lookup.json?receptor={PhoneNumber}&token={Code}&template=VerifyCementAccount";

            // ارسال درخواست به API و دریافت پاسخ
            var content = client.DownloadString(url);
        }
    }
}
