using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Group;
using System;

namespace CampusCourses.Services.IServices
{
    public interface IGroupService
    {
        public Task<CampusGroupModel> createGroup(CreateCampusGroupModel createCampusGroupModel, Guid userId);
        public Task<List<CampusGroupModel>> getListAllGroups(Guid userId);
        public Task<CampusGroupModel> editCampusGroup(Guid id, CreateCampusGroupModel createCampusGroupModel, Guid userId);
        public Task deleteGroup(Guid groupId, Guid userId);
        public Task<List<CampusCoursePreviewModel>> getCampusGroups(Guid id, Guid userId);
    }
}
