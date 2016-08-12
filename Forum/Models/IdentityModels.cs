namespace Forum.Models
{

    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Forum.Models.Answers;
    using Microsoft.AspNet.Identity.Owin;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            PostedAnswers = new List<IAnswer>();
            PostedThreads = new List<ForumThread>();
        }

        public int Score
        {
            get;
            set;
        }

        public List<IAnswer> PostedAnswers
        {
            get;
            set;
        }

        public List<ForumThread> PostedThreads
        {
            get;
            set;
        }

        public string DefaultProfilePictureData
        {
            get;
            set;
        }

        public string UploadedProfilePicturePath
        {
            get;
            set;
        }

        private string ConvertImageToBase64(string path)
        {
            using (Image image = Image.FromFile(path, true))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public string GetProfileImageSrc()
        {
            var pictureSrc = "";

            if (!string.IsNullOrEmpty(this.UploadedProfilePicturePath))
            {
                var extension = this.UploadedProfilePicturePath.Split('.')
                    .Last();

                pictureSrc = "data:image/" + extension + ";base64," + ConvertImageToBase64(this.UploadedProfilePicturePath);
            }
            else
            {
                pictureSrc = "data:image/png;base64," + this.DefaultProfilePictureData;
            }

            return pictureSrc;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}