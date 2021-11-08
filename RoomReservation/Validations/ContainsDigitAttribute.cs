using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Validations
{
	public class ContainsDigitAttribute: ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if(!(value is string))
			{
				return new ValidationResult("Must be a string");
			}

			foreach(char c in (string)value)
			{
				if (char.IsDigit(c))
				{
					return ValidationResult.Success;
				}
			}

			return new ValidationResult("Must contain a number");
		}


	}
}
