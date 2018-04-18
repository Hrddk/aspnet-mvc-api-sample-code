using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DatabaseLibrary.DbContext;
using DatabaseLibrary.Repository;
using Newtonsoft.Json;

namespace aspnet_mvc_api.Controllers
{
    public class UserController : ApiController
    {
        private UserRepository _repository;

        public UserController()
        {
            _repository = UserRepository.Instance;
        }

        // GET: api/User
        [ResponseType(typeof(IEnumerable<User>))]
        public async Task<IHttpActionResult> GetUsers()
        {
            try
            {
                var list = await _repository.Users().ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/User/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            try
            {
                User user = await _repository.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return BadRequest();

            try
            {
                var updatedUser = await _repository.Update(user);
                return Ok(updatedUser);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    return InternalServerError(ex);
            }
        }

        // POST: api/User
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newlyAddedUser = await _repository.Add(user);
                return Ok(newlyAddedUser);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/User/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await _repository.Delete(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _repository.Users().Count(e => e.Id == id) > 0;
        }
    }
}