using LearningDDD.Api.Dtos.Group;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Api.Services.Groups
{
    public interface IGroupService
    {
        Task<Result<Group>> CreateGroupAsync(CreateGroup groupDto);
        Task<Result<bool>> UpdateGroupAsync(Guid id, UpdateGroup groupDto);
        Task<Result<bool>> DeleteGroupAsync(Guid id);
        Task<Result<IEnumerable<CreatedGroup>>> GetGroupsAsync();
    }
}