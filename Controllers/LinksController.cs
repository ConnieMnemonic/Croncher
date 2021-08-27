using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Croncher.Models;
using Croncher.Helpers;

namespace Croncher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly LinkContext _context;

        public LinksController(LinkContext context)
        {
            _context = context;
        }

        // GET: api/Links/5
        [HttpGet("{encodedId}")]
        public async Task<ActionResult<string>> GetLink(string encodedId)
        {
            int id = IntBaseConverter.Base62ToBase10(encodedId);

            var link = await _context.Links.FindAsync(id);

            if (link == null)
            {
                return NotFound();
            }

            return link.Url;
        }

        // POST: api/Links
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{url}")]
        public async Task<ActionResult<string>> PostLink(string url)
        {
            Link link = new Link() { Url = url };

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            var convertedId = IntBaseConverter.Base10ToBase62(link.Id);

            return CreatedAtAction(nameof(GetLink), new { encodedId = convertedId }, convertedId);
        }

        private bool LinkExists(int id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}
