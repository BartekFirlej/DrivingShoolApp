using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ILicenceCategoryService
    {
        public Task<PagedList<LicenceCategoryGetDTO>> GetLicenceCategories(int page, int size);
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

        public async Task<PagedList<LicenceCategoryGetDTO>> GetLicenceCategories(int page, int size)
        {
            var licenceCategories = await _licenceCategoryRepository.GetLicenceCategories(page, size);
            if(!licenceCategories.PagedItems.Any())
                throw new NotFoundLicenceCategoryException();
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
            var createdCategory = await _licenceCategoryRepository.PostLicenceCategory(newCategory);
            return await _licenceCategoryRepository.GetLicenceCategory(createdCategory.Id);
        }
    }
}
