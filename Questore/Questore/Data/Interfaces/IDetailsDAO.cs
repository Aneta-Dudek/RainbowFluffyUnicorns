using Questore.Data.Dtos;

namespace Questore.Data.Interfaces
{
    public interface IDetailsDAO
    {
        public void AddDetail(DetailDto detailDto);

        public void DeleteDetail(int id);
    }
}
