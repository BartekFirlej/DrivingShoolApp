using DrivingSchoolApp.Models;
using DrivingSchoolApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IDrivingLicenceRepository
    {
        public Task<ICollection<DrivingLicenceGetDTO>> GetDrivingLicences();
        public Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId, DateTime date);
        public Task<DrivingLicenceGetDTO> GetDrivingLicence(int id);
        public Task<DrivingLicence> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails);
    }
    public class DrivingLicenceRepository : IDrivingLicenceRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public DrivingLicenceRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetDrivingLicences()
        {
            return await _dbContext.DrivingLicences
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
                                   }).ToListAsync();
        }

        public async Task<DrivingLicenceGetDTO> GetDrivingLicence(int id)
        {
            return await _dbContext.DrivingLicences
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

        public async Task<DrivingLicence> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails)
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

        public async Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId, DateTime date)
        {
            return await _dbContext.DrivingLicences
                                   .Include(d => d.LicenceCategory)
                                   .Include(d => d.Customer)
                                   .Where(d => d.CustomerId == customerId && (d.ExpirationDate >= date || d.ExpirationDate == null))
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
    }
}
