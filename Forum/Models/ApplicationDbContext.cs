﻿namespace Forum.Models
{

    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    using Forum.Models.Answers;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            var dbcontext = new ApplicationDbContext();            
            return dbcontext;
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public virtual DbSet<Category> Categories
        {
            get; set;
        }

        public virtual DbSet<ForumThread> Threads
        {
            get; set;
        }

        public virtual DbSet<Tag> Tags
        {
            get; set;
        }

        public virtual DbSet<RegisteredUserAnswer> RegisteredUsersAnswer
        {
            get; set;
        }

        public virtual DbSet<AnonymousUserAnswer> AnonymousUsersAnswer
        {
            get; set;
        }
    }
}