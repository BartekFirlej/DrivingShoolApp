﻿using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IRequiredLicenceCategoryRepository
    {
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements();
        public Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId);
        public Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<RequiredLicenceCategory> CheckRequirement(int licenceCategoryId, int requiredLicenceCategoryId);
        public Task<RequiredLicenceCategory> DeleteRequirement(RequiredLicenceCategory requirementToDelete);
        public Task<RequiredLicenceCategory> PostRequirement(RequiredLicenceCategoryRequestDTO requirementDetails); 
        public Task<RequiredLicenceCategory> UpdateRequirement(int licenceCategoryId, int requiredLicenceCategoryId, RequiredLicenceCategoryRequestDTO requirementUpdate);
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
            return await (from rlc in _dbContext.RequiredLicenceCategories.AsNoTracking()
                    join lc in _dbContext.LicenceCategories on rlc.LicenceCategoryId equals lc.Id
                    join rlcRequired in _dbContext.LicenceCategories on rlc.RequiredLicenceCategoryId equals rlcRequired.Id
                    select new RequiredLicenceCategoryGetDTO
                    {
                        LicenceCategoryId = lc.Id,
                        LicenceCategoryName = lc.Name,
                        RequiredLicenceCategoryId = rlcRequired.Id,
                        RequiredLicenceCategoryName = rlcRequired.Name,
                        RequiredYears = rlc.RequiredYears
                    }).ToListAsync();
        }

        public async Task<RequiredLicenceCategoryGetDTO> GetRequirement(int licenceCategoryId, int requiredLicenceCategoryId)
        {
            return await (from rlc in _dbContext.RequiredLicenceCategories.AsNoTracking()
                          join lc in _dbContext.LicenceCategories on rlc.LicenceCategoryId equals lc.Id
                          join rlcRequired in _dbContext.LicenceCategories on rlc.RequiredLicenceCategoryId equals rlcRequired.Id
                          where rlc.LicenceCategoryId == licenceCategoryId && rlc.RequiredLicenceCategoryId == requiredLicenceCategoryId
                          select new RequiredLicenceCategoryGetDTO
                          {
                              LicenceCategoryId = lc.Id,
                              LicenceCategoryName = lc.Name,
                              RequiredLicenceCategoryId = rlcRequired.Id,
                              RequiredLicenceCategoryName = rlcRequired.Name,
                              RequiredYears = rlc.RequiredYears
                          }).FirstOrDefaultAsync();
        }



        public async Task<ICollection<RequiredLicenceCategoryGetDTO>> GetRequirements(int licenceCategoryId)
        {
            return await (from rlc in _dbContext.RequiredLicenceCategories.AsNoTracking()
                   join lc in _dbContext.LicenceCategories on rlc.LicenceCategoryId equals lc.Id
                   join rlcRequired in _dbContext.LicenceCategories on rlc.RequiredLicenceCategoryId equals rlcRequired.Id
                   where rlc.LicenceCategoryId == licenceCategoryId
                   select new RequiredLicenceCategoryGetDTO
                   {
                       LicenceCategoryId = lc.Id,
                       LicenceCategoryName = lc.Name,
                       RequiredLicenceCategoryId = rlcRequired.Id,
                       RequiredLicenceCategoryName = rlcRequired.Name,
                       RequiredYears = rlc.RequiredYears
                   }).ToListAsync();
        }

        public async Task<RequiredLicenceCategory> PostRequirement(RequiredLicenceCategoryRequestDTO requirementDetails)
        {
            var requirementToAdd = new RequiredLicenceCategory
            {
                LicenceCategoryId = requirementDetails.LicenceCategoryId,
                RequiredLicenceCategoryId = requirementDetails.RequiredLicenceCategoryId,
                RequiredYears = requirementDetails.RequiredYears
            };
            await _dbContext.RequiredLicenceCategories.AddAsync(requirementToAdd);
            await _dbContext.SaveChangesAsync();
            return requirementToAdd;
        }

        public async Task<RequiredLicenceCategory> CheckRequirement(int licenceCategoryId, int requiredLicenceCategoryId)
        {
            return await (from rlc in _dbContext.RequiredLicenceCategories.AsNoTracking()
                          join lc in _dbContext.LicenceCategories on rlc.LicenceCategoryId equals lc.Id
                          join rlcRequired in _dbContext.LicenceCategories on rlc.RequiredLicenceCategoryId equals rlcRequired.Id
                          where rlc.LicenceCategoryId == licenceCategoryId && rlc.RequiredLicenceCategoryId == requiredLicenceCategoryId
                          select rlc)
                          .AsNoTracking()
                          .FirstOrDefaultAsync();
        }

        public async Task<RequiredLicenceCategory> DeleteRequirement(RequiredLicenceCategory requirementToDelete)
        {
            var deletedRequirement = _dbContext.RequiredLicenceCategories.Remove(requirementToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedRequirement.Entity;
        }

        public async Task<RequiredLicenceCategory> UpdateRequirement(int licenceCategoryId, int requiredLicenceCategoryId, RequiredLicenceCategoryRequestDTO requirementUpdate)
        {
            var requirement = await(from rlc in _dbContext.RequiredLicenceCategories.AsNoTracking()
                                    join lc in _dbContext.LicenceCategories on rlc.LicenceCategoryId equals lc.Id
                                    join rlcRequired in _dbContext.LicenceCategories on rlc.RequiredLicenceCategoryId equals rlcRequired.Id
                                    where rlc.LicenceCategoryId == licenceCategoryId && rlc.RequiredLicenceCategoryId == requiredLicenceCategoryId
                                    select rlc)
                          .FirstOrDefaultAsync();
            requirement.RequiredYears = requirementUpdate.RequiredYears;
            requirement.LicenceCategoryId = requirementUpdate.LicenceCategoryId;
            requirement.RequiredLicenceCategoryId = requirementUpdate.RequiredLicenceCategoryId;
            await _dbContext.SaveChangesAsync();
            return requirement;
        }
    }
}
