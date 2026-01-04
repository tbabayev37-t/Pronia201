using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;

namespace MVCProniaTask.Areas.Admin.Controllers;
[Area("Admin")]
public class TagController(AppDbContext _context) : Controller
{
    public IActionResult Index()
    {
        var tags = _context.Tags.ToList();
        return View(tags);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Tag tag)
    {
        if (!ModelState.IsValid)
        {
            return View(tag);
        }
        _context.Tags.Add(tag);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Delete(int id)
    {
        var tags = _context.Tags.Find(id);
        if(tags is null)
        {
            return NotFound();      
        }
        _context.Tags.Remove(tags);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Update(int id)
    {
        var tag = _context.Tags.Find(id);
        if (tag is null)
        {
            return NotFound();
        }
        return View(tag);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(Tag tag)
    {
        if (!ModelState.IsValid)
        {
            return View(tag);
        }
        var existTag = _context.Tags.Find(tag.Id);
        if(existTag is null)
        {
            return NotFound();
        }
        existTag.name = tag.name;
        _context.Tags.Update(existTag);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));

    }

}
