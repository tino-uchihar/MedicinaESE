using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

namespace MedicinaESE.Pages.Pacientee
{
    public class HistorialCitasModel : PageModel
    {
        public class CitaViewModel
        {
            public int IdCita { get; set; }
            public DateTime Fecha { get; set; }
            public string Hora { get; set; } = "";
            public string TipoCita { get; set; } = "";
            public string Profesional { get; set; } = "";
            public string Estado { get; set; } = "";
            public string Notas { get; set; } = "";
        }

        public List<CitaViewModel> Citas { get; set; } = new();

        public void OnGet()
        {
            var documento = HttpContext.Session.GetString("UsuarioDocumento");
            if (string.IsNullOrEmpty(documento))
            {
                Response.Redirect("/login");
                return;
            }

            // Leer las citas desde la sesión
            var citasJson = HttpContext.Session.GetString("CitasPaciente");
            if (!string.IsNullOrEmpty(citasJson))
            {
                Citas = JsonSerializer.Deserialize<List<CitaViewModel>>(citasJson) ?? new List<CitaViewModel>();
            }
            else
            {
                Citas = new List<CitaViewModel>();
            }
        }

        public IActionResult OnPostGenerarPDF(int id)
        {
            var documento = HttpContext.Session.GetString("UsuarioDocumento");
            if (string.IsNullOrEmpty(documento))
            {
                return Redirect("/login");
            }

            // Leer las citas desde la sesión
            var citasJson = HttpContext.Session.GetString("CitasPaciente");
            var citas = !string.IsNullOrEmpty(citasJson) ? JsonSerializer.Deserialize<List<CitaViewModel>>(citasJson) : new List<CitaViewModel>();
            var cita = citas?.FirstOrDefault(c => c.IdCita == id);
            if (cita == null)
            {
                TempData["Mensaje"] = "No se encontró la cita.";
                return RedirectToPage();
            }

            // Crear el PDF
            using (var ms = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontSection = new XFont("Arial", 14, XFontStyle.Bold);
                var fontNormal = new XFont("Arial", 12, XFontStyle.Regular);
                var fontLabel = new XFont("Arial", 12, XFontStyle.Bold);
                var blueBrush = new XSolidBrush(XColor.FromArgb(0, 102, 204));
                var grayBrush = new XSolidBrush(XColor.FromArgb(240, 240, 240));
                var blackBrush = XBrushes.Black;

                int margin = 40;
                int y = margin;

                // Título principal
                gfx.DrawString("Resumen de Cita Médica", fontTitle, blueBrush, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
                y += 45;

                // Línea horizontal
                gfx.DrawLine(XPens.DarkBlue, margin, y, page.Width - margin, y);
                y += 20;

                // Recuadro de datos
                int boxHeight = 180;
                gfx.DrawRectangle(grayBrush, margin, y, page.Width - 2 * margin, boxHeight);

                int dataY = y + 20;
                int labelX = margin + 15;
                int valueX = margin + 150;
                int lineSpacing = 28;

                // Sección: Datos de la cita
                gfx.DrawString("Datos de la cita", fontSection, blueBrush, labelX, dataY);
                dataY += lineSpacing;
                gfx.DrawString("Fecha:", fontLabel, blackBrush, labelX, dataY);
                gfx.DrawString($"{cita.Fecha:dd/MM/yyyy}", fontNormal, blackBrush, valueX, dataY);
                dataY += lineSpacing;
                gfx.DrawString("Hora:", fontLabel, blackBrush, labelX, dataY);
                gfx.DrawString(cita.Hora, fontNormal, blackBrush, valueX, dataY);
                dataY += lineSpacing;
                gfx.DrawString("Tipo de cita:", fontLabel, blackBrush, labelX, dataY);
                gfx.DrawString(cita.TipoCita, fontNormal, blackBrush, valueX, dataY);
                dataY += lineSpacing;
                gfx.DrawString("Profesional:", fontLabel, blackBrush, labelX, dataY);
                gfx.DrawString(cita.Profesional, fontNormal, blackBrush, valueX, dataY);
                dataY += lineSpacing;
                gfx.DrawString("Estado:", fontLabel, blackBrush, labelX, dataY);
                gfx.DrawString(cita.Estado, fontNormal, blackBrush, valueX, dataY);

                // Sección: Notas
                int notasY = y + boxHeight + 30;
                gfx.DrawString("Notas de la cita", fontSection, blueBrush, labelX, notasY);
                notasY += 22;
                var notasRect = new XRect(labelX, notasY, page.Width - 2 * margin - 30, 60);
                gfx.DrawRectangle(XBrushes.White, notasRect);
                gfx.DrawString(string.IsNullOrWhiteSpace(cita.Notas) ? "(Sin notas)" : cita.Notas, fontNormal, blackBrush, notasRect, XStringFormats.TopLeft);

                // Pie de página
                string footer = $"Documento generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
                gfx.DrawString(footer, new XFont("Arial", 9, XFontStyle.Italic), XBrushes.Gray, new XRect(0, page.Height - 30, page.Width, 20), XStringFormats.Center);

                document.Save(ms, false);
                ms.Position = 0;
                var fileName = $"Cita_{cita.IdCita}_{cita.Fecha:yyyyMMdd}.pdf";
                return File(ms.ToArray(), "application/pdf", fileName);
            }
        }
    }
} 