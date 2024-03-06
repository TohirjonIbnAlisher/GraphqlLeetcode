using GraphqlLeetcode.Models;

namespace GraphqlLeetcode.Repositories.Brokers;

public interface ILeetCodeBroker
{
    Task<List<Submission>> GetTotalSolvedProblemsCountAsync(string leetcodeUsername);
    Task<DailyProblem> GetDailyProblemAsync();
}
