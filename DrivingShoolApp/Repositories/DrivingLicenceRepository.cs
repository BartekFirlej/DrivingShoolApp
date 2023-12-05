using DrivingSchoolApp.Models;
using DrivingSchoolApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IDrivingLicenceRepository
    {
        public Task<PagedList<DrivingLicenceGetDTO>> GetDrivingLicences(int page, int size);
        public Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId);
        public Task<ICollection<DrivingLicence>> CheckCustomerDrivingLicences(int customerId, DateTime date);
        public Task<DrivingLicenceGetDTO> GetDrivingLicence(int id);
        public Task<DrivingLicence> PostDrivingLicence(DrivingLicenceRequestDTO drivingLicenceDetails);
        public Task<DrivingLicence> DeleteDrivingLicence(DrivingLicence drivingLicenceToDelete);
        public Task<DrivingLicence> CheckDrivingLicence(int drivingLicenceId);
        public Task<DrivingLicence> CheckDrivingLicenceTracking(int drivingLicenceId);
        public Task<DrivingLicence> UpdateDrivingLicence(DrivingLicence drivingLicenceToUpdate, DrivingLicenceRequestDTO drivingLicenceUpdate);
    }
    public class DrivingLicenceRepository : IDrivingLicenceRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public DrivingLicenceRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<DrivingLicenceGetDTO>> GetDrivingLicences(int page, int size)
        {
            return await PagedList<DrivingLicenceGetDTO>.Create(
                _dbContext.DrivingLicences
                    .AsNoTracking()
                    .Include(d => d.LicenceCategory)
                    .Include(d => d.Customer)
                    .Select(d => new DrivingLicenceGetDTO
                    {
                        Id = d.Id,
                        CustomerId = d.CustomerId,
                        CustomerName = d.Customer.Name,
                        CutomserSecondName = d.Customer.SecondName,
                        LicenceCategoryId = d.LicenceCategoryId,
                        LicenceCategoryName = d.LicenceCategory.Name,
                        ExpirationDate = (DateTime)d.ExpirationDate,
                        ReceivedDate = d.ReceivedDate
                    }).OrderBy(d => d.Id),
                page, size);
        }

        public async Task<DrivingLicenceGetDTO> GetDrivingLicence(int id)
        {
            return await _dbContext.DrivingLicences
                                   .AsNoTracking()
                                   .Include(d => d.LicenceCategory)
                                   .Include(d => d.Customer)
                                   .Where(d => d.Id == id)
                                   .Select(d => new DrivingLicenceGetDTO
                                   {
                                       Id = d.Id,
                                       CustomerId = d.CustomerId,
                                       CustomerName = d.Customer.Name,
                                       CutomserSecondName = d.Customer.SecondName,
                                       LicenceCategoryId = d.LicenceCategoryId,
                                       LicenceCategoryName = d.LicenceCategory.Name,
                                       ExpirationDate = (DateTime)d.ExpirationDate,
                                       ReceivedDate = d.ReceivedDate
                                   }).FirstOrDefaultAsync();
        }

        public async Task<DrivingLicence> PostDrivingLicence(DrivingLicenceRequestDTO drivingLicenceDetails)
        {

            DrivingLicence drivingLicenceToAdd;
            if(drivingLicenceDetails.ExpirationDate == DateTime.MinValue)
            {
                drivingLicenceToAdd = new DrivingLicence
                {
                    ReceivedDate = drivingLicenceDetails.ReceivedDate,
                    CustomerId = drivingLicenceDetails.CustomerId,
                    LicenceCategoryId = drivingLicenceDetails.LicenceCategoryId
                };
            }
            else
            {
                drivingLicenceToAdd = new DrivingLicence
                {
                    ExpirationDate = drivingLicenceDetails.ExpirationDate,
                    ReceivedDate = drivingLicenceDetails.ReceivedDate,
                    CustomerId = drivingLicenceDetails.CustomerId,
                    LicenceCategoryId = drivingLicenceDetails.LicenceCategoryId
                };
            }
            await _dbContext.DrivingLicences.AddAsync(drivingLicenceToAdd);
            await _dbContext.SaveChangesAsync();
            return drivingLicenceToAdd;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId)
        {
            return await _dbContext.DrivingLicences
                                   .AsNoTracking()
                                   .Include(d => d.LicenceCategory)
                                   .Include(d => d.Customer)
                                   .Where(d => d.CustomerId == customerId && (d.ExpirationDate >= DateTime.Now || d.ExpirationDate == null))
                                   .Select(d => new DrivingLicenceGetDTO
                                   {
                                       Id = d.Id,
                                       CustomerId = d.CustomerId,
                                       CustomerName = d.Customer.Name,
                                       CutomserSecondName = d.Customer.SecondName,
                                       LicenceCategoryId = d.LicenceCategoryId,
                                       LicenceCategoryName = d.LicenceCategory.Name,
                                       ReceivedDate = d.ReceivedDate,
                                       ExpirationDate = (DateTime)d.ExpirationDate
                                   }).ToListAsync();
        }

        public async Task<DrivingLicence> DeleteDrivingLicence(DrivingLicence drivingLicenceToDelete)
        {
            var deletedDrivingLicence = _dbContext.DrivingLicences.Remove(drivingLicenceToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedDrivingLicence.Entity;
        }

        public async Task<DrivingLicence> CheckDrivingLicence(int drivingLicenceId)
        {
            return await _dbContext.DrivingLicences
                                   .AsNoTracking()
                                   .Where(d => d.Id == drivingLicenceId)
                                   .FirstOrDefaultAsync();
        }

        public async Task<DrivingLicence> CheckDrivingLicenceTracking(int drivingLicenceId)
        {
            return await _dbContext.DrivingLicences
                                   .Where(d => d.Id == drivingLicenceId)
                                   .FirstOrDefaultAsync();
        }

        public async Task<ICollection<DrivingLicence>> CheckCustomerDrivingLicences(int customerId, DateTime date)
        {
            return await _dbContext.DrivingLicences
                                  .AsNoTracking()
                                  .Where(d => d.CustomerId == customerId && (d.ExpirationDate >= date || d.ExpirationDate == null))
                                  .ToListAsync();
        }

        public async Task<DrivingLicence> UpdateDrivingLicence(DrivingLicence drivingLicenceToUpdate, DrivingLicenceRequestDTO drivingLicenceUpdate)
        {
            drivingLicenceToUpdate.ExpirationDate = drivingLicenceUpdate.ExpirationDate;
            drivingLicenceToUpdate.ReceivedDate = drivingLicenceUpdate.ReceivedDate;
            drivingLicenceToUpdate.LicenceCategoryId = drivingLicenceUpdate.LicenceCategoryId;
            drivingLicenceToUpdate.CustomerId = drivingLicenceUpdate.CustomerId;
            await _dbContext.SaveChangesAsync();
            return drivingLicenceToUpdate;
        }
    }
}
