namespace EFMigrationService.Server.Hub;

/// <summary>
/// Represents a client interface for a terminal connection, providing methods
/// for receiving output lines and signaling the end of a response.
/// </summary>
public interface ITerminalClient
{
    /// <summary>
    /// Sends a line of output to the client.
    /// <para>
    /// This method is typically used to communicate status messages, command results,
    /// or any text output to the terminal client.
    /// </para>
    /// </summary>
    /// <param name="line">The line of text to be sent to the client.</param>
    /// <returns>A task representing the asynchronous operation of sending the line.</returns>
    Task ReceiveLine(string line);

    /// <summary>
    /// Signals to the client that the response from a command execution has ended.
    /// <para>
    /// This method is used to notify the client that no further output will be sent for the current command.
    /// </para>
    /// </summary>
    /// <returns>A task representing the asynchronous operation of signaling the end of the response.</returns>
    Task EndOfResponse();
}
