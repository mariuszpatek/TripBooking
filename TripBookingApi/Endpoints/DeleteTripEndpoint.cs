using FastEndpoints;
using JetBrains.Annotations;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;

[UsedImplicitly]
public class DeleteTripEndpoint : EndpointWithoutRequest
{
    private readonly ITripRepository _repository;

    public DeleteTripEndpoint(ITripRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Delete("/trips/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        await _repository.DeleteTrip(id);
        await SendOkAsync(ct);
    }
}