using EmpMVC.Data;
using EmpMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using X.PagedList;
using System.Drawing.Printing;
using X.PagedList.Extensions;

namespace EmpMVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly EmployeeDbContext _context;
        public DepartmentController(EmployeeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "DeptName" : sortOrder;
            IPagedList<Department> departments = null;

            switch (sortOrder)
            {
                case "DeptName":
                    if (sortOrder.Equals(CurrentSort))
                        departments = _context.Departments.OrderByDescending
                                (m => m.DeptName).ToPagedList(pageIndex, pageSize);
                    else
                        departments = _context.Departments.OrderBy
                                (m => m.DeptName).ToPagedList(pageIndex, pageSize);
                    break;
            }
                    return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department dept) {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(dept);
                _context.SaveChanges();
                TempData["ResultOk"] = "Record added!";
                return RedirectToAction("Index");
            }
            return View(dept);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var dept = _context.Departments.Find(id);

            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department dept)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(dept);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated!";
                return RedirectToAction("Index");
            }

            return View(dept);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var dept = _context.Departments.Find(id);

            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDept(int? id)
        {
            var deleterecord = _context.Departments.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted!";
            return RedirectToAction("Index");
        }
    }
}
