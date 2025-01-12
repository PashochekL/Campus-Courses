using CampusCourses.Data.DTO.Group;

namespace CampusCourses.Services.IServices
{
    public interface IGroupService
    {
        public Task<CampusGroupModel> createGroup(CreateCampusGroupModel createCampusGroupModel, Guid userId);
        public Task<List<CampusGroupModel>> getListAllGroups(Guid userId);
        public Task<CampusGroupModel> editCampusGroup(Guid id, CreateCampusGroupModel createCampusGroupModel, Guid userId);
    }
}
