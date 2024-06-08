using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;
using TripBookingApi.Domain;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;

public class EditTripRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfSeats { get; set; }
}

[UsedImplicitly]
public class EditTripEndpoint : Endpoint<EditTripRequest, Trip>
{
    private readonly ITripRepository _repository;

    public EditTripEndpoint(ITripRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Put("/trips/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EditTripRequest req, CancellationToken ct)
    {
        var trip = await _repository.GetTripById(req.Id);
        if (trip is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        trip.UpdateTrip(req.Name, req.Country, req.Description, req.StartDate, req.NumberOfSeats);

        await _repository.UpdateTrip(trip);

        await SendOkAsync(trip, ct);
    }
}

[UsedImplicitly]
public class EditTripRequestValidator : Validator<EditTripRequest>
{
    public EditTripRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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