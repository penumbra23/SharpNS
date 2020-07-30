using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SharpNS.Validation.Attributes
{
    public class DomainAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value as string))
                return new ValidationResult("Domain field missing");
            var match = Regex.IsMatch(value as string, "^((([a-zA-Z0-9][a-zA-Z0-9-]*[a-zA-Z0-9])+|\\*)\\.)+[a-zA-Z0-9]{2,}$");
            return match ? ValidationResult.Success : new ValidationResult("Domain specified doesn't comply the domain format");
        }
    }
}
