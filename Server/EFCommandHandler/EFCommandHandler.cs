namespace EFMigrationService.Server.EFCommandHandler;

using CliWrap;
using CliWrap.Buffered;
using CliWrap.Builders;
using CliWrap.Exceptions;

using RaptorUtils.Threading;

/// <summary>
/// Handles the execution of Entity Framework (EF) CLI commands in a thread-safe manner.
/// <para>
/// This allows for the coordination of multiple command executions while preventing concurrent runs.
/// </para>
/// </summary>
/// <param name="args">The arguments to pass to the EF CLI commands.</param>
internal class EFCommandHandler(IEnumerable<string> args)
{
    private readonly AsyncLock commandLock = new();

    private readonly string args = CreateArgs(args);

    /// <summary>
    /// Initializes a new instance of the <see cref="EFCommandHandler"/> class with the specified arguments.
    /// This constructor allows passing a variable number of arguments.
    /// </summary>
    /// <param name="args">The arguments to pass to the EF CLI commands.</param>
    public EFCommandHandler(params string[] args)
        : this((IEnumerable<string>)args)
    {
    }

    /// <summary>
    /// Gets a value indicating whether a command is currently being executed.
    /// </summary>
    public bool ExecutingCommand => this.commandLock.Locked;

    /// <summary>
    /// Executes the specified EF command with the provided arguments.
    /// The command output is piped to the specified target,
    /// and an optional action can be executed before the command starts.
    /// </summary>
    /// <param name="command">The EF command to execute (e.g., "migrations add").</param>
    /// <param name="outputPipe">The pipe to capture the command's output.</param>
    /// <param name="onCommandExecuting">An optional action to perform when the command is about to be executed.</param>
    /// <returns>A task that represents the asynchronous execution of the command.</returns>
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

    /// <summary>
    /// Builds the argument string from the provided IEnumerable of arguments.
    /// </summary>
    /// <param name="args">The collection of arguments to be processed.</param>
    /// <returns>A formatted string of arguments.</returns>
    private static string CreateArgs(IEnumerable<string> args)
    {
        return new ArgumentsBuilder().Add(args).Build();
    }
}
