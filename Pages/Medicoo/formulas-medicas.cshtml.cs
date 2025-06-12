using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

namespace MedicinaESE.Pages.Medicoo
{
    public class FormulasMedicasModel : PageModel
    {
        [BindProperty]
        public string Paciente { get; set; } = "";
        [BindProperty]
        public string Medicamentos { get; set; } = "";
        [BindProperty]
        public string Indicaciones { get; set; } = "";
        public bool PdfGenerado { get; set; } = false;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Paciente) || string.IsNullOrWhiteSpace(Medicamentos) || string.IsNullOrWhiteSpace(Indicaciones))
            {
                ModelState.AddModelError(string.Empty, "Todos los campos son obligatorios.");
                return Page();
            }

            using (var ms = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontSection = new XFont("Arial", 14, XFontStyle.Bold);
                var fontNormal = new XFont("Arial", 12, XFontStyle.Regular);
                var blueBrush = new XSolidBrush(XColor.FromArgb(0, 102, 204));
                var blackBrush = XBrushes.Black;
                int margin = 40;
                int y = margin;
                gfx.DrawString("Fórmula Médica", fontTitle, blueBrush, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
                y += 45;
                gfx.DrawLine(XPens.DarkBlue, margin, y, page.Width - margin, y);
                y += 20;
                gfx.DrawString("Paciente:", fontSection, blueBrush, margin, y);
                gfx.DrawString(Paciente, fontNormal, blackBrush, margin + 120, y);
                y += 35;
                gfx.DrawString("Medicamentos:", fontSection, blueBrush, margin, y);
                y += 25;
                gfx.DrawString(Medicamentos, fontNormal, blackBrush, new XRect(margin, y, page.Width - 2 * margin, 60), XStringFormats.TopLeft);
                y += 70;
                gfx.DrawString("Indicaciones:", fontSection, blueBrush, margin, y);
                y += 25;
                gfx.DrawString(Indicaciones, fontNormal, blackBrush, new XRect(margin, y, page.Width - 2 * margin, 60), XStringFormats.TopLeft);
                string footer = $"Documento generado el {System.DateTime.Now:dd/MM/yyyy HH:mm}";
                gfx.DrawString(footer, new XFont("Arial", 9, XFontStyle.Italic), XBrushes.Gray, new XRect(0, page.Height - 30, page.Width, 20), XStringFormats.Center);
                document.Save(ms, false);
                ms.Position = 0;
                var fileName = $"Formula_{Paciente.Replace(" ", "_")}_{System.DateTime.Now:yyyyMMddHHmmss}.pdf";
                return File(ms.ToArray(), "application/pdf", fileName);
            }
        }
    }
} 