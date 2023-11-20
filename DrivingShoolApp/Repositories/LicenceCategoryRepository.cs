using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ILicenceCategoryRepository
    {
        public Task<PagedList<LicenceCategoryGetDTO>> GetLicenceCategories(int page, int size);
        public Task<LicenceCategoryGetDTO> GetLicenceCategory(int id);
        public Task<LicenceCategory> PostLicenceCategory(LicenceCategoryPostDTO newCategory);
        public Task<LicenceCategory> CheckLicenceCategory(int licenceCategoryId);
        public Task<LicenceCategory> DeleteLicenceCategory(LicenceCategory licenceCategoryToDelete);
    }
    public class LicenceCategoryRepository : ILicenceCategoryRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public LicenceCategoryRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<LicenceCategoryGetDTO>> GetLicenceCategories(int page, int size)
        {
            return await PagedList<LicenceCategoryGetDTO>.Create(
                    _dbContext.LicenceCategories.
                         Select(l => new LicenceCategoryGetDTO { Id = l.Id, Name = l.Name })
                         .OrderBy(l => l.Id),
                    page, size);
        }

        public async Task<LicenceCategoryGetDTO> GetLicenceCategory(int id)
        {
            return await _dbContext.LicenceCategories
                              .Where(l => l.Id == id)
                              .Select(l => new LicenceCategoryGetDTO { Id = l.Id, Name = l.Name })
                              .FirstOrDefaultAsync();

        }

        public async Task<LicenceCategory> PostLicenceCategory(LicenceCategoryPostDTO licenceCategoryDetails)
        {
            var licenceCategoryToAdd = new LicenceCategory { Name = licenceCategoryDetails.Name };
            await _dbContext.LicenceCategories.AddAsync(licenceCategoryToAdd);
            await _dbContext.SaveChangesAsync();
            return licenceCategoryToAdd;
        }

        public async Task<LicenceCategory> CheckLicenceCategory(int licenceCategoryId)
        {
            return await _dbContext.LicenceCategories
                  .Where(l => l.Id == licenceCategoryId)
                  .AsNoTracking()
                  .FirstOrDefaultAsync();
        }

        public async Task<LicenceCategory> DeleteLicenceCategory(LicenceCategory licenceCategoryToDelete)
        {
            var deletedLicenceCategory = _dbContext.LicenceCategories.Remove(licenceCategoryToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedLicenceCategory.Entity;
        }
    }
}
