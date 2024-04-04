namespace Shared.RequestFeatures;

public class HotelParameters : RequestParameters
{
    public HotelParameters() => OrderBy = "name";
	

	public double MinRating { get; set; } = 0;
    public double MaxRating { get; set; } = 5.0;
    public double HotelRating { get; set; }
    public string? SearchTerm { get; set; }

    public bool ValidRatingRange => MaxRating > MinRating;
}
