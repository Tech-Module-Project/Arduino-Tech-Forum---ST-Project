namespace Forum.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Tag
    {
        public Tag()
        {
            Threads = new List<ForumThread>();
        }

		[Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public virtual ICollection<ForumThread> Threads
        {
            get;
            set;
        }
    }
}
