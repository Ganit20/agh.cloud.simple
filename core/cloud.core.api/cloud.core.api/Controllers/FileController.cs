using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cloud.core.api.Services;
using cloud.core.database.interf;
using cloud.core.objects.Database;
using cloud.core.objects.Enums;
using cloud.core.objects.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestEase;
using RestEase.Implementation;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cloud.core.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        FileService fileService;
        IDbFileApi dbFileApi= RestClient.For<IDbFileApi>("http://cloud.core.database");
        private readonly IDbUserApi _userApi = RestClient.For<IDbUserApi>("http://cloud.core.database");

        public FileController(FileService fileService )
        {
            this.fileService = fileService;
        }
        [Authorize]
        [HttpGet]
        public async Task<List<FilePreview>> GetFiles([FromQuery] string? path)
        {
            var userId = User.Claims.FirstOrDefault(x=>x.Type== ClaimTypes.NameIdentifier).Value;
            if (!Directory.Exists(userId))
                Directory.CreateDirectory(userId);
                return await this.fileService.GetFileList(path==null?userId:userId+path);

        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UploadFile([FromBody] UploadFileRequest request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!Directory.Exists(userId))
                Directory.CreateDirectory(userId);
            request.Path = userId + request.Path;
            var userSpaceInfo = await this._userApi.GetUserFileInfo(int.Parse(userId));
            if(userSpaceInfo.SpaceUsed+request.File.Length>userSpaceInfo.MaxCapacity)
                return BadRequest("Brak miejsca!");

            if (string.IsNullOrEmpty(request.File) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Type))
            {
                return BadRequest("Wszystkie pola są wymagane.");
            }
            string base64Data = request.File.Replace("data:" + request.Type + ";base64,", string.Empty);

            byte[] fileBytes = System.Convert.FromBase64String(base64Data);

            string filePath = Path.Combine(request.Path, request.Name);

            try
            {
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
                await dbFileApi.AddFile(int.Parse(userId), fileBytes.Length);
                return Ok("Plik został pomyślnie zapisany.");
            }
            catch (IOException ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas zapisu pliku: {ex.Message}");
            }


        }
        [Authorize]
        [HttpPost("folder")]
        public async Task<ActionResult> CreateFolder([FromBody] CreateFolderRequest request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!Directory.Exists(userId))
                Directory.CreateDirectory(userId);
            if (Directory.Exists(Path.Combine(userId, request.Path))) {
                return BadRequest();
            }
            Directory.CreateDirectory(userId+request.Path);
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteFile([FromQuery] string? path)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!Directory.Exists(userId))
                Directory.CreateDirectory(userId);
            if (Directory.Exists(userId+ path))
            {
                Directory.Delete(userId + path);
            }
            double size = new FileInfo(userId + path).Length;
            if (System.IO.File.Exists(userId + path))
            {
                System.IO.File.Delete(userId + path);
            }
            await dbFileApi.RemoveFile(int.Parse(userId), size);

            return Ok();
        }
        [Authorize]
        [HttpGet("download")]
        public async Task<ActionResult> DownloadFile([FromQuery] string? path)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!Directory.Exists(userId))
                Directory.CreateDirectory(userId);
            
                try
                {
                    string filePath = userId+ path;

                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound(); 
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/octet-stream", filePath.Substring(filePath.LastIndexOf("/") + 1));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
                }
            
        }
        [HttpGet("stream/{id}")]
        public async Task<IActionResult> StreamVideo([FromQuery] string? path,int id)
        {
            var userId = id.ToString();

            if (!System.IO.File.Exists(userId + path))
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(userId + path))
            {
                return StatusCode(500, "Wystąpił błąd podczas transkodowania pliku wideo.");
            }

            var stream = new FileStream(userId + path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            return File(stream, "video/mp4");
        }
        [HttpPost("share")]
        [Authorize]

        public async Task<IActionResult> CreateShareLink([FromBody] FileShareRequest request)
        {

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            request.FilePath = userId + request.FilePath;
            return Ok("http://localhost:4200/file/"+ await this.dbFileApi.CreateShareLink(request));
        }

        [HttpGet("share/{id}")]
        public async Task<IActionResult> GetSharedFileAsync(Guid id)
        {

            var fileInfo = await this.dbFileApi.GetSharedFile(id);
            if (!fileInfo.IsActive)
                return BadRequest();
            return Ok(await this.fileService.GetFile(fileInfo.FilePath));
        }
        [HttpGet("share/all")]
        public async Task<IActionResult> GetAllSharedFiles()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return Ok(await this.dbFileApi.GetAllSharedFiles(int.Parse(userId)));

        }
        [HttpGet("share/download/{id}")]
        public async Task<IActionResult> DownloadSharedFile(Guid id)
        {
            var fileInfo = await this.dbFileApi.GetSharedFile(id);
            if (!fileInfo.IsActive)
                return BadRequest();

            return Ok(await this.DownloadFile(fileInfo.FilePath));

        }
        [Authorize]
        [HttpPut("share/deactivate/{id}")]
        public async Task<IActionResult> GetAllSharedFiles(Guid id)
        {
            await this.dbFileApi.DeactivateLink(id);
            return Ok();

        }
    }
}

