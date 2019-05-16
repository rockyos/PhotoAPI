﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
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
        private readonly IAddPhotoService _indexService;

        protected ISession Session => HttpContext.Session;

        public PhotoController(IGetPhotoService getPhotoService, IResizeService resizer, IDeleteService deleteService,
            IAddPhotoService indexService)
        {
            _getPhotoService = getPhotoService;
            _resizer = resizer;
            _deleteService = deleteService;
            _indexService = indexService;
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

        [HttpPost]
        public async Task PostAsync([FromForm] IFormFile file)
        {
            await _indexService.GetIndexServiceAsync(file, Session, _sessionkey);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromForm] string value)
        {

        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _deleteService.DeleteAsync(id, Session, _sessionkey);
        }
    }
}
