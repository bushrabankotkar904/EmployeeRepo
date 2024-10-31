using EmpMVC.Data;
using EmpMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;
using X.PagedList;

namespace EmpMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeeController(EmployeeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            //IEnumerable<Employee> obj = _context.Employees
            //    .Include(e => e.Department).ToList();
            //return View(obj);
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "FirstName" : sortOrder;
            IPagedList<Employee> employee = null;

            switch (sortOrder)
            {
                case "FirstName":
                    if (sortOrder.Equals(CurrentSort))
                        employee = _context.Employees.Include(e => e.Department).OrderByDescending
                                (m => m.FirstName).ToPagedList(pageIndex, pageSize);
                    else
                        employee = _context.Employees.Include(e => e.Department).OrderBy
                                (m => m.FirstName).ToPagedList(pageIndex, pageSize);
                    break;
                case "LastName":
                    if (sortOrder.Equals(CurrentSort))
                        employee = _context.Employees.Include(e => e.Department).OrderByDescending
                                (m => m.LastName).ToPagedList(pageIndex, pageSize);
                    else
                        employee = _context.Employees.Include(e => e.Department).OrderBy
                                (m => m.LastName).ToPagedList(pageIndex, pageSize);
                    break;
                case "Designation":
                    if (sortOrder.Equals(CurrentSort))
                        employee = _context.Employees.Include(e => e.Department).OrderByDescending
                                (m => m.Designation).ToPagedList(pageIndex, pageSize);
                    else
                        employee = _context.Employees.Include(e => e.Department).OrderBy
                                (m => m.Designation).ToPagedList(pageIndex, pageSize);
                    break;
                case "Address":
                    if (sortOrder.Equals(CurrentSort))
                        employee = _context.Employees.Include(e => e.Department).OrderByDescending
                                (m => m.Address).ToPagedList(pageIndex, pageSize);
                    else
                        employee = _context.Employees.Include(e => e.Department).OrderBy
                                (m => m.Address).ToPagedList(pageIndex, pageSize);
                    break;
                case "CreatedOn":
                    if (sortOrder.Equals(CurrentSort))
                        employee = _context.Employees.Include(e => e.Department).OrderByDescending
                                (m => m.CreatedOn).ToPagedList(pageIndex, pageSize);
                    else
                        employee = _context.Employees.Include(e => e.Department).OrderBy
                                (m => m.CreatedOn).ToPagedList(pageIndex, pageSize);
                    break;
                case "DeptName":
                    if (sortOrder.Equals(CurrentSort))
                        employee = _context.Employees.Include(e => e.Department).OrderByDescending
                                (m => m.Department.DeptName).ToPagedList(pageIndex, pageSize);
                    else
                        employee = _context.Employees.Include(e => e.Department).OrderBy
                                (m => m.Department.DeptName).ToPagedList(pageIndex, pageSize);
                    break;
            }
            return View(employee);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _context.Departments
            .FromSqlRaw("EXEC GetDepartments")
            .ToListAsync();

            
            ViewBag.DepartmentId = new SelectList(departments, "Id", "DeptName");
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                //_context.Employees.Add(emp);
                //_context.SaveChanges();
                //TempData["ResultOk"] = "Record added!";
                //return RedirectToAction("Index");
                _context.Database.ExecuteSqlRaw(
           "EXEC AddEmployee @FirstName, @LastName, @Designation, @DepartmentId, @Address",
           new SqlParameter("@FirstName", emp.FirstName),
           new SqlParameter("@LastName", emp.LastName),
           new SqlParameter("@Designation", emp.Designation),
           new SqlParameter("@DepartmentId", emp.DepartmentId),
           new SqlParameter("@Address", emp.Address)
           );
                _context.SaveChanges();
                TempData["ResultOk"] = "Record added!";
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var emp = _context.Employees.Find(id);

            if (emp == null)
            {
                return NotFound();
            }
            var departments = await _context.Departments
            .FromSqlRaw("EXEC GetDepartments")
            .ToListAsync();


            ViewBag.DepartmentId = new SelectList(departments, "Id", "DeptName");
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw(
           "EXEC UpdateEmployee @EmpId, @FirstName, @LastName, @Designation, @DepartmentId, @Address",
           new SqlParameter("@EmpId", emp.EmployeeId),
           new SqlParameter("@FirstName", emp.FirstName),
           new SqlParameter("@LastName", emp.LastName),
           new SqlParameter("@Designation", emp.Designation),
           new SqlParameter("@DepartmentId", emp.DepartmentId),
           new SqlParameter("@Address", emp.Address)
           );
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated!";
                return RedirectToAction("Index");
            }

            return View(emp);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var emp = _context.Employees.Find(id);

            if (emp == null)
            {
                return NotFound();
            }
            var departments = await _context.Departments
            .FromSqlRaw("EXEC GetDepartments")
            .ToListAsync();


            ViewBag.DepartmentId = new SelectList(departments, "Id", "DeptName");
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmp(int? EmployeeId)
        {
            var deleterecord = _context.Employees.Find(EmployeeId);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted!";
            return RedirectToAction("Index");
        }
    }
}
