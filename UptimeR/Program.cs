using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UptimeR.Application;
using UptimeR.Application.Interfaces;
using UptimeR.Application.UseCases;
using UptimeR.Areas.Identity;
using UptimeR.Data;
using UptimeR.Persistance;
using UptimeR.Persistance.Repositorys;
using UptimeR.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IURLRepository, URLRepository>();
builder.Services.AddScoped<ILogHistoryRepository, LogHistoryRepository>();

builder.Services.AddScoped<IURLUseCases, URLUseCases>();
builder.Services.AddScoped<ILogHistoryUseCases, LogHistoryUseCases>();


builder.Services.AddTransient<IUptimeWorker, UptimeWorker>();
builder.Services.AddSingleton<WorkerService>();
builder.Services.AddHostedService(s => s.GetRequiredService<WorkerService>());

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

PrepDb.Preppopulation(app);

app.Run();
