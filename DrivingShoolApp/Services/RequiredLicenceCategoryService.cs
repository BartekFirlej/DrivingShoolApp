using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;


namespace DrivingSchoolApp.Services
{
    public interface IRequiredLicenceCategoryService
    {
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements();
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId);
        public Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<RequiredLicenceCategoryGetDTO> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails);
    }
    public class RequiredLicenceCategoryService : IRequiredLicenceCategoryService
    {
        private readonly IRequiredLicenceCategoryRepository _requiredLicenceCategoryRepository;
        private readonly ILicenceCategoryService _licenceCategoryService;

        public RequiredLicenceCategoryService(IRequiredLicenceCategoryRepository requiredLicenceCategoryRepository, ILicenceCategoryService licenceCategoryService)
        {
            _requiredLicenceCategoryRepository = requiredLicenceCategoryRepository;
            _licenceCategoryService = licenceCategoryService;
        }

        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements()
        {
            var requiredLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirements();
            if (!requiredLicenceCategories.Any())
                throw new NotFoundRequiredLicenceCategoryException();
            return requiredLicenceCategories;
        }

        public async Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId)
        {
            var requiredLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirement(licenceCategoryId, requiredLicenceCategoryId);
            if (requiredLicenceCategories == null)
                throw new NotFoundRequiredLicenceCategoryException(licenceCategoryId, requiredLicenceCategoryId);
            return requiredLicenceCategories;
        }

        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId)
        {
            var requiredLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirements(licenceCategoryId);
            if (!requiredLicenceCategories.Any())
                throw new NotFoundRequiredLicenceCategoryException(licenceCategoryId);
            return requiredLicenceCategories;
        }

        public async Task<RequiredLicenceCategoryGetDTO> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails)
        {
            var licenceCategory = await _licenceCategoryService.GetLicenceCategory(requirementDetails.LicenceCategoryId);
            var requiredLicenceCategory = await _licenceCategoryService.GetLicenceCategory(requirementDetails.RequiredLicenceCategoryId);
            var addedRequiredLicenceCategory = await _requiredLicenceCategoryRepository.PostRequirement(requirementDetails);
            return await _requiredLicenceCategoryRepository.GetRequirement(addedRequiredLicenceCategory.LicenceCategoryId, addedRequiredLicenceCategory.RequiredLicenceCategoryId);
        }
    }
}
