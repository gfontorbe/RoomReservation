using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Validations
{
	public sealed class IsDateAfterAttribute : ValidationAttribute
		//, IClientModelValidator
	{
		private readonly bool allowEqualDates;

		public string TestedPropertyName { get; }

		public IsDateAfterAttribute(string testedPropertyName, bool allowEqualDates = false)
		{
			TestedPropertyName = testedPropertyName;
			this.allowEqualDates = allowEqualDates;
		}

		public string GetErrorMessage() => $"Must be later than {TestedPropertyName}.";

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var propertyTestedInfo = validationContext.ObjectType.GetProperty(TestedPropertyName);
			if(propertyTestedInfo == null)
			{
				return new ValidationResult($"Unknown property ${TestedPropertyName}");
			}

			var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

			if((DateTime)value >= (DateTime)propertyTestedValue)
			{
				if(allowEqualDates && (DateTime)value == (DateTime)propertyTestedValue)
				{
					return ValidationResult.Success;
				}
				else if((DateTime)value > (DateTime)propertyTestedValue)
				{
					return ValidationResult.Success;
				}
			}

			return new ValidationResult(GetErrorMessage());
		}

		public void AddValidation(ClientModelValidationContext context)
		{
			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, "data-val-isdateafter", GetErrorMessage());
			MergeAttribute(context.Attributes, "data-val-isdateafter-testpropertyname", TestedPropertyName);
		}

		private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
		{
			if (attributes.ContainsKey(key))
			{
				return false;
			}

			attributes.Add(key, value);
			return true;
		}
	}
}
