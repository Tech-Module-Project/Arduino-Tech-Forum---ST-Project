namespace Forum.Models.Answers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class AnswerBase : IAnswer
    {
        protected AnswerBase()
        {
            Replies = new List<AnswerBase>();
            CreationDate = DateTime.Now;
        }

		[Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [StringLength(4000)]
        public string Body
        {
            get; set;
        }

        public int PositivePoints
        {
            get; set;
        }

        public int NegativePoints
        {
            get; set;
        }

        [Required]
        public ForumThread ForumThread
        {
            get; set;
        }

        public DateTime? CreationDate
        {
            get; set;
        }

        public virtual ICollection<AnswerBase> Replies
        {
            get; set;
        }

        public AnswerBase ParentAnswer
        {
            get; set;
        }
    }
}