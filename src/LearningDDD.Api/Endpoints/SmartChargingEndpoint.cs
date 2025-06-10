using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Api.Dtos.Group;
using LearningDDD.Api.Extensions;
using LearningDDD.Api.Services.ChargeStations;
using LearningDDD.Api.Services.Connectors;
using LearningDDD.Api.Services.Groups;

namespace LearningDDD.Api.Endpoints
{
    public static class LearningDDDEndpoint
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            MapGroupEndpoints(builder);

            MapChargeStationEndpoints(builder);

            MapConnectorEndpoints(builder);

            return builder;
        }

        private static void MapGroupEndpoints(IEndpointRouteBuilder builder)
        {
            var groupsApi = builder.MapGroup("/groups")
            .AddFluentValidationAutoValidation(); // Apply to all endpoints in this group

            groupsApi.MapPost("/", async (CreateGroup createGroup, IGroupService groupService) =>
            {
                var result = await groupService.CreateGroupAsync(createGroup);

                return result.ToApiResult("/groups", true);
            });

            groupsApi.MapPut("/{id}", async (Guid id, UpdateGroup updateGroup, IGroupService groupService) =>
            {
                var result = await groupService.UpdateGroupAsync(id, updateGroup);
                return result.ToApiResult("/groups");
            });

            groupsApi.MapGet("/", async (IGroupService groupService) =>
            {
                var result = await groupService.GetGroupsAsync();
                return result.ToApiResult("/groups");
            });

            //Remove Group
            groupsApi.MapDelete("/{id}", async (Guid id, IGroupService groupService) =>
            {
                var result = await groupService.DeleteGroupAsync(id);
                return result.ToApiResult("/groups");
            });
        }

        private static void MapChargeStationEndpoints(IEndpointRouteBuilder builder)
        {
            var chargeStationsApi = builder.MapGroup("/chargestations")
            .AddFluentValidationAutoValidation();

            //Add ChargeStation
            chargeStationsApi.MapPost("/", async (CreateChargeStation createChargeStation, IChargeStationService chargeStationService) =>
            {
                var result = await chargeStationService.CreateChargeStationAsync(createChargeStation);
                return result.ToApiResult("/chargestations", true);
            });

            //chargeStationsApi.MapGet("/chargestations", async (AppDbContext appDbContext) =>
            //{
            //    return appDbContext.ChargeStations;
            //});

            //Update ChargeStation
            chargeStationsApi.MapPut("/{id}", async (
                Guid id,
                UpdateChargeStation updateChargeStation,
                IChargeStationService chargeStationService) =>
            {
                var result = await chargeStationService.UpdateChargeStationAsync(id, updateChargeStation);
                return result.ToApiResult("/chargestations");
            });

            //Remove ChargeStation
            chargeStationsApi.MapDelete("/{id}", async (Guid id, Guid groupId, IChargeStationService chargeStationService) =>
            {
                var result = await chargeStationService.DeleteChargeStationAsync(id, groupId);
                return result.ToApiResult("/chargestations");
            });
        }

        private static void MapConnectorEndpoints(IEndpointRouteBuilder builder)
        {
            var connectorsApi = builder.MapGroup("/connectors")
            .AddFluentValidationAutoValidation();

            //Add Connector
            connectorsApi.MapPost("/", async (CreateConnector createConnector, IConnectorService connectorService, IServiceProvider serviceProvider) =>
            {
                var result = await connectorService.CreateConnectorAsync(createConnector);
                return result.ToApiResult("/connectors", true);
            });

            //Update Connector
            connectorsApi.MapPut("/{id}", async (Guid id, UpdateConnector updateConnector, IConnectorService connectorService) =>
            {
                var result = await connectorService.UpdateConnectorAsync(id, updateConnector);
                return result.ToApiResult("/connectors");
            });

            //Remove Connector
            connectorsApi.MapDelete("/{connectorId}", async (Guid connectorId, Guid chargeStationId, Guid groupId, IConnectorService connectorService) =>
            {
                var result = await connectorService.DeleteConnectorAsync(connectorId, chargeStationId, groupId);
                return result.ToApiResult("/connectors");
            });
        }
    }
}