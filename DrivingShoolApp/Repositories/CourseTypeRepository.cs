using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICourseTypeRepository
    {
        public Task<PagedList<CourseTypeGetDTO>> GetCourseTypes(int page, int size);
        public Task<CourseTypeGetDTO> GetCourseType(int courseTypeId);
        public Task<CourseType> PostCourseType(CourseTypePostDTO courseTypeDetails);
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
                }),
                page, size);
        }
        public async Task<CourseTypeGetDTO> GetCourseType(int courseTypeId)
        {
            return await _dbContext.CourseTypes
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
        public async Task<CourseType> PostCourseType(CourseTypePostDTO courseTypeDetails)
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
    }
}
