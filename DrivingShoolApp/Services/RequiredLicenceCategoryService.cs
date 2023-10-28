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
        public Task<RequiredLicenceCategoryGetDTO> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails);
        public Task<bool> MeetRequirements(ICollection<DrivingLicenceGetDTO> drivingLicences, int licenceCategoryId, DateTime receiveDate);
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
            await _licenceCategoryService.GetLicenceCategory(licenceCategoryId);
            await _licenceCategoryService.GetLicenceCategory(requiredLicenceCategoryId);
            var requiredLicenceCategories = await _requiredLicenceCategoryRepository.GetRequirement(licenceCategoryId, requiredLicenceCategoryId);
            if (requiredLicenceCategories == null)
                throw new NotFoundRequiredLicenceCategoryException(licenceCategoryId, requiredLicenceCategoryId);
            return requiredLicenceCategories;
        }

        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId)
        {
            await _licenceCategoryService.GetLicenceCategory(licenceCategoryId);
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

        public async Task<bool> MeetRequirements(ICollection<DrivingLicenceGetDTO> drivingLicences, int licenceCategoryId, DateTime receiveDate)
        {
            ICollection<RequiredLicenceCategoryGetDTO> requiredDrivingLicenceCategories = new List<RequiredLicenceCategoryGetDTO>();
            try
            {
                requiredDrivingLicenceCategories = await GetRequirements(licenceCategoryId);
            }
            catch(NotFoundRequiredLicenceCategoryException e)
            {
                return true;
            }
            if (requiredDrivingLicenceCategories.Any() && !drivingLicences.Any())
                return false;
            foreach(var requiredDrivingLicenceCategory in requiredDrivingLicenceCategories)
            {
                DrivingLicenceGetDTO drivingLicence = drivingLicences.Where(d => d.LicenceCategoryId == requiredDrivingLicenceCategory.LicenceCategoryId).FirstOrDefault();
                if (drivingLicence == null)
                    return false;
                DateTime StartDate = drivingLicence.ReceivedDate;
                DateTime RequiredTime = StartDate.AddYears(requiredDrivingLicenceCategory.RequiredYears);
                if (receiveDate < RequiredTime)
                    return false;
            }
            return true;
        }
    }
}
