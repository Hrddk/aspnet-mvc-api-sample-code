using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DatabaseLibrary.DbContext;
using DatabaseLibrary.Repository;

namespace aspnet_mvc_api.Controllers
{
    public class DepartmentController : ApiController
    {
        private readonly DepartmentRepository _repository;
        public DepartmentController()
        {
            _repository = DepartmentRepository.Instance;
        }
        // GET: api/Department
        public async Task<IHttpActionResult> GetDepartments()
        {
            try
            {
                var list = await _repository.Departments().ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Department/5
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> GetDepartment(int id)
        {
            try
            {
                Department department = await _repository.FindAsync(id);
                if (department == null)
                {
                    return NotFound();
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Department/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDepartment(int id, Department department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != department.Id)
                {
                    return BadRequest();
                }

                Department depart = await _repository.Update(department);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }

            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Department
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> PostDepartment(Department department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var addedDepartment = await _repository.Add(department);
                return Ok(addedDepartment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/Department/5
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> DeleteDepartment(int id)
        {
            Department department = await _repository.Delete(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        private bool DepartmentExists(int id)
        {
            return _repository.Departments().Count(e => e.Id == id) > 0;
        }
    }
}