using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTranferObjects;
using System.Text;

namespace My_HotelListing;

public class CsvOutputFormatter : TextOutputFormatter
{
	public CsvOutputFormatter()
	{
		SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedEncodings.Add(Encoding.Unicode);
	}

	protected override bool CanWriteType(Type? type)
	{
		if (typeof(CountryDto).IsAssignableFrom(type) || typeof(IEnumerable<CountryDto>).IsAssignableFrom(type))
		{
			return base.CanWriteType(type);
		}

		return false;		
	}

	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
	{
		var response = context.HttpContext.Response;
		var buffer = new StringBuilder();	

		if(context.Object is IEnumerable<CountryDto>) 
		{
			foreach(var country in (IEnumerable<CountryDto>)context.Object)
			{
				FormatCsv(buffer, country);	
			}
		}
        else
        {
			FormatCsv(buffer, (CountryDto)context.Object);

		}
		await response.WriteAsync(buffer.ToString());	
    }

	private static void FormatCsv(StringBuilder buffer, CountryDto country)
	{
		buffer.AppendLine($"{country.Id}, \"{country.Name}, \"{country.FullCountryDesignation}\"");
	}	
}
