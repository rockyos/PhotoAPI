using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAPI.Extensions;
using PhotoAPI.Models.Dto;
using PhotoAPI.Models.Entity;
using PhotoAPI.Services.Interfaces;

namespace PhotoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly string _sessionkey = "photos";
        private readonly IGetPhotoService _getPhotoService;
        private readonly IResizeService _resizer;
        private readonly IDeleteService _deleteService;


        protected ISession Session => HttpContext.Session;

        public PhotoController(IGetPhotoService getPhotoService, IResizeService resizer, IDeleteService deleteService)
        {
            _getPhotoService = getPhotoService;
            _resizer = resizer;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoDTO>> GetAsync()
        {
            var photos = await _getPhotoService.GetPhotoDBandSessionAsync(Session, _sessionkey);
            return photos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id, int width)
        {
            var sessionPhotos = Session.Get<List<Photo>>(_sessionkey);
            var resizedImage = await _resizer.GetImageAsync(sessionPhotos, id, width);
            return new FileContentResult(resizedImage, "binary/octet-stream");
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
        public async Task DeleteAsync(string guid)
        {
            await _deleteService.DeleteAsync(guid, Session, _sessionkey);
        }
    }
}
