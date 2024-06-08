using TripBookingApi.Domain;

namespace TripBookingApi.Repositories;

public interface ITripRepository
{
    Task<Trip?> GetTripById(Guid id);
    Task<List<Trip>> GetAllTrips();
    Task<bool> IsTripExists(string name);
    Task<List<Trip>> GetTripsByCountry(string country);
    Task AddTrip(Trip trip);
    Task UpdateTrip(Trip trip);
    Task DeleteTrip(Guid id);
}