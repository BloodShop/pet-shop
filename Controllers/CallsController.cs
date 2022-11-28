using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;
using PetShopProj.Hubs;
using PetShopProj.Models;

namespace PetShopProj.Controllers
{
    public class CallsController : Controller
    {
        readonly PetDbContext _ctx;
        readonly IHubContext<CallCenterHub, ICallCenterHub> _signalrHub;

        public CallsController(PetDbContext context,
            IHubContext<CallCenterHub, ICallCenterHub> signalrHub)
        {
            _signalrHub = signalrHub;
            _ctx = context;
        }

        // GET: Calls
        public async Task<IActionResult> Index() => View(await _ctx.Calls.ToListAsync());

        [HttpGet]
        public IActionResult GetCalls()
        {
            var res = _ctx.Calls.ToList();
            return Ok(res);
        }

        // GET: Calls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _ctx.Calls == null)
                return NotFound();

            var call = await _ctx.Calls.FirstOrDefaultAsync(m => m.Id == id);
            if (call == null)
                return NotFound();

            return View(call);
        }

        // GET: Calls/Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Problem,CallTime,Answered,AnswerTime")] Call call)
        {
            if (ModelState.IsValid)
            {
                _ctx.Add(call);
                await _ctx.SaveChangesAsync();
                await _signalrHub.Clients.Group("CallCenters").NewCallReceivedAsync(call);
                //await _signalrHub.Clients.All.SendAsync("LoadCalls");
                return RedirectToAction(nameof(Index));
            }
            return View(call);
        }

        // GET: Calls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _ctx.Calls == null)
                return NotFound();

            var call = await _ctx.Calls.FindAsync(id);
            if (call == null)
                return NotFound();

            return View(call);
        }

        // POST: Calls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Problem,CallTime,Answered,AnswerTime")] Call call)
        {
            if (id != call.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _ctx.Update(call);
                    await _ctx.SaveChangesAsync();
                    await _signalrHub.Clients.Group("CallCenters").CallEditedAsync(call);
                    //await _signalrHub.Clients.All.SendAsync("LoadCalls");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallExists(call.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(call);
        }

        // GET: Calls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var call = await _ctx.Calls.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (call == null) return BadRequest();

                _ctx.Calls.Remove(call);
                if (await _ctx.SaveChangesAsync() > 0)
                {
                    await _signalrHub.Clients.Group("CallCenters").CallDeletedAsync();
                    //await _signalrHub.Clients.All.SendAsync("LoadCalls");
                    //return Ok(new { success = true, deletedCall = call });
                    return RedirectToAction(nameof(Index));
                }
                else
                    return BadRequest("Database Error");
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: Calls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_ctx.Calls == null)
                return Problem("Entity set 'PetDbContext.Calls' is null.");
            var call = await _ctx.Calls.FindAsync(id);
            if (call != null)
                _ctx.Calls.Remove(call);

            await _ctx.SaveChangesAsync();
            await _signalrHub.Clients.Group("CallCenters").CallDeletedAsync();
            //await _signalrHub.Clients.All.SendAsync("LoadCalls");
            return RedirectToAction(nameof(Index));
        }

        bool CallExists(int id) => _ctx.Calls.Any(e => e.Id == id);
    }
}