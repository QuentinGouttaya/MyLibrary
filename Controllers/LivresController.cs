using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("livres")]
    public class LivresController : ControllerBase
    {
        private readonly MediaDb _context;

        public LivresController(MediaDb context)
        {
            _context = context;
        }

        // GET: /livres
        [HttpGet]
        public async Task<IActionResult> GetAllLivres()
        {
            var livres = await _context.Medias.ToListAsync();
            return Ok(livres);
        }

        // GET: /livres?start=xxx&end=yyy
        [HttpGet("filter")]
        public async Task<IActionResult> GetLivresFilterAuthorTitle(
            [FromQuery] string? start,
            [FromQuery] string? end)
        {
            var query = _context.Medias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(start))
            {
                string startLower = start.ToLower();
                query = query.Where(l =>
                    EF.Functions.Like(l.Title, startLower + "%") ||
                    EF.Functions.Like(l.Author, startLower + "%"));
            }

            if (!string.IsNullOrWhiteSpace(end))
            {
                string endLower = end.ToLower();
                query = query.Where(l =>
                    EF.Functions.Like(l.Title, "%" + endLower) ||
                    EF.Functions.Like(l.Author, "%" + endLower));
            }

            var resultats = await query
                .Select(x => new
                {
                    x.Id,
                    Title = x.Title.ToLower(),
                    Author = x.Author.ToLower()
                })
                .ToListAsync();

            return Ok(resultats);
        }
    

        // GET: /livres/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLivre(int id)
        {
            var livre = await _context.Medias.FindAsync(id);

            if (livre == null)
            {
                return NotFound(new { Message = "Livre non trouvé." });
            }

            return Ok(livre.DisplayInformation());
        }

        // POST: /livres
        [HttpPost]
        public async Task<IActionResult> AddLivre([FromBody] Media newLivre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Medias.Add(newLivre);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetLivre),
                    new { id = newLivre.Id },
                    new { Message = "Livre ajouté avec succès", Data = newLivre }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erreur lors de l'ajout du livre.", Error = ex.Message });
            }
        }

        // PUT: /livres/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLivre(int id, [FromBody] Media updatedLivre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLivre = await _context.Medias.FindAsync(id);

            if (existingLivre == null)
            {
                return NotFound(new { Message = "Livre non trouvé." });
            }


            _context.Entry(existingLivre).CurrentValues.SetValues(updatedLivre);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Livre mis à jour avec succès" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erreur lors de la mise à jour du livre.", Error = ex.Message });
            }
        }

        // DELETE: /livres/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivre(int id)
        {
            var livre = await _context.Medias.FindAsync(id);

            if (livre == null)
            {
                return NotFound(new { Message = "Livre non trouvé." });
            }

            try
            {
                _context.Medias.Remove(livre);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Livre supprimé avec succès" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erreur lors de la suppression du livre.", Error = ex.Message });
            }
        }
    }
}