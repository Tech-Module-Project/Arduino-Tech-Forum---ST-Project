namespace Forum.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Forum.Models;
    using Forum.Models.Answers;

    internal sealed class Configuration : DbMigrationsConfiguration<Forum.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Forum.Models.ApplicationDbContext context)
        {

            var user1 = new ApplicationUser()
                        {
                            Email = "email@email.bg",
                            UserName = "email@email.bg",
                            PasswordHash = "password"
                        };

            var user2 = new ApplicationUser()
                        {
                            Email = "gotiniq@email.bg",
                            UserName = "gotiniq@email.bg",
                            PasswordHash = "password"
                        };

            var category1 = new Category()
                           {
                               Name = "Rituals"
                           };

            var category2 = new Category()
                            {
                                Name = "Others"
                            };

            var tag1 = new Tag()
                       {
                           Name = "demon"
                       };

            var tag2 = new Tag()
                       {
                            Name = "ritual"
                       };

            var tag3 = new Tag()
                       {
                            Name = "how"
                       };

            var thread1 = new ForumThread()
                          {
                              Title = "Kak da prizova demon?",
                              Body = "Някой знае ли как да призова демон?Някой знае ли как да призова демон?",
                              Category = category1,
                              Author = user1,
                              ViewCount = 10,
                              Tags = new List<Tag>()
                                     {
                                         tag1,
                                         tag2,
                                         tag3
                                     }
                          };

            var answer1 = new AnonymousUserAnswer()
                          {
                              Body = "Брат пии си хапчетата",
                              Email = "piisihapchetata@abv.bg",
                              ForumThread = thread1,
                              NegativePoints = 10,
                              PositivePoints = 5
                          };

            var answer2 = new RegisteredUserAnswer()
                          {
                              Body =
                                  "1-во купуваш зелени, червени и сини свещи от кауфланд. 2ро превеждаш ми 20$. 3то дочиташ инструкциите който ще ти пратя по скайп",
                              Author = user2,
                              ForumThread = thread1,
                              NegativePoints = 10
                          };

            var nestedAnswer1_1 = new AnonymousUserAnswer()
                                  {
                                        Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus in ultricies mauris, id posuere metus. Sed quis arcu quam. Sed cursus gravida nisi, vitae venenatis odio molestie ac.",
                                        Email = "mishka@abv.bg",
                                        ForumThread = thread1
                                  };

            
            tag1.Threads.Add(thread1);
            tag2.Threads.Add(thread1);
            tag3.Threads.Add(thread1);

            category1.Threads.Add(thread1);

            thread1.Answers.Add(answer1);
            thread1.Answers.Add(answer2);

            answer1.Replies.Add(nestedAnswer1_1);

            context.Users.AddOrUpdate(user1, user2);
            context.Categories.AddOrUpdate(category1, category2);
            context.Threads.AddOrUpdate(thread1);
            context.AnonymousUsersAnswer.AddOrUpdate(answer1, nestedAnswer1_1);
            context.RegisteredUsersAnswer.AddOrUpdate(answer2);
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
