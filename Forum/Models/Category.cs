namespace Forum.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Data;

	public partial class Category
	{
		public Category()
		{
			Threads = new List<ForumThread>();
		}

		[Key]
		public int Id
		{
			get; set;
		}

		[Required]
		[StringLength(100)]
		public string Name
		{
			get; set;
		}

		public List<ForumThread> Threads
		{
			get; set;
		}
	}
}
