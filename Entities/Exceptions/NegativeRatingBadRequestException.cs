namespace Entities.Exceptions;

public sealed class NegativeRatingBadRequestException : BadRequestException
{
	public NegativeRatingBadRequestException() : base("Hotel Rating Can't be Less than 0.0 or greater than 5.0")
	{
	}
}
