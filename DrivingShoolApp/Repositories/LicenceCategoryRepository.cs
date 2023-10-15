using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ILicenceCategoryRepository
    {
        public Task<ICollection<LicenceCategoryGetDTO>> GetLicenceCategories();
        public Task<LicenceCategoryGetDTO> GetLicenceCategory(int id);
        public Task<LicenceCategory> AddLicenceCategory(LicenceCategoryPostDTO newCategory);
    }
    public class LicenceCategoryRepository : ILicenceCategoryRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public LicenceCategoryRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<LicenceCategoryGetDTO>> GetLicenceCategories()
        {
            return await _dbContext.LicenceCategories.
                                Select(l => new LicenceCategoryGetDTO { Id = l.Id, Name = l.Name })
                                .ToListAsync();
        }

        public async Task<LicenceCategoryGetDTO> GetLicenceCategory(int id)
        {
            return await _dbContext.LicenceCategories
                              .Where(l => l.Id == id)
                              .Select(l => new LicenceCategoryGetDTO { Id = l.Id, Name = l.Name })
                              .FirstOrDefaultAsync();

        }

        public async Task<LicenceCategory> AddLicenceCategory(LicenceCategoryPostDTO licenceCategoryDetails)
        {
            var licenceCategoryToAdd = new LicenceCategory { Name = licenceCategoryDetails.Name };
            await _dbContext.LicenceCategories.AddAsync(licenceCategoryToAdd);
            await _dbContext.SaveChangesAsync();
            return licenceCategoryToAdd;
        }
    }
}
