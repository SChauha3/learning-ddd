using Microsoft.EntityFrameworkCore;
using LearningDDD.Api.Dtos.Group;
using LearningDDD.Domain.Ports;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;
using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Api.Dtos.Connector;

namespace LearningDDD.Api.Services.Groups
{
    public class GroupService : IGroupService
    {
        private const string CapacityErrorMessage = "The Capacity in Amps of a Group should always be greater than or equal to the sum of the Max current in Amps of all Connectors indirectly belonging to the Group.";
        private const string GroupNotFound = "Group not found";

        private readonly IRepository<Group> _groupRepository;

        public GroupService(IRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Result<Group>> CreateGroupAsync(CreateGroup createGroup)
        {
            var result = Group.Create(createGroup.Name, createGroup.Capacity);

            if (result.IsSuccess && result.Value is not null)
                await _groupRepository.AddAsync(result.Value);

            return result;
        }

        public async Task<bool> UpdateGroupAsync(Guid id, UpdateGroup updateGroup)
        {
            var group = await _groupRepository.FindAsync(
                g => g.Id == id,
                query => query
                .Include(g => g.ChargeStations)
                .ThenInclude(cs => cs.Connectors));

            if (group is null)
                return false;

            var result = group.Update(updateGroup.Name, updateGroup.Capacity);
            if (result.IsSuccess)
                await _groupRepository.UpdateAsync(group);

            return result.Value;
        }

        public async Task<bool> DeleteGroupAsync(Guid id)
        {
            var storedGroup = await _groupRepository.FindByIdAsync(id);
            if (storedGroup is null)
                return false;

            await _groupRepository.DeleteAsync(storedGroup);
            return true;
        }

        public async Task<Result<IEnumerable<CreatedGroup>>> GetGroupsAsync()
        {
            var groups = await _groupRepository.GetAsync(
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
