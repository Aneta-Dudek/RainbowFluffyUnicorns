using Questore.Dtos;

namespace Questore.Persistence
{
    public interface IDetailsDAO
    {
        public void AddDetail(DetailDto detailDto);

        public void DeleteDetail(int id);
    }
}
