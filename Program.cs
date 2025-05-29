using MedicinaESE.Services; // Servicio de Noticias
using Microsoft.Data.SqlClient; // 🔹 Usar Microsoft.Data.SqlClient en lugar de System.Data.SqlClient

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DoctorNowDB");

// Registrar la conexión SQL para inyección de dependencias
builder.Services.AddSingleton(_ => new SqlConnection(connectionString));

// Registrar Razor Pages
builder.Services.AddRazorPages();

// Registrar servicio de Noticias
builder.Services.AddSingleton<NoticiaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
