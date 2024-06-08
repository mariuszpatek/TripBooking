using FastEndpoints;
using JetBrains.Annotations;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;

public class ListAllTripsResponse
{
    public string Name { get; set; }
    public string Country { get; set; }
    public DateTime StartDate { get; set; }
    public Guid Id { get; set; }
}

[UsedImplicitly]
public class ListAllTripsEndpoint : EndpointWithoutRequest<List<ListAllTripsResponse>>
{
    private readonly ITripRepository _repository;

    public ListAllTripsEndpoint(ITripRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/trips");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var trips = await _repository.GetAllTrips();
        var response = trips.Select(t => new ListAllTripsResponse
        {
            Id = t.Id,
            Name = t.Name,
            Country = t.Country,
            StartDate = t.StartDate
        }).ToList();

        await SendOkAsync(response, ct);
    }
}