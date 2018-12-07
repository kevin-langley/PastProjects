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
    public class CouponsController : Controller
    {
        private readonly AppDbContext _context;

        public CouponsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coupons
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coupons.ToListAsync());
        }

        // GET: Coupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(m => m.CouponID == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // GET: Coupons/Create
        [Authorize(Roles="Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CouponID,CouponCode")] Coupon coupon, String ShippingCouponValue, String DiscountCouponValue, CouponType SelectedType)
        {
            decimal decShipVal;
            decimal decDisVal;

            try
            {
                Decimal.TryParse(ShippingCouponValue, out decShipVal);
                Decimal.TryParse(DiscountCouponValue, out decDisVal);
            }
            catch
            {
                ViewBag.InvalidValue = "Error. Coupon Value is invalid";
                return View(coupon);
            }

            if (SelectedType == CouponType.FreeShippingForX)
            {
                coupon.CouponValue = decShipVal;
            }
            else
            {
                coupon.CouponValue = decDisVal;
            }

            coupon.CouponActive = true;
            _context.Add(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Coupons/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coupon coupon, Boolean AlreadyActive, Boolean AlreadyInactive)
        {
            if (id != coupon.CouponID)
            {
                return NotFound();
            }

            coupon = _context.Coupons.Find(id);

            try
            {
                if (coupon.CouponActive == true)
                {
                    if (AlreadyActive != true)
                    {
                        coupon.CouponActive = false;
                    }
                }
                if (coupon.CouponActive == false)
                {
                    if (AlreadyInactive == true)
                    {
                        coupon.CouponActive = true;
                    }
                }

                _context.Update(coupon);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(coupon.CouponID))
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




        private bool CouponExists(int id)
        {
            return _context.Coupons.Any(e => e.CouponID == id);
        }
    }
}
