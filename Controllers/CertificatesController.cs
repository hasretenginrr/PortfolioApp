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
    public class CertificatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CertificatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Certificates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificates>>> GetCertificates()
        {
            return await _context.Certificates.ToListAsync();
        }

        // GET: api/Certificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Certificates>> GetCertificates(int id)
        {
            var certificates = await _context.Certificates.FindAsync(id);

            if (certificates == null)
            {
                return NotFound();
            }

            return certificates;
        }

        // PUT: api/Certificates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ApiKeyAuthorize]
        public async Task<IActionResult> PutCertificates(int id, Certificates certificates)
        {
            if (id != certificates.Id)
            {
                return BadRequest();
            }

            _context.Entry(certificates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificatesExists(id))
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

        // POST: api/Certificates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ApiKeyAuthorize]
        public async Task<ActionResult<Certificates>> PostCertificates(Certificates certificates)
        {
            _context.Certificates.Add(certificates);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCertificates", new { id = certificates.Id }, certificates);
        }

        // DELETE: api/Certificates/5
        [HttpDelete("{id}")]
        [ApiKeyAuthorize]
        public async Task<IActionResult> DeleteCertificates(int id)
        {
            var certificates = await _context.Certificates.FindAsync(id);
            if (certificates == null)
            {
                return NotFound();
            }

            _context.Certificates.Remove(certificates);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificatesExists(int id)
        {
            return _context.Certificates.Any(e => e.Id == id);
        }
    }
}
