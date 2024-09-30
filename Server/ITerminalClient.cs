namespace MrHotel.MigrationService;

public interface ITerminalClient
{
    Task ReceiveLine(string line);

    Task EndOfResponse();
}
