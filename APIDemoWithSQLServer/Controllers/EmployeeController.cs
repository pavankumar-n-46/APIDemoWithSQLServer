using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDBAccess;


namespace APIDemoWithSQLServer.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var employees = entities.Employees.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, employees);
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var value = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (value == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, id.ToString());
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, value);
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }
    }
}
