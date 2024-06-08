using FastEndpoints;
using JetBrains.Annotations;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;

public class SearchTripsByCountryResponse
{
    public string Name { get; set; }
    public string Country { get; set; }
    public DateTime StartDate { get; set; }
    public Guid Id { get; set; }
}

[UsedImplicitly]
public class SearchTripsByCountryEndpoint : EndpointWithoutRequest<List<SearchTripsByCountryResponse>>
{
    private readonly ITripRepository _repository;

    public SearchTripsByCountryEndpoint(ITripRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/trips/search/{country}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var country = Route<string>("country");
        var trips = await _repository.GetTripsByCountry(country);
        var response = trips.Select(t => new SearchTripsByCountryResponse
        {
            Id = t.Id,
            Name = t.Name,
            Country = t.Country,
            StartDate = t.StartDate
        }).ToList();

        await SendOkAsync(response, ct);
    }
}