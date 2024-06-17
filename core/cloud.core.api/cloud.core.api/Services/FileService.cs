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
            var fileList = Directory.GetFiles(path);
            List<FilePreview> result = new();
            foreach(var file in fileList)
            {
                result.Add(new FilePreview()
                {
                    Name = file.Substring(file.LastIndexOf("/") + 1),
                    Size = new FileInfo(file).Length,
                    Type = GetFileType(file),
                    Thumbnail = await GetFileThumbnailAsync(file),
                    LastModified=new FileInfo(file).LastWriteTime

                });
            }
            var folders =Directory.GetDirectories(path);
            foreach(var folder in folders)
            {
                result.Add(new FilePreview()
                {
                    Name = folder.Substring(folder.LastIndexOf("/") + 1),
                    Size = 0,
                    Type = FileTypes.Folder,
                    Thumbnail = null,
                    LastModified = new DirectoryInfo(folder).LastWriteTime

                });
            }
            return result.OrderByDescending(x=>x.LastModified).ToList();
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
        public async Task<string> GetFileThumbnailAsync(string fileName)
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
                        return "data:image/png;base64,"+base64ImageRepresentation;
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
        public async Task<string> TranscodeVideoToMp4(string inputFilePath)
        {
            try
            {
                
                var outputPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(inputFilePath)), $"{Path.GetFileNameWithoutExtension(inputFilePath)}_transcoded.mp4");

                var mediaInfo = await FFmpeg.GetMediaInfo(inputFilePath);
                var conversion = await FFmpeg.Conversions.New()
                    .AddParameter($"-i {Path.GetFullPath(inputFilePath)}")
                    .SetOutput(outputPath)
                    .SetOutputFormat(Format.mp4)
                    .Start();

                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas transkodowania pliku wideo: {ex.Message}");
                return null;
            }
        }
    }
}

