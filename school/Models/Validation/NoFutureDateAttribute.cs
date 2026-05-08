using System.ComponentModel.DataAnnotations;

namespace school.Models.Validation
{
    /// <summary>
    /// Validates that a DateTime is not in the future (must be today or earlier).
    /// Used for DateOfBirth fields.
    /// </summary>
    public class NoFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is DateTime dateTime)
            {
                return dateTime.Date <= DateTime.Today;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} cannot be in the future.";
        }
    }
}
