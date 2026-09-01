using AutoMapper;
using INFOVUALT.Data;
using INFOVUALT.DTOs;
using INFOVUALT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace INFOVUALT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public NotesController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet("folder/{folderId}")]
        public IActionResult GetNotesInFolder(int folderId)
        {
            var folder = _db.Folders.FirstOrDefault(f =>
                f.Id == folderId &&
                f.UserId == GetUserId());

            if (folder == null)
                return NotFound("Folder not found.");

            var notes = _db.Notes
                .Where(n => n.FolderId == folderId)
                .ToList();

            return Ok(notes);
        }

        [HttpPost]
        public IActionResult CreateNote(CreateNoteDto dto)
        {
            var folder = _db.Folders.FirstOrDefault(f =>
                f.Id == dto.FolderId &&
                f.UserId == GetUserId());

            if (folder == null)
                return BadRequest("Invalid folder.");

            var note = _mapper.Map<Note>(dto);

            _db.Notes.Add(note);
            _db.SaveChanges();

            return Ok(note);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, UpdateNoteDto dto)
        {
            var note = _db.Notes
                .Include(n => n.Folder)
                .FirstOrDefault(n =>
                    n.Id == id &&
                    n.Folder!.UserId == GetUserId());

            if (note == null)
                return NotFound();

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var note = _db.Notes
                .Include(n => n.Folder)
                .FirstOrDefault(n =>
                    n.Id == id &&
                    n.Folder!.UserId == GetUserId());

            if (note == null)
                return NotFound();

            _db.Notes.Remove(note);
            _db.SaveChanges();

            return Ok("Deleted.");
        }
    }
}