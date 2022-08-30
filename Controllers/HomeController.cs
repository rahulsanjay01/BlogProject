using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using BlogProject.ViewModels;
using X.PagedList;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;

            //var blogs = _context.Blogs.Where(
            //    b => b.Posts.Any(p => p.ReadyStatus == Enums.ReadyStatus.ProductionReady))
            //    .OrderByDescending(b => b.Created)
            //    .Include(b => b.BlogUser)
            //    .ToPagedListAsync(pageNumber, pageSize);

            var blogs = _context.Blogs
                .OrderByDescending(b => b.Created)
                .Include(b => b.BlogUser)
                .ToPagedListAsync(pageNumber, pageSize);

            ViewData["HeaderImage"] = "/images/home-bg.jpg";
            ViewData["MainText"] = "Rahul's Blog";
            ViewData["SubText"] = "Coding & Life";

            return View(await blogs);
        }

        public IActionResult About()
        {
            ViewData["HeaderImage"] = "/images/home-bg.jpg";
            ViewData["MainText"] = "Rahul's Blog";
            ViewData["SubText"] = "Coding & Life";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["HeaderImage"] = "/images/home-bg.jpg";
            ViewData["MainText"] = "Rahul's Blog";
            ViewData["SubText"] = "Coding & Life";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            //This is where we willl be emailing...
            model.Message = $"{model.Message} <hr> Phone: {model.Phone}";
            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
