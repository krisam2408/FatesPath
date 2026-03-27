using TerminalWrapper;
using TerminalWrapper.Console;

namespace ResultTests.Tasks;

public class Fate5x5SubTerminal : MainTask
{
    public override string TaskName => "Fate 5x5 Terminal";

    private readonly int m_diceFace;
    private readonly string m_outputPath;

    public Fate5x5SubTerminal(int diceFace, string outputPath)
    {
        m_diceFace = diceFace;
        m_outputPath = $"{outputPath}/Fate5x5";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        MainTask[] tasks =
        [
            new FateResultTask(m_diceFace, 1, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 2, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 3, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 4, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 5, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 6, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 7, 5, 5, m_outputPath),
            new FateResultTask(m_diceFace, 8, 5, 5, m_outputPath),
        ];

        ConsoleTerminal terminal = ConsoleTerminal.CreateTerminal(tasks);
        await terminal.RunAsync();
    }
}
