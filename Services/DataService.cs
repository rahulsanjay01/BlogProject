using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogProject.Data;
using BlogProject.Enums;
using BlogProject.Models;

namespace BlogProject.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        //Constructor Injection
        public DataService(ApplicationDbContext dbContext,
                            RoleManager<IdentityRole> roleManager,
                            UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //wrapper
        public async Task ManageDataAsync()
        {
            //Task: Create the DB from the Migrations
            await _dbContext.Database.MigrateAsync();

            //Task 1: Seed a few roles into the system
            await SeedRolesAsync();

            //Task 2: Seed a few users into the system
            await SeedUsersAsync();

        }

        private async Task SeedRolesAsync()
        {
            //if there are already roles in the system, do nothing
            if (_dbContext.Roles.Any())
            {
                return;
            }
            //Otherwise, we want to create a few roles
            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                //need to use Role Manager to create roles
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            //if there are already users in the system, do nothing
            if (_dbContext.Users.Any())
            {
                return;
            }

            //Step 1: Creates a new instance of BlogUser
            var adminUser = new BlogUser()
            {
                Email = "a.rahul.sanjay@gmail.com",
                UserName = "a.rahul.sanjay@gmail.com",
                FirstName = "Rahul",
                LastName = "Ahir",
                DisplayName = "rahulahir",
                PhoneNumber = "(800) 555-1212",
                EmailConfirmed = true
            };

            //Step 2: Use the UserManager to create a new user that is defined by adminUser
            await _userManager.CreateAsync(adminUser, "Abc&123!");

            //Step 3: Add this new user to the Administrator role
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());

            //Step 1 Repeat: Create the moderator user
            var modUser = new BlogUser()
            {
                Email = "Visitor@gmail.com",
                UserName = "Visitor@gmail.com",
                FirstName = "Visitor",
                LastName = "Guest",
                DisplayName = "Guest Visitor",
                PhoneNumber = "(800) 555-1111",
                EmailConfirmed = true
            };

            //Step 2 Repeat: Use the UserManager to create a new user that is defined by adminUser
            await _userManager.CreateAsync(modUser, "Abc&123!");

            //Step 3 Repeat: Add this new user to the Administrator role
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());

        }
    }
}
