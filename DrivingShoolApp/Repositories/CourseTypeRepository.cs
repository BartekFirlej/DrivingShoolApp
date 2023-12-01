using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICourseTypeRepository
    {
        public Task<PagedList<CourseTypeGetDTO>> GetCourseTypes(int page, int size);
        public Task<CourseTypeGetDTO> GetCourseType(int courseTypeId);
        public Task<CourseType> PostCourseType(CourseTypeRequestDTO courseTypeDetails);
        public Task<CourseType> CheckCourseType(int courseTypeId);
        public Task<CourseType> DeleteCourseType(CourseType courseTypeToDelete);
        public Task<CourseType> UpdateCourseType(int courseTypeId, CourseTypeRequestDTO courseTypeUpdate);
    }
    public class CourseTypeRepository : ICourseTypeRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;
        
        public CourseTypeRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<CourseTypeGetDTO>> GetCourseTypes(int page, int size)
        {
            return await PagedList<CourseTypeGetDTO>.Create(
                _dbContext.CourseTypes
                .AsNoTracking()
                .Include(c => c.LicenceCategory)
                .Select(c => new CourseTypeGetDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    MinimumAge = c.MinimumAge,
                    LecturesHours = c.LectureHours,
                    DrivingHours = c.DrivingHours,
                    LicenceCategoryId = c.LicenceCategoryId,
                    LicenceCategoryName = c.LicenceCategory.Name
                }).OrderBy(c => c.Id),
                page, size);
        }
        public async Task<CourseTypeGetDTO> GetCourseType(int courseTypeId)
        {
            return await _dbContext.CourseTypes
                             .AsNoTracking()
                             .Include(c => c.LicenceCategory)
                             .Where(c => c.Id == courseTypeId)
                             .Select(c => new CourseTypeGetDTO
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 MinimumAge = c.MinimumAge,
                                 LecturesHours = c.LectureHours,
                                 DrivingHours = c.DrivingHours,
                                 LicenceCategoryId = c.LicenceCategoryId,
                                 LicenceCategoryName = c.LicenceCategory.Name
                             }).FirstOrDefaultAsync();
        }
        public async Task<CourseType> PostCourseType(CourseTypeRequestDTO courseTypeDetails)
        {
            var CourseTypeToAdd = new CourseType
            {
                Name = courseTypeDetails.Name,
                MinimumAge = courseTypeDetails.MinimumAge,
                LectureHours = courseTypeDetails.LecturesHours,
                DrivingHours = courseTypeDetails.DrivingHours,
                LicenceCategoryId = courseTypeDetails.LicenceCategoryId
            };
            await _dbContext.CourseTypes.AddAsync(CourseTypeToAdd);
            await _dbContext.SaveChangesAsync();
            return CourseTypeToAdd;
        }

        public async Task<CourseType> CheckCourseType(int courseTypeId)
        {
            return await _dbContext.CourseTypes
                             .AsNoTracking()
                             .Where(c => c.Id == courseTypeId)
                             .AsNoTracking()
                             .FirstOrDefaultAsync();
        }

        public async Task<CourseType> DeleteCourseType(CourseType courseTypeToDelete)
        {
            var deletedCourseType = _dbContext.CourseTypes.Remove(courseTypeToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedCourseType.Entity;
        }

        public async Task<CourseType> UpdateCourseType(int courseTypeId, CourseTypeRequestDTO courseTypeUpdate)
        {
            var courseType = await _dbContext.CourseTypes
                             .Where(c => c.Id == courseTypeId)
                             .FirstOrDefaultAsync();
            courseType.DrivingHours = courseTypeUpdate.DrivingHours;
            courseType.LectureHours = courseTypeUpdate.LecturesHours;
            courseType.LicenceCategoryId = courseTypeUpdate.LicenceCategoryId;
            courseType.MinimumAge = courseTypeUpdate.MinimumAge;
            courseType.Name = courseTypeUpdate.Name;
            await _dbContext.SaveChangesAsync();
            return courseType;
        }
    }
}
