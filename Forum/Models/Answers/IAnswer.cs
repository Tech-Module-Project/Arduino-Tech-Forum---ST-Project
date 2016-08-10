namespace Forum.Models.Answers
{
    using System;
    using System.Collections.Generic;

    public interface IAnswer
    {
        int Id
        {
            get; set;
        }

        string Body
        {
            get; set;
        }

        int PositivePoints
        {
            get; set;
        }

        int NegativePoints
        {
            get; set;
        }
        
        ForumThread ForumThread
        {
            get; set;
        }

        DateTime? CreationDate
        {
            get; set;
        }

        ICollection<IAnswer> Replies
        {
            get; set;
        }
    }
}
