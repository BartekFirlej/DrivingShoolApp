using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICourseRepository {
        public Task<PagedList<CourseGetDTO>> GetCourses(int page, int size);
        public Task<CourseGetDTO> GetCourse(int courseId);
        public Task<Course> PostCourse(CoursePostDTO courseDetails);
        public Task<int> GetCourseAssignedPeopleCount(int courseId); 
        public Task<Course> CheckCourse(int courseId);
        public Task<Course> DeleteCourse(Course courseToDelete);
    }
    public class CourseRepository : ICourseRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public CourseRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<CourseGetDTO>> GetCourses(int page, int size)
        {
            return await PagedList<CourseGetDTO>.Create(
                            _dbContext.Courses.Include(c => c.CourseType)
                               .Include(c => c.CourseType.LicenceCategory)
                               .Select(c => new CourseGetDTO
                               {
                                   Id = c.Id,
                                   BeginDate = c.BeginDate,
                                   Name = c.Name,
                                   Price = c.Price,
                                   Limit = c.Limit,
                                   CourseType = new CourseTypeGetDTO
                                   {
                                       Id = c.CourseTypeId,
                                       Name = c.CourseType.Name,
                                       LecturesHours = c.CourseType.LectureHours,
                                       DrivingHours = c.CourseType.DrivingHours,
                                       MinimumAge = c.CourseType.MinimumAge,
                                       LicenceCategoryId = c.CourseType.LicenceCategoryId,
                                       LicenceCategoryName = c.CourseType.LicenceCategory.Name
                                   }
                               }
                               ).OrderBy(c => c.Id),
                            page, size);
        }

        public async Task<CourseGetDTO> GetCourse(int courseId)
        {
            return await _dbContext.Courses.Include(c => c.CourseType)
                                           .Include(c => c.CourseType.LicenceCategory)
                                           .Where(c => c.Id == courseId)
                                           .Select(c => new CourseGetDTO
                                           {
                                               Id = c.Id,
                                               BeginDate = c.BeginDate,
                                               Name = c.Name,
                                               Price = c.Price,
                                               Limit = c.Limit,
                                               CourseType = new CourseTypeGetDTO
                                               {
                                                   Id = c.CourseTypeId,
                                                   Name = c.CourseType.Name,
                                                   LecturesHours = c.CourseType.LectureHours,
                                                   DrivingHours = c.CourseType.DrivingHours,
                                                   MinimumAge = c.CourseType.MinimumAge,
                                                   LicenceCategoryId = c.CourseType.LicenceCategoryId,
                                                   LicenceCategoryName = c.CourseType.LicenceCategory.Name
                                               }
                                           }).FirstOrDefaultAsync();
        }

        public async Task<int> GetCourseAssignedPeopleCount(int courseId)
        {
            return await _dbContext.Courses.Where(c => c.Id == courseId).Select(c => c.Registrations.Count()).FirstOrDefaultAsync();  
        }

        public async Task<Course> PostCourse(CoursePostDTO courseDetails)
        {
            var courseToAdd = new Course
            {
                BeginDate = courseDetails.BeginDate,
                Name = courseDetails.Name,
                Price = courseDetails.Price,
                Limit = courseDetails.Limit,
                CourseTypeId = courseDetails.CourseTypeId
            };
            await _dbContext.Courses.AddAsync(courseToAdd);
            await _dbContext.SaveChangesAsync();
            return courseToAdd;
        }

        public async Task<Course> CheckCourse(int courseId)
        {
            return await _dbContext.Courses
                         .Where(c => c.Id == courseId)
                         .AsNoTracking()
                         .FirstOrDefaultAsync();
        }

        public async Task<Course> DeleteCourse(Course courseToDelete)
        {
            var deletedCourse = _dbContext.Courses.Remove(courseToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedCourse.Entity;
        }
    }
}
