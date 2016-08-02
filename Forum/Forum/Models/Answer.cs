namespace Forum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }

        public int? PositivePoints { get; set; }

        public int? NegativePoints { get; set; }

        [StringLength(128)]
        public string AuthorId { get; set; }

        public int ThreadId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Thread Thread { get; set; }
    }
}
