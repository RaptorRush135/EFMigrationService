namespace EFMigrationService.Server.EFCommandHandler;

using CliWrap;
using CliWrap.Buffered;
using CliWrap.Builders;
using CliWrap.Exceptions;

using RaptorUtils.Threading;

internal class EFCommandHandler(IEnumerable<string> args)
{
    private readonly AsyncLock commandLock = new();

    private readonly string args = CreateArgs(args);

    public EFCommandHandler(params string[] args)
        : this((IEnumerable<string>)args)
    {
    }

    public bool ExecutingCommand => this.commandLock.Locked;

    public async Task Execute(string command, PipeTarget outputPipe, Action? onCommandExecuting)
    {
        using var scope = await this.commandLock.LockAsync();

        onCommandExecuting?.Invoke();

        try
        {
            await Cli.Wrap("dotnet")
                .WithArguments($"tool run dotnet-ef {command} {this.args}")
                .WithStandardOutputPipe(outputPipe)
                .ExecuteBufferedAsync();
        }
        catch (CommandExecutionException ex) when (ex.ExitCode == 1)
        {
            // Ignore exit code 1
        }
    }

    private static string CreateArgs(IEnumerable<string> args)
    {
        return new ArgumentsBuilder().Add(args).Build();
    }
}
