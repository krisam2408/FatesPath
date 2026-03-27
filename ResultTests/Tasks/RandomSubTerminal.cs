using ResultTests.Tasks.Random;
using TerminalWrapper;
using TerminalWrapper.Console;

public class RandomSubTerminal : MainTask
{
    public override string TaskName => "Random Terminal";

    private readonly int m_diceFace;
    private readonly string m_outputPath;

    public RandomSubTerminal(int faces, string path)
    {
        m_diceFace = faces; 
        m_outputPath = $"{path}/Random/";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        MainTask[] tasks =
        [
            new RandomResultTask(m_diceFace, 1, m_outputPath),
            new RandomResultTask(m_diceFace, 2, m_outputPath),
            new RandomResultTask(m_diceFace, 3, m_outputPath),
            new RandomResultTask(m_diceFace, 4, m_outputPath),
            new RandomResultTask(m_diceFace, 5, m_outputPath),
            new RandomResultTask(m_diceFace, 6, m_outputPath),
            new RandomResultTask(m_diceFace, 7, m_outputPath),
            new RandomResultTask(m_diceFace, 8, m_outputPath),
            new RandomResultTask(m_diceFace, 9, m_outputPath),
        ];

        ConsoleTerminal subTerminal = ConsoleTerminal.CreateTerminal(tasks);
        await subTerminal.RunAsync();
    }
}
