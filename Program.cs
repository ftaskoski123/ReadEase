using Books.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ReadEase_C_.Helpers;
using ReadEase_C_.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<HashingService>();
builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<Mail>();
builder.Services.AddScoped<ConnectionService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401; 
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:5173") // frontend origin
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
