namespace Forum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Threads_Tags
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ThreadId { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Thread Thread { get; set; }
    }
}
