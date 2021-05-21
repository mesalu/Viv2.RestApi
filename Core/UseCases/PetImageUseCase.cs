using System;
using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public class PetImageUseCase : IPetImageUseCase
    {
        private readonly IBlobStore _blobStore;
        private readonly IUserStore _userStore;
        private readonly IPetStore _petStore;
        private readonly IEntityFactory _entityFactory;

        public PetImageUseCase(IUserStore userStore, IPetStore petStore, 
            IBlobStore blobStore, IEntityFactory entityFactory)
        {
            _userStore = userStore;
            _blobStore = blobStore;
            _petStore = petStore;
            _entityFactory = entityFactory;
        }

        public async Task<bool> Handle(PetImageRequest message, IOutboundPort<BlobUriResponse> outputPort)
        {
            // verify that the user has access to the specified pet:
            var user = await _userStore.GetUserById(message.UserId);
            if (user == null) return false;

            await _userStore.LoadPets(user);
            var pet = user.Pets.FirstOrDefault(p => p.Id == message.PetId);
            if (pet == null) return false;

            // All pet images go to the image container/category.
            const string category = Constants.StorageCategories.Images;
            string blobName; // set based on execution path.
            
            if (message.Update)
            {
                // Generate a new blob name (always) for pet images - ensures no conflict. 
                blobName = $"{message.UserId}_{message.PetId}_{Guid.NewGuid()}";

                // upload the blob to storage.
                await _blobStore.WriteBlob(category, blobName, message.MimeType, message.Content);

                // associate a new blob entry to the pet
                var entity = _entityFactory.GetBlobRecordBuilder()
                    .SetCategory(category)
                    .SetName(blobName)
                    .Build();

                await _petStore.UpdateImage(pet, entity);
            }
            else
            {
                // load the current record from pet:
                await _petStore.LoadImageRecord(pet);
                blobName = pet.ProfileImage?.BlobName;
            }

            // check that there exists a blob associated to pet by this point.
            if (blobName == null) return false;
            
            var response = new BlobUriResponse
            {
                Uri = await _blobStore.GetBlobReadUri(category, blobName, 60, 60),
                ExpiresIn = 60
            };
            
            outputPort.Handle(response);
            return true;
        }
    }
}