namespace Entities.Exceptions;

public sealed class MaxRatingRangeBadRequestException : BadRequestException
{
	public MaxRatingRangeBadRequestException() : base("Max Rating can't be less than Min Rating")
	{
	}
}
