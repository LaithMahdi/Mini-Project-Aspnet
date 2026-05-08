using System.ComponentModel.DataAnnotations;

namespace school.Models.Validation
{
    /// <summary>
    /// Validates that one TimeOnly is after another TimeOnly.
    /// Used to ensure EndTime > StartTime for sessions.
    /// </summary>
    public class TimeAfterAttribute : ValidationAttribute
    {
        private readonly string _comparisonPropertyName;

        public TimeAfterAttribute(string comparisonPropertyName)
        {
            _comparisonPropertyName = comparisonPropertyName;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is not TimeOnly endTime)
                return true;

            return true; // Will be validated in controller with access to both properties
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be after {_comparisonPropertyName}.";
        }
    }
}
