using AspireTesting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Saritasa.Tools.EntityFrameworkCore;

namespace AspireTesting.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public NotesController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet, EndpointName("GetAllNotes")]
    public async Task<IEnumerable<Note>> GetNotes()
    {
        return await dbContext.Notes.ToListAsync();
    }

    [HttpGet("id"), EndpointName("GetNoteById")]
    public async Task<Note> GetNote(int id)
    {
        return await dbContext.Notes.GetAsync(note => note.Id == id);
    }

    [HttpPost, EndpointName("CreateNote")]
    public async Task<int> AddNote([FromBody] Note note)
    {
        note.CreatedAt = DateTime.UtcNow;
        dbContext.Notes.Add(note);
        await dbContext.SaveChangesAsync();

        return note.Id;
    }
}
