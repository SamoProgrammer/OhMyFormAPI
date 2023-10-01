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
using System.Text;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;

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
            return Ok(newForms);
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

            return Ok(new FormViewModel()
            {
                Id = form.Id,
                AuthorId = form.Author.Id,
                EndTime = form.EndTime,
                Title = form.Title
            });
        }

        // POST: api/Form
        [HttpPost]
        public async Task<ActionResult<Form>> PostForm(AddFormModel form)
        {
            if (!await _context.Users.AnyAsync(x => x.Username == form.AuthorUsername))
            {
                return BadRequest();
            }
            await _context.Forms.AddAsync(new Form()
            {
                Author = await _context.Users.Where(x => x.Username == form.AuthorUsername).FirstAsync(),
                EndTime = form.EndTime,
                Title = form.Title,
            });
            await _context.SaveChangesAsync();

            return Ok(new { title = form.Title });
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

        [HttpPost("ConvertFormDataToCsv")]
        public async Task<IActionResult> ConvertFormDataToCsv(int formId)
        {
            if (!await _context.Forms.AnyAsync(x => x.Id == formId))
            {
                return NotFound();
            }
            // Sample data for demonstration
            List<List<FormElementValue>> data = new List<List<FormElementValue>>();
            var formElements = await _context.FormElements.Include(x => x.Form).Where(x => x.Form.Id == formId).ToListAsync();
            foreach (var item in formElements)
            {
                data.Add(await _context.Answers.Include(x => x.Element).Include(x => x.AnsweredBy).Where(x => x.Element.Id == item.Id).ToListAsync());
            }
            List<List<FormElementValue>> sortedData = new List<List<FormElementValue>>();

            foreach (var item in data)
            {
                sortedData.Add(item.OrderBy(x => x.AnsweredBy.Id).ToList());
            }
            var csvOutput=new StringBuilder();
            foreach (var item in sortedData)
            {
                csvOutput.AppendLine(item.First().Element.Label);
                foreach (var element in item)
                {
                    csvOutput.AppendLine($"{element.AnsweredBy.Username},{element.Value}");
                }
                csvOutput.AppendLine("--------------------------------------------");
            }
            return Ok(csvOutput);
            // // Convert data to JSON
            // var jsonData = JsonConvert.SerializeObject(data);

            // // Convert JSON to CSV
            // using (var memoryStream = new MemoryStream())
            // using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            // using (var csvWriter = new CsvWriter(streamWriter, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
            // {
            //     csvWriter.WriteRecords(JsonConvert.DeserializeObject<List<List<FormElementValue>>>(jsonData));
            //     streamWriter.Flush();
            //     return File(memoryStream.ToArray(), "text/csv", "data.csv");
            // }
        }
        [HttpPost("IsUserAnswerdForm")]
        public async Task<IActionResult> IsUserAnswerdForm(string username, int formId)
        {
            if (!await _context.Forms.AnyAsync(x => x.Id == formId))
            {
                return NotFound();
            }
            if (!await _context.Users.AnyAsync(x => x.Username == username))
            {
                return NotFound();
            }
            List<FormElement> formFormElements = await _context.FormElements.Include(x => x.Form).Where(x => x.Form.Id == formId).ToListAsync();
            List<FormElementValue> formAnswers = new List<FormElementValue>();
            foreach (var formElement in formFormElements)
            {
                formAnswers.AddRange(await _context.Answers.Include(x => x.Element).Include(x => x.AnsweredBy).Where(x => x.Element.Id == formElement.Id).ToListAsync());
            }
            bool isUserAnswerdForm = formAnswers.Any(x => x.AnsweredBy.Username == username);
            return Ok(isUserAnswerdForm);
        }
        private bool FormExists(int id)
        {
            return _context.Forms.Any(f => f.Id == id);
        }
    }
}
