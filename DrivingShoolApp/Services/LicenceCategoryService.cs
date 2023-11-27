using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ILicenceCategoryService
    {
        public Task<PagedList<LicenceCategoryGetDTO>> GetLicenceCategories(int page, int size);
        public Task<LicenceCategoryGetDTO> GetLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategoryGetDTO> PostLicenceCategory(LicenceCategoryPostDTO newCategory);
        public Task<LicenceCategory> CheckLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategory> DeleteLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategoryGetDTO> UpdateLicenceCategory(int licenceCategoryId, LicenceCategoryPostDTO licenceCategoryUpdate);
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

        public async Task<LicenceCategory> CheckLicenceCategory(int licenceCategoryId)
        {
            var licenceCategory = await _licenceCategoryRepository.CheckLicenceCategory(licenceCategoryId);
            if (licenceCategory == null)
                throw new NotFoundLicenceCategoryException(licenceCategoryId);
            return licenceCategory;
        }

        public async Task<LicenceCategory> DeleteLicenceCategory(int licenceCategoryId)
        {
            var licenceCategoryToDelete = await CheckLicenceCategory(licenceCategoryId);
            return await _licenceCategoryRepository.DeleteLicenceCategory(licenceCategoryToDelete);
        }

        public async Task<LicenceCategoryGetDTO> UpdateLicenceCategory(int licenceCategoryId, LicenceCategoryPostDTO licenceCategoryUpdate)
        {
            await CheckLicenceCategory(licenceCategoryId);
            await _licenceCategoryRepository.UpdateLicenceCategory(licenceCategoryId, licenceCategoryUpdate);
            return await _licenceCategoryRepository.GetLicenceCategory(licenceCategoryId);
        }
    }
}
