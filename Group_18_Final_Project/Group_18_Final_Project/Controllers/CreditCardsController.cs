using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Group_18_Final_Project.Controllers
{
    [Authorize]
    public class CreditCardsController : Controller
    {
        private readonly AppDbContext _context;

        public CreditCardsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CreditCards
        public IActionResult Index()
        {
            List<CreditCard> CreditCards = new List<CreditCard>();
            CreditCards = _context.CreditCards.Where(o => o.User.UserName == User.Identity.Name).Include(o => o.User).ToList();
            return View(CreditCards);
        }
        // GET: CreditCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCards.Include(m => m.User)
                .FirstOrDefaultAsync(m => m.CreditCardID == id);

            //make sure a customer isn't trying to look at someone else's order
            //AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);
            String idi = User.Identity.Name;
            User userLoggedIn = _context.Users.FirstOrDefault(u => u.UserName == idi);
            if (creditCard.User.UserName != userLoggedIn.UserName)
            {
                return View("Error", new string[] { "You are not authorized to view this credit card!" });
            }


            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // GET: CreditCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreditCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreditCardID,CreditType,CreditCardNumber")] CreditCard creditCard)
        {
            //get user info
            //AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);
            String id = User.Identity.Name;
            User user = _context.Users.Include(m => m.CreditCards).FirstOrDefault(u => u.UserName == id);
            creditCard.User = user;

            if (ModelState.IsValid)
            {
                _context.Add(creditCard);
                user.CreditCards.Add(creditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Account");
            }
            return RedirectToAction("Index","Account");
        }

        // GET: CreditCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCards.FindAsync(id);

            String idi = User.Identity.Name;
            User userLoggedIn = _context.Users.FirstOrDefault(u => u.UserName == idi);
            if (creditCard.User.UserName != userLoggedIn.UserName)
            {
                return View("Error", new string[] { "You are not authorized to edit this credit card!" });
            }


            if (creditCard == null)
            {
                return NotFound();
            }
            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CreditCardID,CreditType,CreditCardNumber")] CreditCard creditCard)
        {
            if (id != creditCard.CreditCardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditCardExists(creditCard.CreditCardID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCards
                .FirstOrDefaultAsync(m => m.CreditCardID == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditCard = await _context.CreditCards.FindAsync(id);
            _context.CreditCards.Remove(creditCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardExists(int id)
        {
            return _context.CreditCards.Any(e => e.CreditCardID == id);
        }
    }
}
