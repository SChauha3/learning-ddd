using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Api.Dtos.Group;
using LearningDDD.Api.Dtos.Outgoing;
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
            builder.MapPost("/groups", async (CreateGroup createGroup, IGroupService groupService) =>
            {
                var result = await groupService.CreateGroupAsync(createGroup);

                return result.ToApiResult("/groups", true);
            }).AddFluentValidationAutoValidation();

            builder.MapPut("/groups/{id}", async (Guid id, UpdateGroup updateGroup, IGroupService groupService) =>
            {
                var result = await groupService.UpdateGroupAsync(id, updateGroup);
                return result.ToApiResult();
            }).AddFluentValidationAutoValidation();

            builder.MapGet("/groups", async (IGroupService groupService) =>
            {
                var result = await groupService.GetGroupsAsync();
                return result.ToApiResult("/groups");
            });

            //Remove Group
            builder.MapDelete("/groups/{id}", async (Guid id, IGroupService groupService) =>
            {
                var result = await groupService.DeleteGroupAsync(id);
                return result.ToApiResult();
            }).AddFluentValidationAutoValidation();

            //Add ChargeStation
            builder.MapPost("/chargestations", async (CreateChargeStation createChargeStation, IChargeStationService chargeStationService) =>
            {
                var result = await chargeStationService.CreateChargeStationAsync(createChargeStation);
                return result.ToApiResult("/chargestations", true);
            }).AddFluentValidationAutoValidation();

            //builder.MapGet("/chargestations", async (AppDbContext appDbContext) =>
            //{
            //    return appDbContext.ChargeStations;
            //});

            //Update ChargeStation
            builder.MapPut("/chargestations/{id}", async (
                Guid id, 
                UpdateChargeStation updateChargeStation, 
                IChargeStationService chargeStationService) =>
            {
                var result = await chargeStationService.UpdateChargeStationAsync(id, updateChargeStation);
                return result.ToApiResult();
            }).AddFluentValidationAutoValidation();

            //Remove ChargeStation
            builder.MapDelete("/chargeStations/{id}", async (Guid id, IChargeStationService chargeStationService) =>
            {
                var result = await chargeStationService.DeleteChargeStationAsync(id);
                return result.ToApiResult();
            }).AddFluentValidationAutoValidation();

            //Add Connector
            builder.MapPost("/connectors", async (CreateConnector createConnector, IConnectorService connectorService, IServiceProvider serviceProvider) =>
            {
                var result = await connectorService.CreateConnectorAsync(createConnector);
                return result.ToApiResult("/connectors", true);
            }).AddFluentValidationAutoValidation();

            //Update Connector
            builder.MapPut("/connectors/{id}", async (Guid id, UpdateConnector updateConnector, IConnectorService connectorService) =>
            {
                var result = await connectorService.UpdateConnectorAsync(id, updateConnector);
                return result.ToApiResult();
            }).AddFluentValidationAutoValidation();

            //Remove Connector
            //builder.MapDelete("/connectors/{connectorId}", async (Guid connectorId, IConnectorService connectorService) =>
            //{
            //    var result = await connectorService.DeleteConnectorAsync(connectorId);
            //    return result.ToApiResult();
            //}).AddFluentValidationAutoValidation();

            return builder;
        }
    }
}