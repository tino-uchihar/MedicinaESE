using MedicinaESE.Services;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DoctorNowDB") 
                       ?? throw new InvalidOperationException("La cadena de conexión no está definida en appsettings.json");


// Registrar la conexión SQL
builder.Services.AddSingleton(_ => new SqlConnection(connectionString));

// Registrar Razor Pages
builder.Services.AddRazorPages();

// Registrar servicio de Noticias con la cadena de conexión
builder.Services.AddSingleton<NoticiaService>(provider => new NoticiaService(connectionString));

var app = builder.Build();

// Configurar el pipeline de ejecución
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
