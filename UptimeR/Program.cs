using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uptimer.ML.Reader;
using UptimeR.Application.Interfaces;
using UptimeR.Application.UseCases;
using UptimeR.Areas.Identity;
using UptimeR.Data;
using UptimeR.ML.Trainer;
using UptimeR.ML.Trainer.Interfaces;
using UptimeR.Persistance;
using UptimeR.Persistance.Repositorys;
using UptimeR.Policies;
using UptimeR.Services;
using UptimeR.Services.Worker;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim(Storage_Claims.Admin.Type, Storage_Claims.Admin.Value));
});


builder.Services.AddScoped<IRavenDB, RavenDB>();
builder.Services.AddScoped<ISQLConn, SQLConn>();

builder.Services.AddScoped<IUptimeWorker, UptimeWorker>();
builder.Services.AddSingleton<WorkerService>();
builder.Services.AddHostedService(s => s.GetRequiredService<WorkerService>());

builder.Services.AddScoped<IAnomalyDetector, AnomalyDetector>();
builder.Services.AddSingleton<DetectAnomaliesService>();
builder.Services.AddHostedService(s => s.GetRequiredService<DetectAnomaliesService>());

builder.Services.AddControllers();

var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/hello", (Func<string>)(() => "Hello World!"));

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