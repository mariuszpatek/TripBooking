namespace TripBookingApi.Domain;

public class Registration
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public Guid TripId { get; private set; }
    public Trip Trip { get; private set; }

    private Registration()
    {
        // Required by EF Core
    }
    
    public Registration(string email, Trip trip)
    {
        Id = Guid.NewGuid();
        Email = email;
        Trip = trip;
    }
}