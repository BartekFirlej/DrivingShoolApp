using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLicenceService
    {
        public Task<ICollection<DrivingLicenceGetDTO>> GetDrivingLicences();
        public Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId, DateTime date);
        public Task<DrivingLicenceGetDTO> GetDrivingLicence(int id);
        public Task<DrivingLicenceGetDTO> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails);

    }
    public class DrivingLicenceService : IDrivingLicenceService
    {
        private readonly IDrivingLicenceRepository _drivingLicenceRepository;
        private readonly ICustomerService _customerService;
        private readonly ILicenceCategoryService _licenceCategoryService;
        private readonly IRequiredLicenceCategoryService _requiredLicenceCategoryService;


        public DrivingLicenceService(IDrivingLicenceRepository drivingLicenceRepository,
                                     ICustomerService customerService,
                                     ILicenceCategoryService licenceCategoryService,
                                     IRequiredLicenceCategoryService requiredLicenceCategoryService)
        {
            _drivingLicenceRepository = drivingLicenceRepository;
            _customerService = customerService;
            _licenceCategoryService = licenceCategoryService;
            _requiredLicenceCategoryService = requiredLicenceCategoryService;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetDrivingLicences()
        {
            var drivingLicences = await _drivingLicenceRepository.GetDrivingLicences();
            if(!drivingLicences.Any())
                throw new NotFoundDrivingLicenceException();
            return drivingLicences;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId, DateTime date)
        {
            var drivingLicences = await _drivingLicenceRepository.GetCustomerDrivingLicences(customerId, date);
            if (!drivingLicences.Any())
                throw new NotFoundDrivingLicenceException();
            return drivingLicences;
        }

        public async Task<DrivingLicenceGetDTO> GetDrivingLicence(int id)
        {
            var drivingLicence = await _drivingLicenceRepository.GetDrivingLicence(id);
            if(drivingLicence == null)
                throw new NotFoundDrivingLicenceException(id);
            return drivingLicence;
        }

        public async Task<DrivingLicenceGetDTO> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails)
        {
            var customer = await _customerService.GetCustomer(drivingLicenceDetails.UserId);
            var licenceCategory = await _licenceCategoryService.GetLicenceCategory(drivingLicenceDetails.LicenceCategoryId);
            ICollection<DrivingLicenceGetDTO> customerDrivingLicences;
            try
            {
                customerDrivingLicences = await GetCustomerDrivingLicences(drivingLicenceDetails.UserId, drivingLicenceDetails.ReceivedDate);
            }
            catch(NotFoundDrivingLicenceException)
            {
                customerDrivingLicences = new List<DrivingLicenceGetDTO>();
            }
            var meetsRequirements = await _requiredLicenceCategoryService.MeetRequirements(customerDrivingLicences, drivingLicenceDetails.LicenceCategoryId, drivingLicenceDetails.ReceivedDate);
            if (!meetsRequirements)
                throw new CustomerDoesntMeetRequirementsException(drivingLicenceDetails.UserId, drivingLicenceDetails.LicenceCategoryId);
            var addedDrivingLicence = await _drivingLicenceRepository.PostDrivingLicence(drivingLicenceDetails);
            return await _drivingLicenceRepository.GetDrivingLicence(addedDrivingLicence.Id);
        }
    }
}
