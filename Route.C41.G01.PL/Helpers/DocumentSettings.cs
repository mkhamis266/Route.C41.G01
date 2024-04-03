using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Route.C41.G01.PL.Helpers
{
	public static class DocumentSettings
	{
		public static async Task<string> UploadFile(IFormFile file, string folderName)
		{
			//1. Get Located Folder Path
			//string FolderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{folderName}"; 
			string FolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files",folderName);

			if (!Directory.Exists(FolderPath))
				Directory.CreateDirectory(FolderPath);

			//2. Get FileName and make it UNIQE
			// file.Name;  extextion .jpg
			string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

			//3. Get File Path
			string filePath = Path.Combine(FolderPath, fileName);

			//4. Save File as Streams [Data Per Time]
			var FileStream = new FileStream(filePath,FileMode.Create);

			await file.CopyToAsync(FileStream);

			return fileName;
		}

		public static void DeleteFile(string fileName, string folderName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files",folderName,fileName);
			if(File.Exists(filePath))
				File.Delete(filePath);	

		}
	}
}
