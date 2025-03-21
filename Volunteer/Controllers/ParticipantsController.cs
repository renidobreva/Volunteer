using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Volunteer.Data;

public class ParticipantsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ParticipantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Participants
    public IActionResult Index(int projectId)
    {
        var participants = _context.Participants.Where(p => p.ProjectId == projectId).ToList();
        return View(participants);
    }

    // GET: Participants/Add/5
    public IActionResult Add(int projectId)
    {
        var model = new Participant
        {
            ProjectId = projectId
        };
        return View(model);
    }

    // POST: Participants/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind("ProjectId,UserId")] Participant participant)
    {
        if (ModelState.IsValid)
        {
            _context.Add(participant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = participant.ProjectId });
        }
        return View(participant);
    }

    // GET: Participants/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var participant = _context.Participants
            .FirstOrDefault(m => m.Id == id);
        if (participant == null)
        {
            return NotFound();
        }

        return View(participant);
    }

    // POST: Participants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { projectId = participant.ProjectId });
    }
}
