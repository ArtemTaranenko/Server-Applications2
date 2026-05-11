using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(
    _ => { },
    typeof(Program).Assembly,
    typeof(BaseService).Assembly);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<MyDbContext>(x => x.UseSqlServer(connectionString));


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();