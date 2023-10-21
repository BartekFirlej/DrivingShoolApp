using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using DrivingShoolApp;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IRegistrationRepository
    {
        public Task<ICollection<RegistrationGetDTO>> GetRegistrations();
        public Task<ICollection<RegistrationGetDTO>> GetCourseRegistrations(int courseId);
        public Task<ICollection<RegistrationGetDTO>> GetUserRegistrations(int userId);
        public Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId);
        public Task<Registration> PostRegistration(RegistrationPostDTO registrationDetails);
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
                                   .Select(r => new RegistrationGetDTO
                                   {
                                       RegistrationDate = r.RegistrationDate,
                                       CustomerId = r.CustomerId,
                                       CourseId = r.CourseId
                                   }).ToListAsync();

        }

        public async Task<ICollection<RegistrationGetDTO>> GetCourseRegistrations(int courseId)
        {
            return await _dbContext.Registrations
                       .Where(r => r.CourseId == courseId)
                       .Select(r => new RegistrationGetDTO
                       {
                           RegistrationDate = r.RegistrationDate,
                           CustomerId = r.CustomerId,
                           CourseId = r.CourseId
                       }).ToListAsync();
        }

        public async Task<ICollection<RegistrationGetDTO>> GetUserRegistrations(int userId)
        {
            return await _dbContext.Registrations
                       .Where(r => r.CustomerId == userId)
                       .Select(r => new RegistrationGetDTO
                       {
                           RegistrationDate = r.RegistrationDate,
                           CustomerId = r.CustomerId,
                           CourseId = r.CourseId
                       }).ToListAsync();
        }

        public async Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId)
        {
            return await _dbContext.Registrations
                       .Where(r => r.CustomerId == customerId && r.CourseId == courseId)
                       .Select(r => new RegistrationGetDTO
                       {
                           RegistrationDate = r.RegistrationDate,
                           CustomerId = r.CustomerId,
                           CourseId = r.CourseId
                       }).FirstOrDefaultAsync();
        }

        public async Task<Registration> PostRegistration(RegistrationPostDTO registrationDetails)
        {
            var registrationToAdd = new Registration
            {
                RegistrationDate = DateTime.Now,
                CustomerId = registrationDetails.CustomerId,
                CourseId = registrationDetails.CourseId
            };
            await _dbContext.Registrations.AddAsync(registrationToAdd);
            await _dbContext.SaveChangesAsync();
            return registrationToAdd;
        }
    }
}
