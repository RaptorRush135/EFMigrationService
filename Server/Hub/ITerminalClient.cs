namespace EFMigrationService.Server.Hub;

public interface ITerminalClient
{
    Task ReceiveLine(string line);

    Task EndOfResponse();
}
