using TerminalWrapper;
using TerminalWrapper.Console;

namespace ResultTests.Tasks;

public class Fate3x3SubTerminal : MainTask
{
    public override string TaskName => "Fate 3x3 Terminal";

    private readonly int m_diceFace;
    private readonly string m_outputPath;

    public Fate3x3SubTerminal(int diceFace, string outputPath)
    {
        m_diceFace = diceFace;
        m_outputPath = $"{outputPath}/Fate3x3";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        MainTask[] tasks =
        [
            new FateResultTask(m_diceFace, 1, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 2, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 3, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 4, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 5, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 6, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 7, 3, 3, m_outputPath),
            new FateResultTask(m_diceFace, 8, 3, 3, m_outputPath),
        ];

        ConsoleTerminal terminal = ConsoleTerminal.CreateTerminal(tasks);
        await terminal.RunAsync();
    }
}
