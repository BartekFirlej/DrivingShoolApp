using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IRegistrationRepository
    {
        public Task<ICollection<RegistrationGetDTO>> GetRegistrations();
        public Task<PagedList<RegistrationGetDTO>> GetCourseRegistrations(int courseId, int page, int size);
        public Task<PagedList<RegistrationGetDTO>> GetCustomerRegistrations(int customerId, int page, int size);
        public Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId);
        public Task<Registration> CheckRegistration(int customerId, int courseId);
        public Task<Registration> DeleteRegistration(Registration registrationToDelete);
        public Task<Registration> PostRegistration(RegistrationRequestDTO registrationDetails, DateTime registrationDate);
    }
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;
        
        public RegistrationRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetRegistrations()
        {
            return await _dbContext.Registrations
                                   .AsNoTracking() 
                                   .Select(r => new RegistrationGetDTO
                                   {
                                       RegistrationDate = r.RegistrationDate,
                                       CustomerId = r.CustomerId,
                                       CourseId = r.CourseId
                                   }).ToListAsync();

        }

        public async Task<PagedList<RegistrationGetDTO>> GetCourseRegistrations(int courseId, int page, int size)
        {
            return await PagedList<RegistrationGetDTO>.Create(
                    _dbContext.Registrations
                        .AsNoTracking()
                       .Where(r => r.CourseId == courseId)
                       .Select(r => new RegistrationGetDTO
                       {
                           RegistrationDate = r.RegistrationDate,
                           CustomerId = r.CustomerId,
                           CourseId = r.CourseId
                       })
                       .OrderBy(r => r.CustomerId),
                    page, size);
        }

        public async Task<PagedList<RegistrationGetDTO>> GetCustomerRegistrations(int customerId, int page, int size)
        {
            return await PagedList<RegistrationGetDTO>.Create(
                     _dbContext.Registrations
                       .AsNoTracking()
                       .Where(r => r.CustomerId == customerId)
                       .Select(r => new RegistrationGetDTO
                       {
                           RegistrationDate = r.RegistrationDate,
                           CustomerId = r.CustomerId,
                           CourseId = r.CourseId
                       }).OrderBy(r => r.CourseId),
                    page, size);
        }

        public async Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId)
        {
            return await _dbContext.Registrations
                        .AsNoTracking()
                       .Where(r => r.CustomerId == customerId && r.CourseId == courseId)
                       .Select(r => new RegistrationGetDTO
                       {
                           RegistrationDate = r.RegistrationDate,
                           CustomerId = r.CustomerId,
                           CourseId = r.CourseId
                       }).FirstOrDefaultAsync();
        }

        public async Task<Registration> PostRegistration(RegistrationRequestDTO registrationDetails, DateTime registrationDate)
        {
            var registrationToAdd = new Registration
            {
                RegistrationDate = registrationDate,
                CustomerId = registrationDetails.CustomerId,
                CourseId = registrationDetails.CourseId
            };
            await _dbContext.Registrations.AddAsync(registrationToAdd);
            await _dbContext.SaveChangesAsync();
            return registrationToAdd;
        }

        public async Task<Registration> CheckRegistration(int customerId, int courseId)
        {
            return await _dbContext.Registrations
                       .AsNoTracking()
                       .Where(r => r.CustomerId == customerId && r.CourseId == courseId)
                       .FirstOrDefaultAsync();
        }

        public async Task<Registration> DeleteRegistration(Registration registrationToDelete)
        {
            var deletedRegistration = _dbContext.Registrations.Remove(registrationToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedRegistration.Entity;
        }
    }
}
