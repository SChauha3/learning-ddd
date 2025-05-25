using Microsoft.EntityFrameworkCore;
using SmartCharging.Api.Dtos.Group;
using SmartCharging.Api.Dtos.Outgoing;
using SmartCharging.Api.Repositories;
using Group = SmartCharging.Api.Models.Group;

namespace SmartCharging.Api.Services.Groups
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

        public async Task<Result<Guid>> CreateGroupAsync(CreateGroup createGroup)
        {
            var group = Group.Create(createGroup.Name, createGroup.Capacity);

            await _groupRepository.AddAsync(group);
            return Result<Guid>.Success(group.Id);
        }

        public async Task<Result> UpdateGroupAsync(Guid id, UpdateGroup updateGroup)
        {
            var group = await GetGroupAsync(id);

            if (group == null)
                return Result.Fail($"Could not find group with Id {id}.", ErrorType.NotFound);

            if (!group.TryUpdate(updateGroup.Name, updateGroup.Capacity))
                return Result.Fail("New capacity is less than total connector load.", ErrorType.InValidCapacity);

            await _groupRepository.UpdateAsync(group);
            return Result.Success();
        }

        public async Task<Result> DeleteGroupAsync(Guid id)
        {
            var storedGroup = await _groupRepository.FindByIdAsync(id);
            if (storedGroup == null)
                return Result.Fail($"group not found with Group Id {id}", ErrorType.NotFound);

            await _groupRepository.DeleteAsync(storedGroup);
            return Result.Success();
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

        private async Task<Group?> GetGroupAsync(Guid id) =>
            await _groupRepository.FindAsync(
                g => g.Id == id,
                query => query
                .Include(g => g.ChargeStations)
                .ThenInclude(cs => cs.Connectors));
    }
}
