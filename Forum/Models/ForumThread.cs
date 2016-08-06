namespace Forum.Models
{
    using Answers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class ForumThread 
    {
        public ForumThread()
        {
            Answers = new List<IAnswer>();
            Tags = new List<Tag>();
        }

		[Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [StringLength(200)]
        public string Title
        {
            get; set;
        }

        [Required]
        [StringLength(4000)]
        public string Body
        {
            get; set;
        }

        public ApplicationUser Author
        {
            get; set;
        }

        [Required]
        public Category Category
        {
            get; set;
        }
        
        public DateTime? CreationDate
        {
            get; set;
        }

        public long ViewCount
        {
            get; set;
        }

        public IAnswer BestAnswer
        {
            get; set;
        }

        public List<IAnswer> Answers
        {
            get;
            set;
        }

        public List<Tag> Tags
        {
            get;
            set;
        }
    }
}
