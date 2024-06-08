using FastEndpoints;
using JetBrains.Annotations;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;

public class GetSingleTripResponse
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfSeats { get; set; }
    public Guid Id { get; set; }
}

[UsedImplicitly]
public class GetSingleTripEndpoint : EndpointWithoutRequest<GetSingleTripResponse>
{
    private readonly ITripRepository _repository;

    public GetSingleTripEndpoint(ITripRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/trips/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var trip = await _repository.GetTripById(id);
        if (trip == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new GetSingleTripResponse
        {
            Id = trip.Id,
            Name = trip.Name,
            Country = trip.Country,
            Description = trip.Description,
            StartDate = trip.StartDate,
            NumberOfSeats = trip.NumberOfSeats
        };

        await SendOkAsync(response, ct);
    }
}