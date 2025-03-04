using Microsoft.AspNetCore.Mvc;

namespace WebApi.Interfaces
{
    public interface IUserServise
    {
        List<User>? GetAll();

        User? Get(int id);

        void Add(User user);

        void Delete(int id);

        void Update(User user, string Role);

        int Count { get; }
        public ActionResult Login(User user);
    }
}
