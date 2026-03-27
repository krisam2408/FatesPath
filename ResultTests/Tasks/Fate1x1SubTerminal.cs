using TerminalWrapper;
using TerminalWrapper.Console;

namespace ResultTests.Tasks;

public class Fate1x1SubTerminal : MainTask
{
    public override string TaskName => "Fate 1x1 Terminal";

    private readonly int m_diceFace;
    private readonly string m_outputPath;

    public Fate1x1SubTerminal(int diceFace, string outputPath)
    {
        m_diceFace = diceFace;
        m_outputPath = $"{outputPath}/Fate1x1";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        MainTask[] tasks =
        [
            new FateResultTask(m_diceFace, 1, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 2, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 3, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 4, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 5, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 6, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 7, 1, 1, m_outputPath),
            new FateResultTask(m_diceFace, 8, 1, 1, m_outputPath),
        ];

        ConsoleTerminal terminal = ConsoleTerminal.CreateTerminal(tasks);
        await terminal.RunAsync();
    }
}
