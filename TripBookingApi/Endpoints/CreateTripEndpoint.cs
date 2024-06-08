using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;
using TripBookingApi.Domain;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;
public class CreateTripRequest
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfSeats { get; set; }
}

[UsedImplicitly]
public class CreateTripEndpoint : Endpoint<CreateTripRequest, Trip>
{
    private readonly ITripRepository _repository;

    public CreateTripEndpoint(ITripRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Post("/trips");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTripRequest req, CancellationToken ct)
    {
        var isTripExists = await _repository.IsTripExists(req.Name);
        if (isTripExists)
        {
            AddError("Trip name must be unique.", "Name");
            await SendErrorsAsync(400, ct);
            return;
        }

        var trip = new Trip(req.Name, req.Country, req.Description, req.StartDate, req.NumberOfSeats);

        await _repository.AddTrip(trip);

        await SendOkAsync(trip, ct);
    }
}

[UsedImplicitly]
public class CreateTripRequestValidator : Validator<CreateTripRequest>
{
    public CreateTripRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .Must(date => date > DateTime.Now).WithMessage("Start date must be in the future.");

        RuleFor(x => x.NumberOfSeats)
            .InclusiveBetween(1, 100);
    }
}