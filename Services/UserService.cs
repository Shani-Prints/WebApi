
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class UserService : IUserServise
    {
        int nextId;
        string text;
        List<User>? userList { get; }
        ITokenService tokenService;

        public UserService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
            text = Path.Combine(
                "Data",
                "User.json"
            );

            using (var jsonOpen = File.OpenText(text))
            {
                userList = JsonSerializer.Deserialize<List<User>>(jsonOpen.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            nextId = userList != null ? userList[userList.Count - 1].Id + 1 : 1;
        }

        private void saveToFile()
        {
            File.WriteAllText(text, JsonSerializer.Serialize(userList));
        }

        public List<User>? GetAll() => userList;

        public User? Get(int id) => userList?.FirstOrDefault(j => j.Id == id);

        public void Add(User user)
        {
            user.Id = nextId++;
            userList?.Add(user);
            saveToFile();
        }
        public void Update(User user, string Role)
        {
            if (userList == null)
                throw new InvalidOperationException("The User list has not been initialized.");
            var index = userList.FindIndex(j => j.Id == user.Id);
            if (index == -1)
                return;
            user.Role = Role;
            userList[index] = user;
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;
            userList?.Remove(user);
            saveToFile();
        }

        public int Count { get => userList?.Count() ?? 0; }

        public ActionResult Login(User user)
        {
            User? findUser = userList?.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (findUser == null)
                return new NotFoundObjectResult("User not found");

            string? type = findUser.Role;
            var claims = new List<Claim>
            {
                new Claim("id", findUser.Id.ToString()),
                new Claim("userName", findUser.Username ?? "null"),
                new Claim("type", type??"User"),
            };

            var token = tokenService.GetToken(claims);

            return new OkObjectResult(tokenService.WriteToken(token));
        }
    }

    public static class UserServiceHelper
    {
        public static void AddUserService(this IServiceCollection services)
        {
            services.AddSingleton<IUserServise, UserService>();
        }
    }
}