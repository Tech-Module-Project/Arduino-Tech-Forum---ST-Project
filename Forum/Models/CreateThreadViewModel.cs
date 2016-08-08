using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class CreateThreadViewModel
    {
        public string Title
        {
            get; set;
        }
        
        public string Body
        {
            get; set;
        }

        public int CategoryId
        {
            get; set;
        }
    }
}