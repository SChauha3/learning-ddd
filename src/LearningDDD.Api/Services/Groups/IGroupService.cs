using LearningDDD.Api.Dtos.Group;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Api.Services.Groups
{
    public interface IGroupService
    {
        Task<Result<Group>> CreateGroupAsync(CreateGroup groupDto);
        Task<bool> UpdateGroupAsync(Guid id, UpdateGroup groupDto);
        Task<bool> DeleteGroupAsync(Guid id);
        Task<Result<IEnumerable<CreatedGroup>>> GetGroupsAsync();
    }
}