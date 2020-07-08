using Microsoft.AspNetCore.Http;
using Questore.Dtos;

namespace Questore.Photos
{
    public interface IPhotoAccessor
    {
        public PhotoUploadResult AddPhoto(IFormFile file);

        public string DeletePhoto(string photoId);
    }
}
