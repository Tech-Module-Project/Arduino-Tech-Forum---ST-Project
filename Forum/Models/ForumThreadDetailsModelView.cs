namespace Forum.Models
{

    using System.Collections.Generic;

    public class ForumThreadDetailsModelView
    {
        public ForumThread Thread { get; set; }
        public List<Answers.IAnswer> Answers { get; set; }

    }
}