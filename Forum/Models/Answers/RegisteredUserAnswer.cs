using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models.Answers
{
    using System.ComponentModel.DataAnnotations;

    public partial class RegisteredUserAnswer : AnswerBase
    {
        public string Author_Id { get; set; }

        [ForeignKey("Author_Id")]
        public ApplicationUser Author
        {
            get; set;
        }

    }
}
