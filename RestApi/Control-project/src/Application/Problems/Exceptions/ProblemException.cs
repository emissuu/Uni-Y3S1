using Domain.Problems;

namespace Application.Problems.Exceptions;

public abstract class ProblemException(ProblemId problemId, string message, Exception? innerException = null) 
    : Exception(message, innerException)
{
    public ProblemId ProblemId { get; } = problemId;
}

public class ProblemAlreadyExistsException(ProblemId problemId)
    : ProblemException(problemId, $"Problem already exists under id {problemId}");
    
public class ProblemNotFoundException(ProblemId problemId) 
    : ProblemException(problemId, $"Problem not found under id {problemId}");
    
public class ProblemUnhandledException(ProblemId problemId, Exception? innerException = null)
    : ProblemException(problemId, $"Unhandled problem under id {problemId}", innerException);