namespace EFMigrationService.Server.Hub;

using CliWrap;

using EFMigrationService.Server.EFCommandHandler;

using Microsoft.AspNetCore.SignalR;

internal class TerminalHub(
    ILogger<TerminalHub> logger,
    EFCommandHandler commandHandler)
    : Hub<ITerminalClient>
{
    public override async Task OnConnectedAsync()
    {
        var caller = this.Clients.Caller;

        await caller.ReceiveLine("Connected to server");
        await this.ExecuteAndSendCommandResult(caller, string.Empty);
    }

    public Task ReceiveCommand(string command)
    {
        logger.LogInformation("Received command. Command: \"{Command}\"", command);

        return this.ExecuteAndSendCommandResult(this.Clients.Caller, command);
    }

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
