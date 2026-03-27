using TerminalWrapper;
using TerminalWrapper.Console;

namespace ResultTests.Tasks;

public class Fate10x10SubTerminal : MainTask
{
    public override string TaskName => "Fate 10x10 Terminal";

    private readonly int m_diceFace;
    private readonly string m_outputPath;

    public Fate10x10SubTerminal(int diceFace, string outputPath)
    {
        m_diceFace = diceFace;
        m_outputPath = $"{outputPath}/Fate10x10";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        MainTask[] tasks =
        [
            new FateResultTask(m_diceFace, 1, 10, 10, m_outputPath),
            new FateResultTask(m_diceFace, 2, 10, 10, m_outputPath),
            new FateResultTask(m_diceFace, 3, 10, 10, m_outputPath),
            new FateResultTask(m_diceFace, 4, 10, 10, m_outputPath),
            new FateResultTask(m_diceFace, 5, 10, 10, m_outputPath),
            new FateResultTask(m_diceFace, 6, 10, 10, m_outputPath),
            new FateResultTask(m_diceFace, 7, 10, 10, m_outputPath),
        ];

        ConsoleTerminal terminal = ConsoleTerminal.CreateTerminal(tasks);
        await terminal.RunAsync();
    }
}
