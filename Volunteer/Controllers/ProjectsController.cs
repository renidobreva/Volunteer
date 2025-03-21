using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context) => _context = context;

    // GET: Projects
    public async Task<IActionResult> Index(string searchString) =>
        View(await _context.Projects
                           .Where(p => string.IsNullOrEmpty(searchString) || p.Name.Contains(searchString) || p.Description.Contains(searchString))
                           .ToListAsync());

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(int? id) =>
        id == null ? NotFound() : View(await _context.Projects.Include(p => p.Participants).FirstOrDefaultAsync(m => m.Id == id));

    // GET: Projects/Create
    public IActionResult Create() => View();

    // POST: Projects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Project project)
    {
        if (ModelState.IsValid)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    // GET: Projects/Edit/5
    public async Task<IActionResult> Edit(int? id) =>
        id == null ? NotFound() : View(await _context.Projects.FindAsync(id));

    // POST: Projects/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Project project)
    {
        if (id != project.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projects.Any(p => p.Id == project.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    // GET: Projects/Delete/5
    public async Task<IActionResult> Delete(int? id) =>
        id == null ? NotFound() : View(await _context.Projects.FirstOrDefaultAsync(m => m.Id == id));

    // POST: Projects/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
