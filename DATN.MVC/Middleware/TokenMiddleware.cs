using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace DATN.MVC.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        private readonly IMemoryCache _cache;

        public TokenMiddleware(RequestDelegate next, IConfiguration configuration, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
            _secretKey = configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured.");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Session.GetString("Token");

            // Kiểm tra nếu đã ở trang Login, bỏ qua Middleware
            var path = context.Request.Path.Value?.ToLower();
            if (path?.Contains("/account/login") == true)
            {
                await _next(context);
                return;
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Nếu context.Items đã chứa các claims, bỏ qua xử lý
                    if (!context.Items.ContainsKey("userId"))
                    {
                        // Giải mã token và lưu vào HttpContext.Items nếu chưa có
                        if (!_cache.TryGetValue(token, out Dictionary<string, string> claims))
                        {
                            claims = ValidateAndDecodeToken(token);
                            _cache.Set(token, claims, TimeSpan.FromMinutes(30));
                        }

                        // Gán các claims vào HttpContext.Items
                        foreach (var claim in claims)
                        {
                            context.Items[claim.Key] = claim.Value;
                        }
                    }
                }
                catch
                {
                    // Token không hợp lệ, xóa session và chuyển hướng
                    ClearSessionData(context);
                    context.Response.Redirect("/Account/Login");
                    return; // Dừng xử lý
                }
            }
            else
            {
                // Không có token, chuyển hướng đến Login
                ClearSessionData(context);
                context.Response.Redirect("/Account/Login");
                return; // Dừng xử lý
            }

            await _next(context); // Chỉ tiếp tục nếu token hợp lệ
        }

        private Dictionary<string, string> ValidateAndDecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = handler.ValidateToken(token, validationParameters, out _);
            return principal.Claims.ToDictionary(c => c.Type, c => c.Value);
        }

        private void ClearSessionData(HttpContext context)
        {
            context.Session.Clear();
        }
    }
}
