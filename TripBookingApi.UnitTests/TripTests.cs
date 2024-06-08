using FluentAssertions;
using TripBookingApi.Domain;

namespace TripBookingApi.UnitTests
{
    public class TripTests
    {
        private Trip _trip;

        [SetUp]
        public void Setup()
        {
            _trip = new Trip("Test Trip", "Test Country", "Test Description", DateTime.Now, 10);
        }

        [Test]
        public void UpdateTrip_WhenCalled_ShouldUpdateTripDetails()
        {
            // Arrange
            var newName = "Updated Trip";
            var newCountry = "Updated Country";
            var newDescription = "Updated Description";
            var newStartDate = DateTime.Now.AddDays(1);
            var newNumberOfSeats = 20;

            // Act
            _trip.UpdateTrip(newName, newCountry, newDescription, newStartDate, newNumberOfSeats);

            // Assert
            _trip.Name.Should().Be(newName);
            _trip.Country.Should().Be(newCountry);
            _trip.Description.Should().Be(newDescription);
            _trip.StartDate.Should().Be(newStartDate);
            _trip.NumberOfSeats.Should().Be(newNumberOfSeats);
        }

        [Test]
        public void Register_WhenEmailNotRegistered_ShouldAddRegistrationAndReturnSuccess()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = _trip.Register(email);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _trip.Registrations.Should().ContainSingle(r => r.Email == email);
        }

        [Test]
        public void Register_WhenEmailAlreadyRegistered_ShouldNotAddRegistrationAndReturnFailure()
        {
            // Arrange
            var email = "test@example.com";
            _trip.Register(email);

            // Act
            var result = _trip.Register(email);

            // Assert
            result.IsFailure.Should().BeTrue();
            _trip.Registrations.Count(r => r.Email == email).Should().Be(1);
        }
    }
}