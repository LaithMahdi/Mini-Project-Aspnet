using System.ComponentModel.DataAnnotations;

namespace school.Models.Validation
{
    /// <summary>
    /// Validates that a DateOnly is not in the past (must be today or later).
    /// Used for SessionDate fields.
    /// </summary>
    public class NoPastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is DateOnly dateOnly)
            {
                return dateOnly >= DateOnly.FromDateTime(DateTime.Today);
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} cannot be in the past.";
        }
    }
}
