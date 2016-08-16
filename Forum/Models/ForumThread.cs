namespace Forum.Models
{
    using Answers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class ForumThread 
    {
        public const int MinTitleLength = 4;

        public const int MinBodyLength = 10;

        public ForumThread()
        {
            Answers = new List<AnswerBase>();
            Tags = new List<Tag>();
            CreationDate = DateTime.Now;
        }

		[Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [StringLength(200, MinimumLength = MinTitleLength)]
        public string Title
        {
            get; set;
        }

        [Required]
        [StringLength(4000, MinimumLength = MinBodyLength)]
        [DataType(DataType.MultilineText)]
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

        public DateTime? LastModified { get; set; }

        public long ViewCount
        {
            get; set;
        }

        public AnswerBase BestAnswer
        {
            get; set;
        }

        public virtual ICollection<AnswerBase> Answers
        {
            get;
            set;
        }

        public virtual ICollection<Tag> Tags
        {
            get;
            set;
        }
    }
}
