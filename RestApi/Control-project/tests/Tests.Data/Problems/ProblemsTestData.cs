using Domain.Problems;

namespace Tests.Data.Problems;

public static class ProblemsTestData
{
    public static Problem GetFirstTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "First gettable problem",
        code: "return \"4\".ToString();",
        answer: "\"4\"",
        category: Category.String,
        difficulty: Difficulty.Medium);
    
    public static Problem GetSecondTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "Second gettable problem",
        code: "return \"Hello\";",
        answer: "\"Hello\"",
        category: Category.String,
        difficulty: Difficulty.Easy);
    
    public static Problem CreateValidTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "Valid creation problem",
        code: "Console.Writeline(4);",
        answer: "\"4\"",
        category: Category.String,
        difficulty: Difficulty.Easy);
    
    public static Problem CreateTakenTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "Already created creation problem",
        code: "Console.Writeline('2');",
        answer: "\"2\"",
        category: Category.String,
        difficulty: Difficulty.Easy);
    
    public static Problem CreateInvalidTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "V",
        code: "Console.Writeline(\"-\");",
        answer: "\"-\"",
        category: Category.String,
        difficulty: Difficulty.Easy);
    
    public static Problem UpdateValidTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "Valid problem for updating",
        code: "Console.Writeline();",
        answer: "\\n",
        category: Category.String,
        difficulty: Difficulty.Easy);
    
    public static Problem UpdateTakenTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "Already created problem for updating",
        code: "return 3 + 4.1;",
        answer: "7.1",
        category: Category.Math,
        difficulty: Difficulty.Medium);
    
    public static Problem UpdateInvalidTestProblem() => Problem.New(
        id: ProblemId.New(),
        title: "d",
        code: "Console.Writeline(\"-\");",
        answer: "\"-\"",
        category: Category.String,
        difficulty: Difficulty.Easy);
    
    public static ProblemId NonExistentTestProblemId() => ProblemId.New();
    
}