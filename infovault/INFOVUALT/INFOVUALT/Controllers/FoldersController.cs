using AutoMapper;
using INFOVUALT.Data;
using INFOVUALT.DTOs;
using INFOVUALT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INFOVUALT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FoldersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public FoldersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public IActionResult GetFolders()
        {
            var userId = GetUserId();

            var folders = _db.Folders
                             .Where(f => f.UserId == userId)
                             .ToList();

            return Ok(folders);
        }

        [HttpPost]
        public IActionResult CreateFolder(CreateFolderDto dto)
        {
            var folder = _mapper.Map<Folder>(dto);

            folder.UserId = GetUserId();

            _db.Folders.Add(folder);
            _db.SaveChanges();

            return Ok(folder);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFolder(int id)
        {
            var folder = _db.Folders.FirstOrDefault(f =>
                f.Id == id &&
                f.UserId == GetUserId());

            if (folder == null)
                return NotFound();

            _db.Folders.Remove(folder);
            _db.SaveChanges();

            return Ok("Deleted.");
        }
    }
}