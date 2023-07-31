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
    public class FormElementValueController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public FormElementValueController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FormElementValue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormElementValue>>> GetFormElementValues()
        {
            return await _context.Answers
                .Include(fe => fe.Element) // Include the related FormElement
                .Include(fe => fe.AnsweredBy) // Include the related User (Answered By)
                .ToListAsync();
        }

        // GET: api/FormElementValue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormElementValue>> GetFormElementValue(int id)
        {
            var formElementValue = await _context.Answers
                .Include(fe => fe.Element) // Include the related FormElement
                .Include(fe => fe.AnsweredBy) // Include the related User (Answered By)
                .FirstOrDefaultAsync(fe => fe.Id == id);

            if (formElementValue == null)
            {
                return NotFound();
            }

            return formElementValue;
        }

        // POST: api/FormElementValue
        [HttpPost]
        public async Task<ActionResult<FormElementValue>> PostFormElementValue(FormElementValue formElementValue)
        {
            _context.Answers.Add(formElementValue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormElementValue), new { id = formElementValue.Id }, formElementValue);
        }

        // PUT: api/FormElementValue/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormElementValue(int id, FormElementValue formElementValue)
        {
            if (id != formElementValue.Id)
            {
                return BadRequest();
            }

            _context.Entry(formElementValue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormElementValueExists(id))
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

        // DELETE: api/FormElementValue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormElementValue(int id)
        {
            var formElementValue = await _context.Answers.FindAsync(id);
            if (formElementValue == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(formElementValue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormElementValueExists(int id)
        {
            return _context.Answers.Any(fe => fe.Id == id);
        }
    }
}
