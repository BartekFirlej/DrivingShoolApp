using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IRequiredLicenceCategoryRepository
    {
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements();
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId);
        public Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<RequiredLicenceCategory> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails);
    }
    public class RequiredLicenceCategoryRepository : IRequiredLicenceCategoryRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public RequiredLicenceCategoryRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements()
        {
            return await _dbContext.RequiredDrivingLicences.Include(l => l.LicenceCategory)
                                                           .Include(l => l.RequiredLicenceCategories)
                                                           .Select(l => new RequiredLicenceCategoryGetDTO
                                                           {
                                                               LicenceCategoryId = l.LicenceCategoryId,
                                                               LicenceCategoryName = l.LicenceCategory.Name,
                                                               RequiredLicenceCategoryId = l.RequiredLicenceCategoryId,
                                                               RequiredLicenceCategoryName = l.RequiredLicenceCategories.Name,
                                                               RequiredYears = l.RequiredYears
                                                           }).ToListAsync();
        }

        public async Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId)
        {
            return await _dbContext.RequiredDrivingLicences.Include(l => l.LicenceCategory)
                                               .Include(l => l.RequiredLicenceCategories)
                                               .Where(l => l.LicenceCategoryId == licenceCategoryId && l.RequiredLicenceCategoryId==requiredLicenceCategoryId)
                                               .Select(l => new RequiredLicenceCategoryGetDTO
                                               {
                                                   LicenceCategoryId = l.LicenceCategoryId,
                                                   LicenceCategoryName = l.LicenceCategory.Name,
                                                   RequiredLicenceCategoryId = l.RequiredLicenceCategoryId,
                                                   RequiredLicenceCategoryName = l.RequiredLicenceCategories.Name,
                                                   RequiredYears = l.RequiredYears
                                               }).FirstOrDefaultAsync();
        }



        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId)
        {
            return await _dbContext.RequiredDrivingLicences.Include(l => l.LicenceCategory)
                                               .Include(l => l.RequiredLicenceCategories)
                                               .Where(l => l.LicenceCategoryId == licenceCategoryId)
                                               .Select(l => new RequiredLicenceCategoryGetDTO
                                               {
                                                   LicenceCategoryId = l.LicenceCategoryId,
                                                   LicenceCategoryName = l.LicenceCategory.Name,
                                                   RequiredLicenceCategoryId = l.RequiredLicenceCategoryId,
                                                   RequiredLicenceCategoryName = l.RequiredLicenceCategories.Name,
                                                   RequiredYears = l.RequiredYears
                                               }).ToListAsync();
        }

        public async Task<RequiredLicenceCategory> PostRequirement(RequiredLicenceCategoryPostDTO requirementDetails)
        {
            var requirementToAdd = new RequiredLicenceCategory
            {
                LicenceCategoryId = requirementDetails.LicenceCategoryId,
                RequiredLicenceCategoryId = requirementDetails.RequiredLicenceCategoryId,
                RequiredYears = requirementDetails.RequiredYears
            };
            await _dbContext.AddAsync(requirementToAdd);
            await _dbContext.SaveChangesAsync();
            return requirementToAdd;
        }
    }
}
