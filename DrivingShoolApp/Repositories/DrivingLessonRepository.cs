using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IDrivingLessonRepository
    {
        public Task<ICollection<DrivingLessonGetDTO>> GetDrivingLessons();
        public Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId);
        public Task<DrivingLesson> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails);
    }
    public class DrivingLessonRepository : IDrivingLessonRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public DrivingLessonRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<DrivingLessonGetDTO>> GetDrivingLessons()
        {
            return await _dbContext.DrivingLessons
                           .Include(d => d.Customer)
                           .Include(d => d.Lecturer)
                           .Select(d => new DrivingLessonGetDTO
                           {
                               Id = d.Id,
                               LessonDate = d.LessonDate,
                               CustomerId = d.CustomerId,
                               CustomerName = d.Customer.Name,
                               LecturerId = d.LecturerId,
                               LecturerName = d.Lecturer.Name,
                               AddressId = d.AddressId
                           }).ToListAsync();
                          
        }

        public async Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId)
        {
            return await _dbContext.DrivingLessons
               .Include(d => d.Customer)
               .Include(d => d.Lecturer)
               .Where(d => d.Id == drivingLessonId)
               .Select(d => new DrivingLessonGetDTO
               {
                   Id = d.Id,
                   LessonDate = d.LessonDate,
                   CustomerId = d.CustomerId,
                   CustomerName = d.Customer.Name,
                   LecturerId = d.LecturerId,
                   LecturerName = d.Lecturer.Name,
                   AddressId = d.AddressId
               }).FirstOrDefaultAsync();
        }

        public async Task<DrivingLesson> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails)
        {
            var drivingLessonToAdd = new DrivingLesson
            {
                LessonDate = drivingLessonDetails.LessonDate,
                CustomerId = drivingLessonDetails.CustomerId,
                LecturerId = drivingLessonDetails.LecturerId,
                AddressId = drivingLessonDetails.AddressId
            };
            await _dbContext.DrivingLessons.AddAsync(drivingLessonToAdd);
            await _dbContext.SaveChangesAsync();
            return drivingLessonToAdd;
        }
    }
}
