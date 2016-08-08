namespace Forum.Models.Answers
{
    using System.ComponentModel.DataAnnotations;

    public partial class RegisteredUserAnswer : AnswerBase
    {
        [Required]
        public ApplicationUser Author
        {
            get; set;
        }
    }
}
