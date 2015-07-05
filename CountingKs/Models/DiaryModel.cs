using System;
using System.Collections.Generic;

namespace CountingKs.Models
{
	public class DiaryModel
	{
		//public string Url { get; set; }
		public ICollection<LinkModel> Links { get; set; } 
		public DateTime CurrentDate { get; set; }
		public IEnumerable<DiaryEntryModel> Entries { get; set; }
	}
}