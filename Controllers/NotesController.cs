using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApi.Data;
using NoteApi.Entity;

namespace NoteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Notes (Kategori ve Başlığa Göre Dinamik Arama)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes([FromQuery] string? baslik, [FromQuery] string? kategori)
        {
            var query = _context.Notes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(baslik))
            {
                query = query.Where(n => n.Baslik.Contains(baslik));
            }

            if (!string.IsNullOrWhiteSpace(kategori))
            {
                query = query.Where(n => n.Kategori.Contains(kategori));
            }

            return Ok(await query.ToListAsync());
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound(new { Message = $"Id değeri {id} olan not bulunamadı." });
            }

            return Ok(note);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Baslik))
            {
                return BadRequest(new { Message = "Not başlığı boş olamaz." });
            }

            note.OlusturmaTarihi = DateTime.UtcNow;
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note updatedNote)
        {
            var existingNote = await _context.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return NotFound(new { Message = $"Id değeri {id} olan not bulunamadı." });
            }

            existingNote.Baslik = updatedNote.Baslik;
            existingNote.Icerik = updatedNote.Icerik;
            existingNote.Kategori = updatedNote.Kategori;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound(new { Message = $"Id değeri {id} olan not bulunamadı." });
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Not başarıyla silindi." });
        }
    }
}
