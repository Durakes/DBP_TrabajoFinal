using PruebaDBP.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Cadena de conexion
var conexionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<bdlumiereContext>(options =>
{
    options.UseMySql(conexionString, ServerVersion.AutoDetect(conexionString));
});

//Fin de cadena de conexion

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
