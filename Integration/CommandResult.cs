namespace EFMigrationService.Integration;

/// <summary>
/// Represents the result of executing a command, including standard output and standard error messages.
/// </summary>
/// <param name="StandardOutput">The output produced by the command during execution.</param>
/// <param name="StandardError">The error output produced by the command.</param>
internal record CommandResult(
    string StandardOutput,
    string StandardError);
