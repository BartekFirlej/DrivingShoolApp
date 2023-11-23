using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;


namespace DrivingSchoolApp.Services
{
    public interface IRequiredLicenceCategoryService
    {
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements();
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId);
        public Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<RequiredLicenceCategory> CheckRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<RequiredLicenceCategoryGetDTO> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails);
        public Task<RequiredLicenceCategory> DeleteRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<bool> MeetRequirements(ICollection<DrivingLicence> drivingLicences, int licenceCategoryId, DateTime receiveDate);
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
            await _licenceCategoryService.CheckLicenceCategory(licenceCategoryId);
            await _licenceCategoryService.CheckLicenceCategory(requiredLicenceCategoryId);
            var requiredLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirement(licenceCategoryId, requiredLicenceCategoryId);
            if (requiredLicenceCategories == null)
                throw new NotFoundRequiredLicenceCategoryException(licenceCategoryId, requiredLicenceCategoryId);
            return requiredLicenceCategories;
        }

        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId)
        {
            await _licenceCategoryService.CheckLicenceCategory(licenceCategoryId);
            var requiredLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirements(licenceCategoryId);
            if (!requiredLicenceCategories.Any())
                throw new NotFoundRequiredLicenceCategoryException(licenceCategoryId);
            return requiredLicenceCategories;
        }

        public async Task<RequiredLicenceCategoryGetDTO> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails)
        {
            if (requirementDetails.RequiredYears <= 0)
                throw new ValueMustBeGreaterThanZeroException("required years");
            var licenceCategory = await _licenceCategoryService.CheckLicenceCategory(requirementDetails.LicenceCategoryId);
            var requiredLicenceCategory = await _licenceCategoryService.CheckLicenceCategory(requirementDetails.RequiredLicenceCategoryId);
            var requirementExists = await _requiredLicenceCategoryRepository.CheckRequirement(requirementDetails.LicenceCategoryId, requirementDetails.RequiredLicenceCategoryId);
            if (requirementExists != null)
                throw new RequirementAlreadyExistsException(requirementDetails.RequiredLicenceCategoryId, requirementDetails.LicenceCategoryId);
            var addedRequiredLicenceCategory = await _requiredLicenceCategoryRepository.PostRequirement(requirementDetails);
            return await _requiredLicenceCategoryRepository.GetRequirement(addedRequiredLicenceCategory.LicenceCategoryId, addedRequiredLicenceCategory.RequiredLicenceCategoryId);
        }

        public async Task<bool> MeetRequirements(ICollection<DrivingLicence> drivingLicences, int licenceCategoryId, DateTime receiveDate)
        {
            await _licenceCategoryService.CheckLicenceCategory(licenceCategoryId);
            var requiredDrivingLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirements(licenceCategoryId);
            if (!requiredDrivingLicenceCategories.Any())
                return true;
            if (requiredDrivingLicenceCategories.Any() && !drivingLicences.Any())
                return false;
            foreach(var requiredDrivingLicenceCategory in requiredDrivingLicenceCategories)
            {
                DrivingLicence drivingLicence = drivingLicences.Where(d => d.LicenceCategoryId == requiredDrivingLicenceCategory.RequiredLicenceCategoryId).FirstOrDefault();
                if (drivingLicence == null)
                    return false;
                DateTime StartDate = drivingLicence.ReceivedDate;
                DateTime RequiredTime = StartDate.AddYears(requiredDrivingLicenceCategory.RequiredYears);
                if (receiveDate < RequiredTime)
                    return false;
            }
            return true;
        }

        public async Task<RequiredLicenceCategory> CheckRequirement(int licenceCategoryId, int requiredLicenceCategoryId)
        {
            await _licenceCategoryService.CheckLicenceCategory(licenceCategoryId);
            await _licenceCategoryService.CheckLicenceCategory(requiredLicenceCategoryId);
            var requiredLicenceCategory = await _requiredLicenceCategoryRepository.CheckRequirement(licenceCategoryId, requiredLicenceCategoryId);
            if (requiredLicenceCategory == null)
                throw new NotFoundRequiredLicenceCategoryException(licenceCategoryId, requiredLicenceCategoryId);
            return requiredLicenceCategory;
        }

        public async Task<RequiredLicenceCategory> DeleteRequirement(int licenceCategoryId, int requiredLicenceCategoryId)
        {
            var requirementToDelete = await CheckRequirement(licenceCategoryId, requiredLicenceCategoryId);
            return await _requiredLicenceCategoryRepository.DeleteRequirement(requirementToDelete);
        }
    }
}
