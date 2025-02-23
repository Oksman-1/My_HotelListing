﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository.Configuration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
	{
		public void Configure(EntityTypeBuilder<Hotel> builder)
		{
			builder.HasData
			(
				new Hotel
				{
					Id = 1,
					Name = "Sandals Resort and Spa",
					Address = "Negril",
					CountryId = 1,
					Rating = 4.5
				},
				new Hotel
				{
					Id = 2,
					Name = "Comfort Suites",
					Address = "George Town",
					CountryId = 3,
					Rating = 4.3
				},
				new Hotel
				{
					Id = 3,
					Name = "Grand Palladium",
					Address = "Nassua",
					CountryId = 2,
					Rating = 4.0
				}
			);
		}
	}
}
