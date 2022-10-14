using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacebookAPI.DataAccess.Models;

namespace FacebookAPI.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly FacebookDBContext _context;

        public FriendsController(FacebookDBContext context)
        {
            _context = context;
        }

        // GET: api/Friends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friend>>> GetFriends()
        {
            return await _context.Friends.ToListAsync();
        }

        // GET: api/Friends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> GetFriend(int id)
        {
            var friend = await _context.Friends.FindAsync(id);

            if (friend == null)
            {
                return NotFound();
            }

            return friend;
        }

        // PUT: api/Friends/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriend(int id, Friend friend)
        {
            if (id != friend.SendId)
            {
                return BadRequest();
            }

            _context.Entry(friend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(id))
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

        // POST: api/Friends
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Friend>> PostFriend(Friend friend)
        {
            _context.Friends.Add(friend);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FriendExists(friend.SendId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFriend", new { id = friend.SendId }, friend);
        }

        // DELETE: api/Friends/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriend(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FriendExists(int id)
        {
            return _context.Friends.Any(e => e.SendId == id);
        }
    }
}
