using BlogProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Blog> Posts { get; set; }
        public DbSet<Blog> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogProject.Models.Comment> Comment { get; set; }
        public DbSet<BlogProject.Models.Post> Post { get; set; }

    }
}
