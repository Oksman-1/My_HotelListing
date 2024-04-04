namespace Entities.Exceptions;

public sealed class HotelNotFoundException : NotFoundException
{
    public HotelNotFoundException(int hotelId) : base($"The Hotel with id: {hotelId} dosen't exist in the database.")
    {
            
    }
}
