using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Validation;

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
