namespace ElectronicInvoiceAdE.Validation;

public enum ValidationSeverity
{
    Error,
    Warning
}

/// <summary>
/// Represents a validation error or warning according to official SDI Technical Specifications.
/// </summary>
public record ValidationError(
    string ErrorCode,
    string Message,
    string PropertyPath,
    ValidationSeverity Severity = ValidationSeverity.Error)
{
    public override string ToString() => $"[{ErrorCode}] {PropertyPath}: {Message}";
}

public class ValidationResult
{
    public List<ValidationError> Errors { get; } = new();

    public bool IsValid => Errors.All(e => e.Severity != ValidationSeverity.Error);

    public void AddError(string errorCode, string message, string propertyPath)
    {
        Errors.Add(new ValidationError(errorCode, message, propertyPath, ValidationSeverity.Error));
    }

    public void AddWarning(string errorCode, string message, string propertyPath)
    {
        Errors.Add(new ValidationError(errorCode, message, propertyPath, ValidationSeverity.Warning));
    }
}
