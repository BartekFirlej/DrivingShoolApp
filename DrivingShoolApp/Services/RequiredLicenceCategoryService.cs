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
        public Task<bool> MeetRequirements(int customerId, int licenceCategoryId, DateTime receiveDate);
    }
    public class RequiredLicenceCategoryService : IRequiredLicenceCategoryService
    {
        private readonly IRequiredLicenceCategoryRepository _requiredLicenceCategoryRepository;
        private readonly ILicenceCategoryService _licenceCategoryService;
        private readonly IDrivingLicenceService _drivingLicenceService;

        public RequiredLicenceCategoryService(IRequiredLicenceCategoryRepository requiredLicenceCategoryRepository, ILicenceCategoryService licenceCategoryService, IDrivingLicenceService drivingLicenceService)
        {
            _requiredLicenceCategoryRepository = requiredLicenceCategoryRepository;
            _licenceCategoryService = licenceCategoryService;
            _drivingLicenceService = drivingLicenceService;
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

        public async Task<bool> MeetRequirements(int customerId, int licenceCategoryId, DateTime receiveDate)
        {
            ICollection<RequiredLicenceCategoryGetDTO> requiredDrivingLicenceCategories = new List<RequiredLicenceCategoryGetDTO>();
            ICollection<DrivingLicenceGetDTO> drivingLicences = new List<DrivingLicenceGetDTO>();
            try
            {
                requiredDrivingLicenceCategories = await GetRequirements(licenceCategoryId);
                drivingLicences = await _drivingLicenceService.GetCustomerDrivingLicences(customerId);
            }
            catch(NotFoundRequiredLicenceCategoryException e)
            {
                return true;
            }
            catch(NotFoundDrivingLicenceException e)
            {
                if (requiredDrivingLicenceCategories.Any())
                    return false;
            }
            foreach(var requiredDrivingLicenceCategory in requiredDrivingLicenceCategories)
            {
                DrivingLicenceGetDTO drivingLicence = drivingLicences.Where(d => d.LicenceCategoryId == requiredDrivingLicenceCategory.LicenceCategoryId).FirstOrDefault();
                DateTime StartDate = drivingLicence.ReceivedDate;
                DateTime RequiredTime = StartDate.AddYears(requiredDrivingLicenceCategory.RequiredYears);
                if (receiveDate >= RequiredTime)
                    continue;
                else
                    return false;
            }
            return true;
        }
    }
}
