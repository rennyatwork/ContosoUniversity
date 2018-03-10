using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;
using System.Data.Common;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public async Task<ActionResult> About()
        //{
        //    /*
        //     * In the 1.0 version of Entity Framework Core, the entire result set is returned to the client, 
        //     * and grouping is done on the client. 
        //     * In some scenarios this could create performance problems. 
        //     * Be sure to test performance with production volumes of data, and if necessary 
        //     * use raw SQL to do the grouping on the server. 
        //     * For information about how to use raw SQL, see the last tutorial in this series.
        //     * 
        //     * **/
        //    IQueryable<EnrollmentDateGroup> data =
        //        from student in _context.Students
        //        group student by student.EnrollmentDate into dateGroup
        //        select new EnrollmentDateGroup()
        //        {
        //            EnrollmentDate = dateGroup.Key,
        //            StudentCount = dateGroup.Count()
        //        };
        //    return View(await data.AsNoTracking().ToListAsync());
        //}


        /*
         Earlier you created a student statistics grid for the About page that showed the number of students 
         for each enrollment date. You got the data from the Students entity set (_context.Students) and used LINQ 
         to project the results into a list of EnrollmentDateGroup view model objects. 
         Suppose you want to write the SQL itself rather than using LINQ. To do that you need to run a SQL query 
         that returns something other than entity objects. In EF Core 1.0, 
         one way to do that is write ADO.NET code and get the database connection from EF.
         */
        public async Task<ActionResult> About()
        {
            List<EnrollmentDateGroup> groups = new List<EnrollmentDateGroup>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                        + "FROM Person "
                        + "WHERE Discriminator = 'Student' "
                        + "GROUP BY EnrollmentDate";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroup { EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1) };
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return View(groups);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
