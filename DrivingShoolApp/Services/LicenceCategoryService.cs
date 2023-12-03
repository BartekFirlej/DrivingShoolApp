using AutoMapper;
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
        public Task<LicenceCategoryResponseDTO> PostLicenceCategory(LicenceCategoryRequestDTO newCategory);
        public Task<LicenceCategory> CheckLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategory> CheckLicenceCategoryTracking(int licenceCategoryId);
        public Task<LicenceCategory> DeleteLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategoryResponseDTO> UpdateLicenceCategory(int licenceCategoryId, LicenceCategoryRequestDTO licenceCategoryUpdate);
    }
    public class LicenceCategoryService : ILicenceCategoryService
    {
        private readonly ILicenceCategoryRepository _licenceCategoryRepository;
        private readonly IMapper _mapper;

        public LicenceCategoryService(ILicenceCategoryRepository licenceCategoryRepository, IMapper mapper)
        {
            _licenceCategoryRepository = licenceCategoryRepository;
            _mapper = mapper;
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

        public async Task<LicenceCategoryResponseDTO> PostLicenceCategory(LicenceCategoryRequestDTO newCategory)
        {
            var createdCategory = await _licenceCategoryRepository.PostLicenceCategory(newCategory);
            return _mapper.Map<LicenceCategoryResponseDTO>(createdCategory);
        }

        public async Task<LicenceCategory> CheckLicenceCategory(int licenceCategoryId)
        {
            var licenceCategory = await _licenceCategoryRepository.CheckLicenceCategory(licenceCategoryId);
            if (licenceCategory == null)
                throw new NotFoundLicenceCategoryException(licenceCategoryId);
            return licenceCategory;
        }

        public async Task<LicenceCategory> CheckLicenceCategoryTracking(int licenceCategoryId)
        {
            var licenceCategory = await _licenceCategoryRepository.CheckLicenceCategoryTracking(licenceCategoryId);
            if (licenceCategory == null)
                throw new NotFoundLicenceCategoryException(licenceCategoryId);
            return licenceCategory;
        }

        public async Task<LicenceCategory> DeleteLicenceCategory(int licenceCategoryId)
        {
            var licenceCategoryToDelete = await CheckLicenceCategory(licenceCategoryId);
            return await _licenceCategoryRepository.DeleteLicenceCategory(licenceCategoryToDelete);
        }

        public async Task<LicenceCategoryResponseDTO> UpdateLicenceCategory(int licenceCategoryId, LicenceCategoryRequestDTO licenceCategoryUpdate)
        {
            var licenceCategoryToUpdate = await CheckLicenceCategoryTracking(licenceCategoryId);
            var updatedLicenceCategory = await _licenceCategoryRepository.UpdateLicenceCategory(licenceCategoryToUpdate, licenceCategoryUpdate);
            return _mapper.Map<LicenceCategoryResponseDTO>(updatedLicenceCategory);
        }
    }
}
