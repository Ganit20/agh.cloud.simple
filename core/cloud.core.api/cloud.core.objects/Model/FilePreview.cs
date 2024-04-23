using System;
using cloud.core.objects.Enums;

namespace cloud.core.objects.Model
{
	public class FilePreview
	{
		public string Name { get; set; }
		public FileTypes Type { get; set; }
		public long Size { get; set; }
		public string Thumbnail { get; set; }
	}
}

