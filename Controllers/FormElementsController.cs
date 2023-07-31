using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormGeneratorAPI.Authentication.Entities;
using FormGeneratorAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormGeneratorAPI.Database;

namespace FormGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormElementController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public FormElementController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FormElement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormElement>>> GetFormElements()
        {
            return await _context.FormElements
                .Include(fe => fe.Form) // Include the related Form
                .ToListAsync();
        }

        // GET: api/FormElement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormElement>> GetFormElement(int id)
        {
            var formElement = await _context.FormElements
                .Include(fe => fe.Form) // Include the related Form
                .FirstOrDefaultAsync(fe => fe.Id == id);

            if (formElement == null)
            {
                return NotFound();
            }

            return formElement;
        }

        // POST: api/FormElement
        [HttpPost]
        public async Task<ActionResult<FormElement>> PostFormElement(FormElement formElement)
        {
            _context.FormElements.Add(formElement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormElement), new { id = formElement.Id }, formElement);
        }

        // PUT: api/FormElement/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormElement(int id, FormElement formElement)
        {
            if (id != formElement.Id)
            {
                return BadRequest();
            }

            _context.Entry(formElement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormElementExists(id))
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

        // DELETE: api/FormElement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormElement(int id)
        {
            var formElement = await _context.FormElements.FindAsync(id);
            if (formElement == null)
            {
                return NotFound();
            }

            _context.FormElements.Remove(formElement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormElementExists(int id)
        {
            return _context.FormElements.Any(fe => fe.Id == id);
        }
    }
}
