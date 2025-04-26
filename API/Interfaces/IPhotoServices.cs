using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoServices
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile);

        public Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}