using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Data;
using PortfolioBackend.Entities;
using PortfolioBackend.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperiencesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExperiencesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Experiences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Experiences>>> GetExperiences()
        {
            return await _context.Experiences.ToListAsync();
        }

        // GET: api/Experiences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Experiences>> GetExperiences(int id)
        {
            var experiences = await _context.Experiences.FindAsync(id);

            if (experiences == null)
            {
                return NotFound();
            }

            return experiences;
        }

        // PUT: api/Experiences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ApiKeyAuthorize]
        public async Task<IActionResult> PutExperiences(int id, Experiences experiences)
        {
            if (id != experiences.Id)
            {
                return BadRequest();
            }

            _context.Entry(experiences).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperiencesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Experiences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ApiKeyAuthorize]
        public async Task<ActionResult<Experiences>> PostExperiences(Experiences experiences)
        {
            _context.Experiences.Add(experiences);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExperiences", new { id = experiences.Id }, experiences);
        }

        // DELETE: api/Experiences/5
        [HttpDelete("{id}")]
        [ApiKeyAuthorize]
        public async Task<IActionResult> DeleteExperiences(int id)
        {
            var experiences = await _context.Experiences.FindAsync(id);
            if (experiences == null)
            {
                return NotFound();
            }

            _context.Experiences.Remove(experiences);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExperiencesExists(int id)
        {
            return _context.Experiences.Any(e => e.Id == id);
        }
    }
}
