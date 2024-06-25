var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession();

// Add SignalR services\
builder.Services.AddSignalR();

// Add Chessboard as a singleton service
builder.Services.AddSingleton<Chessboard>();

// Add Game as a singleton service
builder.Services.AddSingleton<Game>();

// Add ChessboardInitializer as a hosted service
builder.Services.AddHostedService<ChessboardInitializer>();

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

app.UseAuthorization();
app.UseSession(); // This must be after UseRouting() and before UseEndpoints()

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map SignalR hub(s)
//app.MapHub<YourHub>("/yourhub");

app.Run();
