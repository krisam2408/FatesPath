using TerminalWrapper;
using TerminalWrapper.Console;

namespace ResultTests.Tasks;

public class Fate15x15SubTerminal : MainTask
{
    public override string TaskName => "Fate 15x15 Terminal";

    private readonly int m_diceFace;
    private readonly string m_outputPath;

    public Fate15x15SubTerminal(int diceFace, string outputPath)
    {
        m_diceFace = diceFace;
        m_outputPath = $"{outputPath}/Fate15x15";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        MainTask[] tasks =
        [
            new FateResultTask(m_diceFace, 1, 15, 15, m_outputPath),
            new FateResultTask(m_diceFace, 2, 15, 15, m_outputPath),
            new FateResultTask(m_diceFace, 3, 15, 15, m_outputPath),
            new FateResultTask(m_diceFace, 4, 15, 15, m_outputPath),
            new FateResultTask(m_diceFace, 5, 15, 15, m_outputPath),
            new FateResultTask(m_diceFace, 6, 15, 15, m_outputPath),
            new FateResultTask(m_diceFace, 7, 15, 15, m_outputPath),
        ];

        ConsoleTerminal terminal = ConsoleTerminal.CreateTerminal(tasks);
        await terminal.RunAsync();
    }
}
