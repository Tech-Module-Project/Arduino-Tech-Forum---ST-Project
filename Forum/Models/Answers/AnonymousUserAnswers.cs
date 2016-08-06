using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Answers
{
    public class AnonymousUserAnswer : AnswerBase
    {
        [Required]
        [StringLength(100)]
        public string Email
        {
            get;
            set;
        }
    }
}