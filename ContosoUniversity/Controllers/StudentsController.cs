using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {
        /*
         * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro
         * 
         ASP.NET dependency injection will take care of passing an instance of SchoolContext into the controller. 
         You configured that in the Startup.cs file earlier.
             */
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            /*
             * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro
             * 
             The async keyword tells the compiler to generate callbacks for parts of the method body and to automatically create the 
             Task<IActionResult> object that is returned.

            The return type Task<IActionResult> represents ongoing work with a result of type IActionResult.

            The await keyword causes the compiler to split the method into two parts. 
            The first part ends with the operation that is started asynchronously. 
            The second part is put into a callback method that is called when the operation completes.

            ToListAsync is the asynchronous version of the ToList extension method.
             */

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : string.Empty;
            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["lixo"] = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewData["CurrentSort"] = sortOrder;

            /*
             Here you are calling the Where method on an IQueryable object, and the filter will be processed on the server. 
             In some scenarios you might be calling the Where method as an extension method on an in-memory collection. 
             (For example, suppose you change the reference to _context.Students so that instead of an EF DbSet it references a repository method that returns 
             an IEnumerable collection.) The result would normally be the same but in some cases may be different.
            For example, the .NET Framework implementation of the Contains method performs a case-sensitive comparison by default, but in SQL Server 
            this is determined by the collation setting of the SQL Server instance. That setting defaults to case-insensitive. 
            You could call the ToUpper method to make the test explicitly case-insensitive: Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper()). 
            That would ensure that results stay the same if you change the code later to use a repository which returns an IEnumerable collection 
            instead of an IQueryable object.
             */

            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;


            var students = from s in _context.Students
                           select s;


            if (!string.IsNullOrEmpty(searchString))
            {
                students = from s in _context.Students
                           where s.FirstMidName.ToUpper().Contains(searchString.ToUpper()) || s.LastName.ToUpper().Contains(searchString.ToUpper())
                           select s;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            

            //return View(await _context.Students.ToListAsync());
            //return View(await students.AsNoTracking().ToListAsync());


            int pageSize = 10;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(),page ?? 1, pageSize));
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            /*
             * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud
             * 
             The Include and ThenInclude methods cause the context to load the Student.Enrollments navigation property, and within each enrollment the Enrollment.
             Course navigation property. You'll learn more about these methods in the reading related data tutorial.1
             The AsNoTracking method improves performance in scenarios where the entities returned will not be updated in the current context's lifetime. 
             You'll learn more about AsNoTracking at the end of this tutorial.

             */
            var student = await _context.Students
        .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
        .AsNoTracking()
        .SingleOrDefaultAsync(m => m.ID == id);



            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
         * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud
         * 
         * Although not implemented in this sample, a production quality application would log the exception. For more information, 
         * see the Log for insight section in Monitoring and Telemetry (Building Real-World Cloud Apps with Azure).
         * 
         * An alternative way to prevent overposting that is preferred by many developers is to use view models rather than entity classes with model binding. 
         * Include only the properties you want to update in the view model. 
         * **/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstMidName,LastName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }



        //// POST: Students/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,FirstMidName,LastName,EnrollmentDate")] Student student)
        //{
        //    if (id != student.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(student);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!StudentExists(student.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(student);
        //}



        /**
        These changes implement a security best practice to prevent overposting. 
            The scaffolder generated a Bind attribute and added the entity created by the model binder to the entity set with a Modified flag. 
            That code is not recommended for many scenarios because the Bind attribute clears out any pre-existing data in fields not listed in the 
            Include parameter.1

            The new code reads the existing entity and calls TryUpdateModel to update fields in the retrieved entity based on user input in the posted form data.      
        */
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Students.SingleOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        //// GET: Students/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await _context.Students
        //        .SingleOrDefaultAsync(m => m.ID == id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(student);
        //}


        /*
   This code accepts an optional parameter that indicates whether the method was called after a failure to save changes. 
   This parameter is false when the HttpGet Delete method is called without a previous failure. 
   When it is called by the HttpPost Delete method in response to a database update error, the parameter is true and an error message is passed to the view.
       */
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }



        //// POST: Students/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);
        //    _context.Students.Remove(student);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        /*
         This code retrieves the selected entity, then calls the Remove method to set the entity's status to Deleted. 
         When SaveChanges is called, a SQL DELETE command is generated.

            If improving performance in a high-volume application is a priority, you could avoid an unnecessary 
            SQL query by instantiating a Student entity using only the primary key value and then setting the entity state to Deleted.
         */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
