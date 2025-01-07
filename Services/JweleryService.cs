using WebApi.Services;
using WebApi.Models;
using WebApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Services
{
    public class JweleryService : IJewelryService
    {
        List<Jewelry> jewelryList { get; }
        int nextId = 4;
        public JweleryService()
        {
            jewelryList = new List<Jewelry>
        {
            new Jewelry { Id = 1, Name = "Pearl necklace",Price=1500,Category="necklace" },
            new Jewelry { Id = 2, Name = "Pandora bracelet",Price=350,Category="bracelet" },
            new Jewelry { Id = 3, Name = "Gold watch",Price=6000,Category="watch" },
        };
        }
        public List<Jewelry> GetAll() => jewelryList;
        public Jewelry Get(int id) => jewelryList.FirstOrDefault(j => j.Id == id);
        public void Add(Jewelry jewelry)
        {
            jewelry.Id = nextId++;
            jewelryList.Add(jewelry);
        }

        public void Update(Jewelry jewelry)
        {
            var index = jewelryList.FindIndex(j => j.Id == jewelry.Id);
            if (index == -1)
                return;
            jewelryList[index] = jewelry;
        }

        public void Delete(int id)
        {
            var jewelry = Get(id);
            if (jewelry is null)
                return;
            jewelryList.Remove(jewelry);
        }

        public int Count { get => jewelryList.Count(); }
    }
    public static class JweleryServiceHelper
    {
        public static void AddJweleryService(this IServiceCollection services)
        {
            services.AddSingleton<IJewelryService, JweleryService>();

        }
    }

}