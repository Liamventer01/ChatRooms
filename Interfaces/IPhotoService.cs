using CloudinaryDotNet.Actions;

namespace ChatRooms.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddThumbnailAsync(IFormFile file);
        Task<ImageUploadResult> AddProfilePictureAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
