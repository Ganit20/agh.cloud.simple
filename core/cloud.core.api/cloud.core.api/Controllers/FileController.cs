using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloud.core.api.Services;
using cloud.core.objects.Model;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cloud.core.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        FileService fileService;
        public FileController(FileService fileService)
        {
            this.fileService = fileService;
        }
        [HttpGet]
        public async Task<List<FilePreview>> GetFiles()
        {
            return await this.fileService.GetFileList("/");

        }
        [HttpPost]
        public async Task<ActionResult> UploadFile([FromBody] UploadFileRequest request)
        {
            if (string.IsNullOrEmpty(request.File) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Type) || string.IsNullOrEmpty(request.Path))
            {
                return BadRequest("Wszystkie pola są wymagane.");
            }
            string base64Data = request.File.Replace("data:" + request.Type + ";base64,", string.Empty);

            // Konwersja danych Base64 na bajty
            byte[] fileBytes = System.Convert.FromBase64String(base64Data);

            // Utworzenie ścieżki do zapisu pliku
            string filePath = Path.Combine(request.Path, request.Name);

            try
            {
                // Zapis pliku na serwerze
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                // Zwrócenie sukcesu
                return Ok("Plik został pomyślnie zapisany.");
            }
            catch (IOException ex)
            {
                // Obsługa błędów związanych z zapisem pliku
                return StatusCode(500, $"Wystąpił błąd podczas zapisu pliku: {ex.Message}");
            }


        }
    }
}

