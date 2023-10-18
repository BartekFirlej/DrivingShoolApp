using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ILicenceCategoryService
    {
        public Task<ICollection<LicenceCategoryGetDTO>> GetLicenceCategories();
        public Task<LicenceCategoryGetDTO> GetLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategoryGetDTO> PostLicenceCategory(LicenceCategoryPostDTO newCategory);
    }
    public class LicenceCategoryService : ILicenceCategoryService
    {
        private readonly ILicenceCategoryRepository _licenceCategoryRepository;

        public LicenceCategoryService(ILicenceCategoryRepository licenceCategoryRepository)
        {
            _licenceCategoryRepository = licenceCategoryRepository;
        }

        public async Task<ICollection<LicenceCategoryGetDTO>> GetLicenceCategories()
        {
            var licenceCategories = await _licenceCategoryRepository.GetLicenceCategories();
            if(!licenceCategories.Any())
                throw new NotFoundLicenceCategoriesException();
            return licenceCategories;
        }

        public async Task<LicenceCategoryGetDTO> GetLicenceCategory(int licenceCategoryId)
        {
            var licenceCategory = await _licenceCategoryRepository.GetLicenceCategory(licenceCategoryId);
            if(licenceCategory == null)
                throw new NotFoundLicenceCategoryException(licenceCategoryId);
            return licenceCategory;
        }

        public async Task<LicenceCategoryGetDTO> PostLicenceCategory(LicenceCategoryPostDTO newCategory)
        {
            var createdCategory = await _licenceCategoryRepository.AddLicenceCategory(newCategory);
            return await _licenceCategoryRepository.GetLicenceCategory(createdCategory.Id);
        }
    }
}
