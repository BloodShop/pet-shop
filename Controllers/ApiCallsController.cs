using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;
using PetShopProj.Hubs;
using PetShopProj.Models;

namespace PetShopProj.Controllers
{
    [ApiController]
    [Route("api/calls")]
    public class ApiCallsController : Controller
    {
        readonly ICallCenterContext _ctx;
        readonly IHubContext<CallCenterHub, ICallCenterHub> _signalrHub;

        public ApiCallsController(IHubContext<CallCenterHub, ICallCenterHub> hubContext,
            ICallCenterContext ctx)
        {
            _signalrHub = hubContext;
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var calls = await _ctx.Calls.ToListAsync();
            return Ok(calls);
        }

        [HttpDelete("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var call = await _ctx.Calls.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (call == null) return BadRequest();

                _ctx.Calls.Remove(call);
                if (await ((DbContext)_ctx).SaveChangesAsync() > 0)
                {
                    await _signalrHub.Clients.Group("CallCenters").CallDeletedAsync();
                    return Ok(new { success = true, deletedCall = call });
                }
                else
                    return BadRequest("Database Error");
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: ApiCalls/Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Problem,CallTime,Answered,AnswerTime")] Call call)
        {
            if (ModelState.IsValid)
            {
                ((PetDbContext)_ctx).Add(call);
                await ((PetDbContext)_ctx).SaveChangesAsync();
                await _signalrHub.Clients.Group("CallCenters").NewCallReceivedAsync(call);
                return RedirectToAction(nameof(Index));
            }
            return View(call);
        }

        // GET: ApiCalls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _ctx.Calls == null)
                return NotFound();

            var call = await _ctx.Calls.FindAsync(id);
            if (call == null)
                return NotFound();
            return View(call);
        }

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
                    ((PetDbContext)_ctx).Update(call);
                    await ((PetDbContext)_ctx).SaveChangesAsync();
                    await _signalrHub.Clients.Group("CallCenters").CallEditedAsync(call);
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

        private bool CallExists(int id) => _ctx.Calls.Any(e => e.Id == id);
    }
}
