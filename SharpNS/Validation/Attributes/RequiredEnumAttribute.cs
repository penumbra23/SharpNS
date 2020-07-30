using System;
using System.ComponentModel.DataAnnotations;

namespace SharpNS.Validation.Attributes
{
    public class RequiredEnumAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Enumeration field is required");

            Type type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value) ? 
                ValidationResult.Success
                : new ValidationResult("Value supplied is not valid for this enumeration");
        }
    }
}
