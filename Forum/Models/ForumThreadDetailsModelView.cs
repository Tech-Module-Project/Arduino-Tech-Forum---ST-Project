namespace Forum.Models
{

    using System.Collections.Generic;

    using Forum.Models.Answers;

    public class ForumThreadDetailsModelView
    {
        public ForumThread Thread { get; set; }
        public IList<IAnswer> Answers { get; set; }

    }
}