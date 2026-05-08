using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace school.Models.Validation
{
    /// <summary>
    /// Validates that a string contains only alphabetic characters, spaces, and hyphens.
    /// Rejects numeric and special characters.
    /// Used for name fields (FullName, FirstName, LastName, etc.).
    /// </summary>
    public class AlphabeticalNameAttribute : ValidationAttribute
    {
        private static readonly Regex NamePattern = new Regex(@"^[a-zA-Zàâäæçéèêëìîïñòôöœùûüœ\s\-']+$", RegexOptions.Compiled);

        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true;

            string stringValue = value.ToString()!;
            return NamePattern.IsMatch(stringValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} can only contain letters, spaces, hyphens, and apostrophes. Numbers are not allowed.";
        }
    }
}
