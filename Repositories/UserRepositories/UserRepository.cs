using GraphqlLeetcode.Models;
using System.Text.Json;

namespace GraphqlLeetcode.Repositories.UserRepositories;
    internal class UserRepository : IUserRepository
    {
        public async Task WriteToFileAsync(List<TelegramUser> users)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\user\Desktop\Lesson\Lesson8Task\GraphqlLeetcode\users.json"))
            {
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                string jsonData = JsonSerializer.Serialize(users, typeof(List<TelegramUser>), options);

                await sw.WriteAsync(jsonData);
            }
        }

        public async Task<List<TelegramUser>> ReadFromFile()
        {
            using (StreamReader streamReader = new StreamReader(@"C:\Users\user\Desktop\Lesson\Lesson8Task\GraphqlLeetcode\users.json"))
            {
                string jsonData = await streamReader.ReadToEndAsync();

                List<TelegramUser> users = JsonSerializer.Deserialize<List<TelegramUser>>(jsonData);

                return users;
            }
        }
    }


