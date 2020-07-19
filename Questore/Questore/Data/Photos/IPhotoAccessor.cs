using Microsoft.AspNetCore.Http;
using Questore.Data.Dtos;

namespace Questore.Data.Photos
{
    public interface IPhotoAccessor
    {
        public PhotoUploadResult AddPhoto(IFormFile file);
        public string DeletePhoto(string photoId);
    }
}
