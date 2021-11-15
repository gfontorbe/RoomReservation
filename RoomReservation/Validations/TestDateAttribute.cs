using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Validations
{
	public sealed class TestDateAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if ((DateTime)value < DateTime.Now)
			{
				return new ValidationResult("test wrong date");
			}

			return ValidationResult.Success;
		}
	}
}
