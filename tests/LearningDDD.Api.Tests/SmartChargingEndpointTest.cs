// csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using LearningDDD.Infrastructure.Data;

namespace LearningDDD.Api.Endpoints
{
    public class LearningDDDEndpointTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public LearningDDDEndpointTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the app's DbContext registration.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add DbContext using in-memory database for testing.
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlite("Data Source=app.db");
                    });
                });
            });
        }

        private AppDbContext GetDbContext()
        {
            var scope = _factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        //[Fact]
        //public async Task Group_CRUD_Works()
        //{
        //    var client = _factory.CreateClient();

        //    // CREATE
        //    var createGroup = new CreateGroup { Name = "Test Group", CapacityInAmps = 100 };
        //    var createResp = await client.PostAsJsonAsync("/groups", createGroup);
        //    Assert.Equal(HttpStatusCode.Created, createResp.StatusCode);

        //    var groupId = await createResp.Content.ReadFromJsonAsync<Guid>();

        //    // READ
        //    var getResp = await client.GetAsync("/groups");
        //    Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

        //    var db = GetDbContext();
        //    var group = await db.Groups.FindAsync(groupId);
        //    Assert.NotNull(group);
        //    Assert.Equal("Test Group", group.Name);

        //    // UPDATE
        //    var updateGroup = new UpdateGroup { Name = "Updated Group", CapacityInAmps = 200 };
        //    var updateResp = await client.PutAsJsonAsync($"/groups/{groupId}", updateGroup);
        //    Assert.Equal(HttpStatusCode.OK, updateResp.StatusCode);

        //    group = await db.Groups.FindAsync(groupId);
        //    Assert.Equal("Updated Group", group.Name);
        //    Assert.Equal(200, group.CapacityInAmps);

        //    // DELETE
        //    var deleteResp = await client.DeleteAsync($"/groups/{groupId}");
        //    Assert.Equal(HttpStatusCode.OK, deleteResp.StatusCode);

        //    group = await db.Groups.FindAsync(groupId);
        //    Assert.Null(group);
        //}

        //[Fact]
        //public async Task ChargeStation_CRUD_Works()
        //{
        //    var client = _factory.CreateClient();
        //    var db = GetDbContext();

        //    // First, create a group for the station
        //    var group = new Group { Name = "G1", CapacityInAmps = 100 };
        //    db.Groups.Add(group);
        //    await db.SaveChangesAsync();

        //    // CREATE
        //    var createStation = new CreateChargeStation { Name = "Station1", GroupId = group.Id };
        //    var createResp = await client.PostAsJsonAsync("/chargestations", createStation);
        //    Assert.Equal(HttpStatusCode.Created, createResp.StatusCode);

        //    var stationId = await createResp.Content.ReadFromJsonAsync<Guid>();

        //    // UPDATE
        //    var updateStation = new UpdateChargeStation { Name = "Station1-Updated", GroupId = group.Id };
        //    var updateResp = await client.PutAsJsonAsync($"/chargestations/{stationId}", updateStation);
        //    Assert.Equal(HttpStatusCode.OK, updateResp.StatusCode);

        //    var station = await db.ChargeStations.FindAsync(stationId);
        //    Assert.Equal("Station1-Updated", station.Name);

        //    // DELETE
        //    var deleteResp = await client.DeleteAsync($"/chargeStations/{stationId}");
        //    Assert.Equal(HttpStatusCode.OK, deleteResp.StatusCode);

        //    station = await db.ChargeStations.FindAsync(stationId);
        //    Assert.Null(station);
        //}

        //[Fact]
        //public async Task Connector_CRUD_Works()
        //{
        //    var client = _factory.CreateClient();
        //    var db = GetDbContext();

        //    // Create group and station first
        //    var group = new Group { Name = "G2", CapacityInAmps = 100 };
        //    db.Groups.Add(group);
        //    var station = new ChargeStation { Name = "S2", GroupId = group.Id };
        //    db.ChargeStations.Add(station);
        //    await db.SaveChangesAsync();

        //    // CREATE
        //    var createConnector = new CreateConnector { ChargeStationId = station.Id, MaxCurrentInAmps = 10 };
        //    var createResp = await client.PostAsJsonAsync("/connectors", createConnector);
        //    Assert.Equal(HttpStatusCode.Created, createResp.StatusCode);

        //    var connectorId = await createResp.Content.ReadFromJsonAsync<Guid>();

        //    // UPDATE
        //    var updateConnector = new UpdateConnector { MaxCurrentInAmps = 20 };
        //    var updateResp = await client.PutAsJsonAsync($"/connectors/{connectorId}", updateConnector);
        //    Assert.Equal(HttpStatusCode.OK, updateResp.StatusCode);

        //    var connector = await db.Connectors.FindAsync(connectorId);
        //    Assert.Equal(20, connector.MaxCurrentInAmps);

        //    // DELETE
        //    var deleteResp = await client.DeleteAsync($"/connectors/{connectorId}");
        //    Assert.Equal(HttpStatusCode.OK, deleteResp.StatusCode);

        //    connector = await db.Connectors.FindAsync(connectorId);
        //    Assert.Null(connector);
        //}
    }
}