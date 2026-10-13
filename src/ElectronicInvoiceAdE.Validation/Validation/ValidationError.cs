namespace ElectronicInvoiceAdE.Validation;

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
