﻿using CampusCourses.Data.DTO.Account;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Group;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusCourses.Controllers
{
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _service;
        public GroupController(IGroupService service)
        {
            _service = service;
        }

        [HttpGet("/groups")]
        [Authorize]
        public async Task<ActionResult<CampusGroupModel>> getListCampusGroups()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    var list = await _service.getListAllGroups(userId);

                    return Ok(new { list });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPost("/groups")]
        [Authorize]
        public async Task<IActionResult> createCampusGroup([FromBody] CreateCampusGroupModel createCampusGroupModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusGroupModel campusGroupModel = await _service.createGroup(createCampusGroupModel, userId);

                    return Ok(new { campusGroupModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPut("/groups/{id}")]
        [Authorize]
        public async Task<IActionResult> editNameCampusGroup(Guid id, [FromBody] CreateCampusGroupModel createCampusGroupModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusGroupModel campusGroupModel = await _service.editCampusGroup(id, createCampusGroupModel, userId);

                    return Ok(new { campusGroupModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpDelete("/groups/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteCampusGroup(Guid id)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    await _service.deleteGroup(id, userId);

                    return Ok();
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpGet("/groups/{id}")]
        [Authorize]
        public async Task<ActionResult<CampusCoursePreviewModel>> getListCampusGroups(Guid id)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    List<CampusCoursePreviewModel> campusCoursePreviewModel = await _service.getCampusGroups(id, userId);

                    return Ok(new { campusCoursePreviewModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }
    }
}
