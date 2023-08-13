using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormGeneratorAPI.Authentication.Entities;
using FormGeneratorAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormGeneratorAPI.Database;
using FormGeneratorAPI.DTOs.Form;

namespace FormGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public FormController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Form
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormViewModel>>> GetForms()
        {
            var forms = await _context.Forms
                .Include(f => f.Author) // Include the related User (Author)
                .ToListAsync();
            List<FormViewModel> newForms = new List<FormViewModel>();
            foreach (var form in forms)
            {
                newForms.Add(new FormViewModel()
                {
                    Id = form.Id,
                    AuthorId = form.Author.Id,
                    EndTime = form.EndTime,
                    Title = form.Title
                });
            }
            return newForms;
        }

        // GET: api/Form/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormViewModel>> GetForm(int id)
        {
            var form = await _context.Forms
                .Include(f => f.Author) // Include the related User (Author)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null)
            {
                return NotFound();
            }

            return new FormViewModel()
            {
                Id = form.Id,
                AuthorId = form.Author.Id,
                EndTime = form.EndTime,
                Title = form.Title
            };
        }

        // POST: api/Form
        [HttpPost]
        public async Task<ActionResult<Form>> PostForm(AddFormModel form)
        {
            if (!await _context.Users.AnyAsync(x => x.Id == form.AuthorId))
            {
                return BadRequest();
            }
            await _context.Forms.AddAsync(new Form()
            {
                Author = await _context.Users.FindAsync(form.AuthorId),
                EndTime = form.EndTime,
                Title = form.Title,
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        // PUT: api/Form/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForm(int id, Form form)
        {
            if (id != form.Id)
            {
                return BadRequest();
            }

            _context.Entry(form).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(id))
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

        // DELETE: api/Form/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var form = await _context.Forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormExists(int id)
        {
            return _context.Forms.Any(f => f.Id == id);
        }
    }
}
