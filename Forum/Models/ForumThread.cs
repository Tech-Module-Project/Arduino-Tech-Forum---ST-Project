using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(200)]
        public string Title
        {
            get; set;
        }

        [Required]
        [StringLength(4000)]
        [DataType(DataType.MultilineText)]
        public string Body
        {
            get; set;
        }

        public string Author_Id { get; set; }

        [ForeignKey("Author_Id")]
        public ApplicationUser Author
        {
            get; set;
        }

        public int Category_Id { get; set; }

        [ForeignKey("Category_Id")]
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
