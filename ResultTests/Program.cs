using ResultTests.Tasks;
using TerminalWrapper;
using TerminalWrapper.Console;

int diceFaces = 10;
string outputPath = "../../../output/";

MainTask[] tasks =
[
    new RandomSubTerminal(diceFaces, outputPath),
    new Fate1x1SubTerminal(diceFaces, outputPath),
    new Fate3x3SubTerminal(diceFaces, outputPath),
    new Fate5x5SubTerminal(diceFaces, outputPath),
    new Fate10x10SubTerminal(diceFaces, outputPath),
    new Fate15x15SubTerminal(diceFaces, outputPath),
    new TimeReviewComparisonTask(outputPath)
];

ConsoleTerminal terminal = ConsoleTerminal.CreateTerminal(tasks);

await terminal.RunAsync();