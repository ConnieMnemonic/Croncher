using Croncher.Helpers;
using Croncher.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Croncher.Controllers
{
    public class IndexController : Controller
    {
        private ILinksService _linksService;

        public IndexController(ILinksService linksService)
        {
            _linksService = linksService;
        }

        //Home page - Shows the form for getting a new link.
        [HttpGet("{encodedId?}")]
        public async Task<IActionResult> Index(string encodedId)
        {
            if (encodedId == null)
            {
                return View();
            }
            else
            {
                var url = await _linksService.GetLinkAsync(encodedId);
                if(url == null)
                {
                    return NotFound();
                }
                return Redirect(url);
            }
        }
    }
}
