using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface IRankService
    {
        public Task<Rank> ShowAsync(int rankId);
        public Task<List<Rank>> ShowAll();
    }
}
