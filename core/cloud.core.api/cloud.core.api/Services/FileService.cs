using System;
using System.Resources;
using cloud.core.objects.Enums;
using cloud.core.objects.Model;
using FFMpegCore.Enums;
using Microsoft.VisualBasic.FileIO;
using Xabe.FFmpeg;

namespace cloud.core.api.Services
{
	public class FileService
	{
		public async Task<List<FilePreview>> GetFileList(string path) {
            var fileList = Directory.GetFiles("/");
            List<FilePreview> result = new();
            foreach(var file in fileList)
            {
                result.Add(new FilePreview()
                {
                    Name = file.Substring(file.LastIndexOf("/") + 1),
                    Size = new FileInfo(file).Length,
                    Type = GetFileType(file),
                    Thumbnail = await GetFileThumbnailAsync(file)
                });
            }
            return result;
        }

        private FileTypes GetFileType(string fileName)
        {
            var extension = new FileInfo(fileName).Extension;
            switch (extension) { 
            case ".pdf":
            case ".doc":
            case ".docx":
                return FileTypes.Document;
            case ".jpg":
            case ".jpeg":
            case ".png":
            case ".gif":
                return FileTypes.Image;
            case ".mp4":
            case ".avi":
            case ".mkv":
                return FileTypes.Video;
            case ".mp3":
            case ".wav":
                return FileTypes.Audio;
            default:
                return FileTypes.Other;
            }
        }
        private async Task<string> GetFileThumbnailAsync(string fileName)
        {
            switch(GetFileType(fileName))
                {
                case FileTypes.Video:
                    {
                        string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".png");
                        IConversion conversion =await FFmpeg.Conversions.FromSnippet.Snapshot(fileName, output, TimeSpan.FromSeconds(0));
                        await conversion.Start();
                        byte[] imageArray = System.IO.File.ReadAllBytes(output);
                        string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                        File.Delete(output);
                        return base64ImageRepresentation;
                    }
                case FileTypes.Image:
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(fileName);
                        string base64ImageRepresentation =$"data:image/{new FileInfo(fileName).Extension.Replace(".","")};base64,{Convert.ToBase64String(imageArray)}";
                        return base64ImageRepresentation;
                    }
                default:
                    return null;
            }
        }
    }
}

