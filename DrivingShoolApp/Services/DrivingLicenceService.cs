using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLicenceService
    {
        public Task<ICollection<DrivingLicenceGetDTO>> GetDrivingLicences();
        public Task<DrivingLicenceGetDTO> GetDrivingLicence(int id);
        public Task<DrivingLicenceGetDTO> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails);

    }
    public class DrivingLicenceService : IDrivingLicenceService
    {
        private readonly IDrivingLicenceRepository _drivingLicenceRepository;
        private readonly IUserService _userService;
        private readonly ILicenceCategoryService _licenceCategoryService;


        public DrivingLicenceService(IDrivingLicenceRepository drivingLicenceRepository,
                                     IUserService userService,
                                     ILicenceCategoryService licenceCategoryService)
        {
            _drivingLicenceRepository = drivingLicenceRepository;
            _userService = userService;
            _licenceCategoryService = licenceCategoryService;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetDrivingLicences()
        {
            var drivingLicences = await _drivingLicenceRepository.GetDrivingLicences();
            if(!drivingLicences.Any())
                throw new NotFoundDrivingLicencesException();
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
            await _userService.GetUser(drivingLicenceDetails.UserId);
            await _licenceCategoryService.GetLicenceCategory(drivingLicenceDetails.LicenceCategoryId);
            var addedDrivingLicence = await _drivingLicenceRepository.PostDrivingLicence(drivingLicenceDetails);
            return await _drivingLicenceRepository.GetDrivingLicence(addedDrivingLicence.Id);
        }   
    }
}
