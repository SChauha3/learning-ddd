using Microsoft.EntityFrameworkCore;
using LearningDDD.Api.Dtos.Group;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;
using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Domain.Interfaces;

namespace LearningDDD.Api.Services.Groups
{
    public class GroupService : IGroupService
    {
        private const string CapacityErrorMessage = "The Capacity in Amps of a Group should always be greater than or equal to the sum of the Max current in Amps of all Connectors indirectly belonging to the Group.";
        private const string GroupNotFound = "Group not found";

        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Group>> CreateGroupAsync(CreateGroup createGroup)
        {
            var result = Group.Create(createGroup.Name, createGroup.Capacity);

            if (result.IsSuccess && result.Value is not null)
            {
                await _unitOfWork.Groups.AddAsync(result.Value);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return result;
        }

        public async Task<Result<bool>> UpdateGroupAsync(Guid id, UpdateGroup updateGroup)
        {
            var group = await _unitOfWork.Groups.FindAsync(
                g => g.Id == id,
                query => query
                .Include(g => g.ChargeStations)
                .ThenInclude(cs => cs.Connectors));

            if (group is null)
                return Result<bool>.Fail(GroupNotFound, ErrorType.GroupNotFound);

            var result = group.Update(updateGroup.Name, updateGroup.Capacity);
            if (result.IsSuccess)
                await _unitOfWork.SaveChangesAsync();
                
            return result;
        }

        public async Task<Result<bool>> DeleteGroupAsync(Guid id)
        {
            var group = await _unitOfWork.Groups.FindByIdAsync(id);
            if (group is null)
                return Result<bool>.Fail(GroupNotFound, ErrorType.GroupNotFound);

            _unitOfWork.Groups.Delete(group);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<CreatedGroup>>> GetGroupsAsync()
        {
            var groups = await _unitOfWork.Groups.GetAsync(
                q => q.Include(g => g.ChargeStations).ThenInclude(cs => cs.Connectors));

            var createdGroups = groups.Select(MapToCreatedGroup);

            return Result<IEnumerable<CreatedGroup>>.Success(createdGroups);
        }

        private static CreatedGroup MapToCreatedGroup(Group group) => new CreatedGroup
        {
            Id = group.Id,
            Name = group.Name,
            Capacity = group.Capacity,
            ChargeStations = group.ChargeStations.Select(cs => new CreatedChargeStation
            {
                Id = cs.Id,
                Name = cs.Name,
                Connectors = cs.Connectors.Select(c => new CreatedConnector
                {
                    ChargeStationContextId = c.ChargeStationContextId,
                    Id = c.Id,
                    MaxCurrent = c.MaxCurrent
                }).ToList()
            }).ToList()
        };

    }
}