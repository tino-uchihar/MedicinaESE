// Pages/Medicoo/Historial-Citas.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Text.Json;

namespace MedicinaESE.Pages.Medicoo
{
    public partial class HistorialCitasMedicoModel : PageModel
    {
        // ---------------- DTO ----------------
        public class CitaViewModel
        {
            public int      IdCita    { get; set; }
            public DateTime Fecha     { get; set; }
            public string   Hora      { get; set; } = "";
            public string   Paciente  { get; set; } = "";
            public string   TipoCita  { get; set; } = "";
            public string   Estado    { get; set; } = "";
            public string   Notas     { get; set; } = "";
        }

        public List<CitaViewModel> Citas { get; private set; } = new();

        // ---------------- GET ----------------
        public IActionResult OnGet()
        {
            // 1. Recuperar documento del médico logueado
            string? doc = HttpContext.Session.GetString("UsuarioDocumento");
            if (string.IsNullOrEmpty(doc))
                return Redirect("/login");

            // 2. Acceso a BD (via DI)
            var db = HttpContext.RequestServices
                                .GetRequiredService<MedicinaESE.Data.ApplicationDbContext>();

            // 3. Obtener médico⇄usuario
            var usuario = db.Usuarios
                            .FirstOrDefault(u => u.DocumentoId == doc);

            if (usuario is null)
            {
                Citas = [];                       // C# 12 collection literal
                return Page();
            }

            var medico = db.Medicos
                           .Include(m => m.Usuario)
                           .FirstOrDefault(m => m.IdUsuario == usuario.IdUsuario);

            if (medico is null)
            {
                Citas = [];
                return Page();
            }

            string especialidad = medico.Especialidad ?? "";

            // 4. Fetch citas (en versión real iría a la tabla Citas)
            var citasJson = HttpContext.Session.GetString("CitasPaciente");
            List<CitaViewModel> todas = string.IsNullOrEmpty(citasJson)
                                        ? []
                                        : JsonSerializer.Deserialize<List<CitaViewModel>>(citasJson)!;

            Citas = todas
                    .Where(c => c.TipoCita.Replace("_", " ")
                                          .Contains(especialidad,
                                                    StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return Page();
        }

        // ----------- POST: PDF ---------------
        public IActionResult OnPostGenerarPDF(int id)
        {
            string? doc = HttpContext.Session.GetString("UsuarioDocumento");
            if (string.IsNullOrEmpty(doc))
                return Redirect("/login");

            var citasJson = HttpContext.Session.GetString("CitasPaciente");
            List<CitaViewModel> citas = string.IsNullOrEmpty(citasJson)
                                        ? []
                                        : JsonSerializer.Deserialize<List<CitaViewModel>>(citasJson)!;

            if (citas.FirstOrDefault(c => c.IdCita == id) is not CitaViewModel cita)
            {
                TempData["Mensaje"] = "No se encontró la cita.";
                return RedirectToPage();
            }

            // --------------- PDF ---------------
            using var ms = new MemoryStream();
            var docPdf  = new PdfDocument();
            var page    = docPdf.AddPage();
            var gfx     = XGraphics.FromPdfPage(page);

            var fTitle   = new XFont("Arial", 18, XFontStyle.Bold);
            var fSection = new XFont("Arial", 14, XFontStyle.Bold);
            var fNormal  = new XFont("Arial", 12, XFontStyle.Regular);
            var fLabel   = new XFont("Arial", 12, XFontStyle.Bold);

            var azul = new XSolidBrush(XColor.FromArgb(0, 102, 204));
            var gris = new XSolidBrush(XColor.FromArgb(240, 240, 240));

            int m = 40;
            int y = m;

            gfx.DrawString("Resumen de Cita Médica", fTitle, azul,
                           new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
            y += 45; gfx.DrawLine(XPens.DarkBlue, m, y, page.Width - m, y); y += 20;

            // ---------- cuadro gris -----------
            gfx.DrawRectangle(gris, m, y, page.Width - 2 * m, 180);
            int dy = y + 20, lx = m + 15, vx = m + 150, step = 28;

            void Line(string label, string value)
            {
                gfx.DrawString(label, fLabel,  XBrushes.Black, lx, dy);
                gfx.DrawString(value, fNormal, XBrushes.Black, vx, dy);
                dy += step;
            }

            gfx.DrawString("Datos de la cita", fSection, azul, lx, dy); dy += step;
            Line("Fecha:",        cita.Fecha.ToString("dd/MM/yyyy"));
            Line("Hora:",         cita.Hora);
            Line("Paciente:",     cita.Paciente);
            Line("Tipo de cita:", cita.TipoCita);
            Line("Estado:",       cita.Estado);

            // -------- Notas ----------
            int notasY = y + 210;
            gfx.DrawString("Notas", fSection, azul, lx, notasY);
            notasY += 20;
            var rect = new XRect(lx, notasY, page.Width - 2 * m - 20, 60);
            gfx.DrawRectangle(XBrushes.White, rect);
            gfx.DrawString(string.IsNullOrWhiteSpace(cita.Notas) ? "(Sin notas)" : cita.Notas,
                           fNormal, XBrushes.Black, rect, XStringFormats.TopLeft);

            gfx.DrawString($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}",
                           new XFont("Arial", 9, XFontStyle.Italic), XBrushes.Gray,
                           new XRect(0, page.Height - 30, page.Width, 20), XStringFormats.Center);

            docPdf.Save(ms, false);
            ms.Position = 0;
            return File(ms.ToArray(), "application/pdf",
                        $"Cita_{cita.IdCita}_{cita.Fecha:yyyyMMdd}.pdf");
        }
    }
}
