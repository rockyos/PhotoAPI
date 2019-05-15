using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAPI.Models.Dto;
using PhotoAPI.Services.Interfaces;

namespace PhotoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly string _sessionkey = "photos";
        private readonly IGetPhotoService _getPhotoService;

        protected ISession Session => HttpContext.Session;


        // GET api/values
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var photos = await _getPhotoService.GetPhotoDBandSessionAsync(Session, _sessionkey);
            return JsonResult(photos);
        }

        private ActionResult JsonResult(List<PhotoDTO> photos)
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
