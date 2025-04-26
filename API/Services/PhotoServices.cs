using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoServices : IPhotoServices
    {
        private readonly Cloudinary _cloudinary;
        public PhotoServices(IOptions<CloudinarySettings> _options)
        {
            var acc = new Account(_options.Value.CloudName,
            _options.Value.ApiKey, _options.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile)
        {
            var uploadResult = new ImageUploadResult();
            try
            {
                if (formFile.Length > 0)
                {
                    using var stream = formFile.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(formFile.FileName, stream),
                        Transformation = new Transformation()
                        .Height(500).Width(500).Crop("fill").Gravity("face"),
                        Folder = "da-net8"
                    };
                    uploadResult=await _cloudinary.UploadAsync(uploadParams);
                }
            }
            catch (Exception ex)
            { }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParans = new DeletionParams(publicId);


            return await _cloudinary.DestroyAsync(deleteParans);
        }
    }
}