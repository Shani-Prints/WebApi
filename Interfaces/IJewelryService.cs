using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
namespace WebApi.Interfaces
{

    public interface IJewelryService
    {
        List<Jewelry> GetAll();
        Jewelry Get(int id);
        void Add(Jewelry jewelry);
        void Delete(int id);
        void Update(Jewelry jewelry);
        int Count { get; }
    }
}
