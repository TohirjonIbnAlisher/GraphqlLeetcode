using GraphqlLeetcode.Models;

namespace GraphqlLeetcode.Repositories.UserRepositories;

internal interface IUserRepository
{
    public Task WriteToFileAsync(List<TelegramUser> users);
    public Task<List<TelegramUser>> ReadFromFile();
}
