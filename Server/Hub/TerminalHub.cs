namespace EFMigrationService.Server.Hub;

using CliWrap;

using EFMigrationService.Server.EFCommandHandler;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Represents a SignalR hub for managing terminal commands and interactions between clients and the server.
/// </summary>
/// <param name="logger">The logger used to log terminal events.</param>
/// <param name="commandHandler">The command handler responsible for executing terminal commands.</param>
internal class TerminalHub(
    ILogger<TerminalHub> logger,
    EFCommandHandler commandHandler)
    : Hub<ITerminalClient>
{
    /// <summary>
    /// Invoked when a client connects to the hub.
    /// Sends a welcome message and the result of a empty command to the client.
    /// </summary>
    /// <inheritdoc/>
    public override async Task OnConnectedAsync()
    {
        var caller = this.Clients.Caller;

        await caller.ReceiveLine("Connected to server");
        await this.ExecuteAndSendCommandResult(caller, string.Empty);
    }

    /// <summary>
    /// Receives a command from the client and executes it.
    /// Logs the received command and sends the result back to the client.
    /// </summary>
    /// <param name="command">The command sent by the client for execution.</param>
    /// <returns>A task that represents the asynchronous operation of processing the command.</returns>
    public Task ReceiveCommand(string command)
    {
        logger.LogInformation("Received command. Command: \"{Command}\"", command);

        return this.ExecuteAndSendCommandResult(this.Clients.Caller, command);
    }

    /// <summary>
    /// Executes the specified command and sends the result to the client.
    /// <para>
    /// If a command is already being executed, it informs the client that the command is in the queue.
    /// </para>
    /// </summary>
    /// <param name="client">The terminal client that will receive the command results.</param>
    /// <param name="command">The command to be executed.</param>
    /// <returns>
    /// A task that represents the asynchronous operation of executing the command and sending the results to the client.
    /// </returns>
    private async Task ExecuteAndSendCommandResult(ITerminalClient client, string command)
    {
        if (commandHandler.ExecutingCommand)
        {
            await client.ReceiveLine("Command in queue...");

            logger.LogInformation("Command added to queue. Command: \"{Command}\"", command);
        }

        try
        {
            await commandHandler.Execute(
                command: command,
                outputPipe: PipeTarget.ToDelegate(client.ReceiveLine),
                onCommandExecuting: () => logger.LogInformation("Executing command. Command: \"{Command}\"", command));
        }
        finally
        {
            await client.EndOfResponse();

            logger.LogInformation("The command finished executing. \"{Command}\"", command);
        }
    }
}
