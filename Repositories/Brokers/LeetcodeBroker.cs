using GraphqlLeetcode.Models;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;

namespace GraphqlLeetcode.Repositories.Brokers;

internal class LeetcodeBroker : ILeetCodeBroker
{
    private string baseUrl = "https://leetcode.com/graphql";
    public async Task<DailyProblem> GetDailyProblemAsync()
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(baseUrl);
            var graphqlRequest = new GraphQLRequest
            {
                Query = @"
                    query questionOfToday {
                        activeDailyCodingChallengeQuestion {
                            date
                            link
                            question{
                                difficulty
                                title
                                topicTags {
                                 name
                                }
                            }
                        }
                    }"

            };

            var requestContent = new StringContent(
                content: JsonSerializer.Serialize(
                    value: graphqlRequest,
                    options: new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }),
                encoding: Encoding.UTF8,
                mediaType: "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(
                requestUri: string.Empty,
                content: requestContent);

            var contentString = await response.Content.ReadAsStringAsync();
            JsonNode? jsonObject = JsonObject.Parse(contentString);

            return GetDailyProblem(jsonObject);
        }
    }

    public async Task<List<Submission>> GetTotalSolvedProblemsCountAsync(string leetcodeUsername)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(baseUrl);

            var graphqlRequest = new GraphQLRequest
            {
                Query = @"
                    query ($username: String!) {
                        matchedUser(username: $username) {
                            submitStats {
                                acSubmissionNum {
                                    difficulty
                                    count
                                    submissions
                                }
                            }
                        }
                    }",
                Variables = new
                {
                    Username = leetcodeUsername
                }
            };

            var requestContent = new StringContent(
                content: JsonSerializer.Serialize(
                    value: graphqlRequest,
                    options: new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }),
                encoding: Encoding.UTF8,
                mediaType: "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(
                requestUri: string.Empty,
                content: requestContent);

            var contentString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonObject.Parse(contentString);

            if (jsonObject?["errors"] is not null)
            {
                //this.logger.LogError($"Leetcode username {leetcodeUsername} not found");
                return null ;
            }

            List<Submission>? solvedProblems = jsonObject?["data"]?["matchedUser"]?["submitStats"]["acSubmissionNum"]
                .Deserialize<List<Submission>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
            return solvedProblems ?? new List<Submission>();
            
        }
    }

    public DailyProblem GetDailyProblem(JsonNode? jsonNode)
    {
        return new DailyProblem()
        {
            Date = jsonNode?["data"]?
                            ["activeDailyCodingChallengeQuestion"]?
                            ["date"]
                            .GetValue<string>(),
            Difficulty = jsonNode?["data"]
                                  ["activeDailyCodingChallengeQuestion"]?
                                  ["question"]?
                                  ["difficulty"]?
                                  .GetValue<string>(),
            Link = jsonNode?["data"]?
                            ["activeDailyCodingChallengeQuestion"]?
                            ["link"]?
                            .GetValue<string>(),
            Tags = string.Join(", ", jsonNode?["data"]?
                            ["activeDailyCodingChallengeQuestion"]?
                            ["question"]?
                            ["topicTags"]?.AsArray().Select(tag => tag["name"])),
            Title = jsonNode?["data"]?
                                        ["activeDailyCodingChallengeQuestion"]?
                                        ["question"]?
                                        ["title"]?
                                        .GetValue<string>()
        };
    }
}
