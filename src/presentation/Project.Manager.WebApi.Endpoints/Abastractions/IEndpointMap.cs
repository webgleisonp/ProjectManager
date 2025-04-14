using Microsoft.AspNetCore.Routing;

namespace Project.Manager.WebApi.Endpoints.Abastractions;

internal interface IEndpointMap
{
    void MapEndpoints(IEndpointRouteBuilder app);
}