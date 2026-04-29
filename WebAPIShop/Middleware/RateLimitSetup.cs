using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace WebAPIShop.Extensions
{
    public static class RateLimitSetup
    {
        public static void AddCustomRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // הגדרת הודעת השגיאה כשהמשתמש נחסם
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // יצירת הגבלה לפי IP ומשתמש
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    // מפתח זיהוי: משלב IP ושם משתמש (אם קיים) כדי למנוע מעקפים
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var user = httpContext.User.Identity?.Name ?? "guest";
                    var partitionKey = $"{user}_{ip}";

                    return RateLimitPartition.GetSlidingWindowLimiter(partitionKey, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 30,              // מקסימום 30 בקשות
                        Window = TimeSpan.FromMinutes(1), // בתוך חלון של דקה
                        SegmentsPerWindow = 3,         // חלוקת הדקה ל-3 מקטעים (כל 20 שניות חלק מהמכסה מתנקה)
                        QueueLimit = 0,                // חשוב: דחייה מיידית ללא תורים כדי לא להעמיס על השרת
                        AutoReplenishment = true
                    });
                });
            });
        }
    }
}