using CustomerServiceChatAPI.Auth;
using CustomerServiceChatAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Service.ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Service.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Service.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] Category model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Validate the model
            }

            // Find the existing category using the ID from the URL
            var existingCategory = await _context.Service.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {id} not found."); // If not found, return 404
            }

            // Update the Name of the existing category
            existingCategory.Service = model.Service;

            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
                return Ok(existingCategory); // Return the updated category
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the category still exists after catching concurrency exception
                if (!CategoryExists(id))
                {
                    return NotFound($"Category with ID {id} not found during save.");
                }
                else
                {
                    throw; // Re-throw the exception if something else went wrong
                }
            }
        }



        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Service.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Service.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Service.Any(e => e.Id == id);
        }
    }
}
