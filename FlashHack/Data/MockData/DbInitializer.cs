using FlashHack.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlashHack.Data.MockData
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.User.AddRange(
                    new User
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@admin.com",
                        Password = "hahaha",
                        Bio = "Admin",
                        PhoneNumber = "07000000000",
                        Employer = "FlashHack",
                        Signature = "Admin",
                        ProfilePicURL = "https://as2.ftcdn.net/v2/jpg/04/75/00/99/1000_F_475009987_zwsk4c77x3cTpcI3W1C1LU4pOSyPKaqi.jpg",
                        IsPremium = true,
                        IsAdmin = true,
                        ShowEmail = false,
                        ShowPhoneNumber = false,
                        ShowToRecruiter = false,
                        Skills = new List<Skill>()
                    },
                    new User
                    {
                        FirstName = "Alice",
                        LastName = "Andersson",
                        Email = "alice@example.com",
                        Password = "hahaha",
                        Bio = "Software developer with a passion for AI and backend development.",
                        PhoneNumber = "07000000000",
                        Employer = "Google",
                        ProfilePicURL = "https://images.pexels.com/photos/445109/pexels-photo-445109.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                        Signature = "Keep coding!",
                        IsPremium = true,
                        IsAdmin = false,
                        ShowEmail = true,
                        ShowPhoneNumber = false,
                        ShowToRecruiter = true,
                    },
                    new User
                    {
                        FirstName = "Bob",
                        LastName = "Bengtsson",
                        Email = "bob@example.com",
                        Password = "hahaha",
                        Bio = "Frontend enthusiast and UI/UX designer.",
                        PhoneNumber = "07000000000",
                        Employer = "Google",
                        ProfilePicURL = "https://images.pexels.com/photos/39866/entrepreneur-startup-start-up-man-39866.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                        Signature = "Keep coding!",
                        IsPremium = false,
                        IsAdmin = false,
                        ShowEmail = true,
                        ShowPhoneNumber = true,
                        ShowToRecruiter = true,
                    },
                    new User
                    {
                        FirstName = "Charlie",
                        LastName = "Carlsson",
                        Email = "charlie@example.com",
                        Password = "hahaha",
                        Bio = "Fullstack developer with experience in cloud computing.",
                        PhoneNumber = "07000000000",
                        Employer = "Google",
                        ProfilePicURL = "https://images.pexels.com/photos/845457/pexels-photo-845457.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                        Signature = "Keep coding!",
                        IsPremium = true,
                        IsAdmin = false,
                        ShowEmail = true,
                        ShowPhoneNumber = true,
                        ShowToRecruiter = true,
                    },
                    new User
                    {
                        FirstName = "Dick",
                        LastName = "Donalnds",
                        Email = "Dick@example.com",
                        Password = "hahaha",
                        Bio = "Fullstack developer with experience in cloud computing.",
                        PhoneNumber = "07000000000",
                        Employer = "Google",
                        ProfilePicURL = "https://images.pexels.com/photos/307847/pexels-photo-307847.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                        Signature = "Keep coding!",
                        IsPremium = true,
                        IsAdmin = false,
                        ShowEmail = true,
                        ShowPhoneNumber = true,
                        ShowToRecruiter = true,
                    }               
                );
                context.SaveChanges();

                var alice = context.User.FirstOrDefault(u => u.Email == "alice@example.com");
                var bob = context.User.FirstOrDefault(u => u.Email == "bob@example.com");
                var charlie = context.User.FirstOrDefault(u => u.Email == "charlie@example.com");
                var dick = context.User.FirstOrDefault(u => u.Email == "dick@example.com");

                context.Skill.AddRange(
                
                    new Skill { UserId = alice.Id, SkillName = "C#", SkillRating = 5, SkillDescription ="Jag är bra" },
                    new Skill { UserId = alice.Id, SkillName = "ASP.NET Core", SkillRating = 4, SkillDescription ="Jag är bra" },
                    new Skill { UserId = alice.Id, SkillName = "SQL", SkillRating = 3, SkillDescription ="Jag är bra" },

                    new Skill { UserId = bob.Id, SkillName = "JavaScript", SkillRating = 5, SkillDescription ="Jag är bra" },
                    new Skill { UserId = bob.Id, SkillName = "React", SkillRating = 4, SkillDescription ="Jag är bra" },
                    new Skill { UserId = bob.Id, SkillName = "UI/UX Design", SkillRating = 3, SkillDescription ="Jag är bra" },

                    new Skill { UserId = charlie.Id, SkillName = "C#", SkillRating = 4, SkillDescription ="Jag är bra" },
                    new Skill { UserId = charlie.Id, SkillName = "ASP.NET Core", SkillRating = 4, SkillDescription ="Jag är bra" },
                    new Skill { UserId = charlie.Id, SkillName = "SQL", SkillRating = 5, SkillDescription ="Jag är bra" },
                    new Skill { UserId = charlie.Id, SkillName = "Machine Learning", SkillRating = 3, SkillDescription ="Jag är bra" },

                    new Skill { UserId = dick.Id, SkillName = "JavaScript", SkillRating = 4, SkillDescription ="Jag är bra" },
                    new Skill { UserId = dick.Id, SkillName = "React", SkillRating = 3, SkillDescription ="Jag är bra" },
                    new Skill { UserId = dick.Id, SkillName = "UI/UX Design", SkillRating = 5, SkillDescription ="Jag är bra" }                
                );
                context.SaveChanges();

                context.HeadCategory.AddRange(
                
                    new HeadCategory { Name = "Programming" },
                    new HeadCategory { Name = "Design" },
                    new HeadCategory { Name = "DevOps" },
                    new HeadCategory { Name = "Cybersecurity" }

                );
                context.SaveChanges();

                context.SubCategory.AddRange(

                    new SubCategory { Name = "Web Development", HeadCategoryId = 1 },
                    new SubCategory { Name = "Mobile Development", HeadCategoryId = 1 },
                    new SubCategory { Name = "Game Development", HeadCategoryId = 1 },

                    new SubCategory { Name = "UI/UX", HeadCategoryId = 2 },
                    new SubCategory { Name = "Graphic Design", HeadCategoryId = 2 },

                    new SubCategory { Name = "Cloud Computing", HeadCategoryId = 3 },
                    new SubCategory { Name = "CI/CD", HeadCategoryId = 3 },

                    new SubCategory { Name = "Network Security", HeadCategoryId = 4 },
                    new SubCategory { Name = "Web Security", HeadCategoryId = 4 }                
                );
                context.SaveChanges();

                context.Company.AddRange(               
                    new Company { Name = "Google", Description = "Google is a multinational technology company that specializes in Internet-related services and products, which include online advertising technologies, a search engine, cloud computing, software, and hardware.", Email = "google@google.com", Location = "Not Found", WebbPage = "Not Found" },
                    new Company { Name = "Microsoft", Description = "Microsoft Corporation is an American multinational technology company with headquarters in Redmond, Washington. It develops, manufactures, licenses, supports, and sells computer software, consumer electronics, personal computers, and related services.", Email = "microsoft@microsoft.com", Location = "Not Found", WebbPage = "Not Found" },
                    new Company { Name = "Facebook", Description = "Facebook, Inc. is an American online social media and social networking service company based in Menlo Park, California. Its website was launched on February 4, 2004, by Mark Zuckerberg, along with fellow Harvard College students and roommates Eduardo Saverin, Andrew McCollum, Dustin Moskovitz, and Chris Hughes.", Email = "faceboo@facebook.com", Location = "Not Found", WebbPage = "Not Found" },
                    new Company { Name = "Amazon", Description = "Amazon.com, Inc., is an American multinational technology company based in Seattle, Washington, that focuses on e-commerce, cloud computing, digital streaming, and artificial intelligence.", Email = "amazon@amazon.com", Location = "Not Found", WebbPage = "Not Found" }               
                );
                context.SaveChanges();

                context.Jobblisting.AddRange(               
                    new Jobblisting
                    {
                        Title = "Fullstack Developer",
                        Content = "We are looking for a Fullstack Developer with experience in .NET and React. Join our team and work on exciting projects!",
                        TimeCreated = DateTime.UtcNow.AddDays(-1),
                        CompanyId = context.Company.First(c => c.Name == "Google").Id
                    },
                    new Jobblisting
                    {
                        Title = "Frontend Developer",
                        Content = "Seeking a talented frontend developer skilled in Angular and TypeScript. Remote-friendly position.",
                        TimeCreated = DateTime.UtcNow.AddDays(-1),
                        CompanyId = context.Company.First(c => c.Name == "Microsoft").Id
                    },
                    new Jobblisting
                    {
                        Title = "Backend Engineer",
                        Content = "Looking for a backend engineer with expertise in ASP.NET Core and database optimization.",
                        TimeCreated = DateTime.UtcNow.AddDays(-1),
                        CompanyId = context.Company.First(c => c.Name == "Facebook").Id
                    },
                    new Jobblisting
                    {
                        Title = "UI/UX Designer",
                        Content = "We need a creative UI/UX designer to help improve our app's user experience.",
                        TimeCreated = DateTime.UtcNow.AddDays(-1),
                        CompanyId = context.Company.First(c => c.Name == "Amazon").Id
                    },
                    new Jobblisting
                    {
                        Title = "DevOps Engineer",
                        Content = "Hiring a DevOps Engineer with experience in CI/CD, Kubernetes, and cloud services.",
                        TimeCreated = DateTime.UtcNow.AddDays(-1),
                        CompanyId = context.Company.First(c => c.Name == "Amazon").Id
                    }               
                );
                context.SaveChanges();


                context.Post.AddRange(
                
                    new Post
                    {
                        Title = "How to optimize .NET Core performance?",
                        Content = "Looking for best practices to improve performance in a .NET Core application. Any suggestions?",
                        TimeCreated = DateTime.UtcNow.AddDays(-5),
                        UserId = alice.Id, // Alice
                        SubCategoryId = 1,
                    },
                    new Post
                    {
                        Title = "Best frontend framework in 2025?",
                        Content = "React, Angular, Vue, or something new? What's the best frontend framework this year?",
                        TimeCreated = DateTime.UtcNow.AddDays(-3),
                        UserId = bob.Id, // Bob
                        SubCategoryId = 2,
                    },
                    new Post
                    {
                        Title = "ASP.NET Identity vs custom authentication?",
                        Content = "Should I use ASP.NET Identity for user authentication, or is it better to roll my own?",
                        TimeCreated = DateTime.UtcNow.AddDays(-7),
                        UserId = charlie.Id, // Charlie
                        SubCategoryId = 3,
                    },
                    new Post
                    {
                        Title = "Cloud deployment: AWS vs Azure?",
                        Content = "Which cloud provider is better for .NET applications, AWS or Azure?",
                        TimeCreated = DateTime.UtcNow.AddDays(-2),
                        UserId = dick.Id, // Dick
                        SubCategoryId = 4,
                    }
                
                );
                context.SaveChanges();

                var post1 = context.Post.FirstOrDefault(p => p.Title == "How to optimize .NET Core performance?");
                var post2 = context.Post.FirstOrDefault(p => p.Title == "Best frontend framework in 2025?");
                var post3 = context.Post.FirstOrDefault(p => p.Title == "ASP.NET Identity vs custom authentication?");
                var post4 = context.Post.FirstOrDefault(p => p.Title == "Cloud deployment: AWS vs Azure?");

                context.Comment.AddRange(                
                    new Comment
                    {
                        Content = "I recommend using caching strategies like MemoryCache or Redis for performance optimization.",
                        TimeCreated = DateTime.UtcNow.AddDays(-4),
                        UserId = bob.Id, // Bob
                        PostId = post1.Id // Post om .NET Core performance
                    },
                    new Comment
                    {
                        Content = "React is still going strong, but keep an eye on SolidJS!",
                        TimeCreated = DateTime.UtcNow.AddDays(-2),
                        UserId = charlie.Id, // Charlie
                        PostId = post2.Id // Post om frontend framework
                    },
                    new Comment
                    {
                        Content = "ASP.NET Identity has built-in security features that are hard to replicate safely.",
                        TimeCreated = DateTime.UtcNow.AddDays(-6),
                        UserId = dick.Id, // Dick
                        PostId = post3.Id // Post om ASP.NET Identity
                    },
                    new Comment
                    {
                        Content = "Azure has better integration with .NET, but AWS has broader cloud adoption.",
                        TimeCreated = DateTime.UtcNow.AddDays(-1),
                        UserId = alice.Id, // Alice
                        PostId = post4.Id // Post om AWS vs Azure
                    }            
                );
                context.SaveChanges();
            }
        }
    }
}
