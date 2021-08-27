using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Croncher.Models;
using Croncher.Helpers;
using Croncher.Services;

namespace Croncher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly ILinksService _linksService;

        public LinksController(ILinksService linksService)
        {
            _linksService = linksService;
        }

        [HttpGet("{encodedId}")]
        public async Task<ActionResult<string>> GetLink(string encodedId)
        {
            var link = await _linksService.GetLinkAsync(encodedId);

            if (link == null)
            {
                return NotFound();
            }

            return link;
        }

        [HttpPost("{url}")]
        public async Task<ActionResult<string>> PostLink(string url)
        {
            var encodedId = await _linksService.InsertLinkAsync(url);

            return CreatedAtAction(nameof(GetLink), new { encodedId = encodedId }, encodedId);
        }
    }
}
