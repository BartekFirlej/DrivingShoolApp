using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp.Services
{
    public interface ICourseTypeService {
        public Task<PagedList<CourseTypeGetDTO>> GetCourseTypes(int page, int size);
        public Task<CourseTypeGetDTO> GetCourseType(int courseTypeId);
        public Task<CourseTypeGetDTO> PostCourseType(CourseTypePostDTO newCourseType);
        public Task<CourseType> CheckCourseType(int courseTypeId);
        public Task<CourseType> DeleteCourseType(int courseTypeId);
    }
    public class CourseTypeService : ICourseTypeService
    {
        private readonly ICourseTypeRepository _courseTypeRepository;
        private readonly ILicenceCategoryService _licenceCategoryService;
        
        public CourseTypeService(ICourseTypeRepository courseTypeRepository, ILicenceCategoryService licenceCategoryService)
        {
            _courseTypeRepository = courseTypeRepository;
            _licenceCategoryService = licenceCategoryService;
        }

        public async Task<PagedList<CourseTypeGetDTO>> GetCourseTypes(int page, int size)
        {
            var courseTypes = await _courseTypeRepository.GetCourseTypes(page, size);
            if(!courseTypes.PagedItems.Any())
                throw new NotFoundCourseTypeException();
            return courseTypes;
        }

        public async Task<CourseTypeGetDTO> GetCourseType(int courseTypeId)
        {
            var course = await _courseTypeRepository.GetCourseType(courseTypeId);
            if(course == null)
                throw new NotFoundCourseTypeException(courseTypeId);
            return course;
        }

        public async Task<CourseTypeGetDTO> PostCourseType(CourseTypePostDTO newCourseType)
        {
            if (newCourseType.LecturesHours <= 0)
                throw new ValueMustBeGreaterThanZeroException("Lecture hours");
            if(newCourseType.DrivingHours <= 0)
                throw new ValueMustBeGreaterThanZeroException("Driving hours");
            if (newCourseType.MinimumAge <= 0)
                throw new ValueMustBeGreaterThanZeroException("Minimum age");
            await _licenceCategoryService.CheckLicenceCategory(newCourseType.LicenceCategoryId);
            var addedCourseType = await _courseTypeRepository.PostCourseType(newCourseType);
            return await _courseTypeRepository.GetCourseType(addedCourseType.Id);
        }

        public async Task<CourseType> CheckCourseType(int courseTypeId)
        {
            var courseType = await _courseTypeRepository.CheckCourseType(courseTypeId);
            if (courseType == null)
                throw new NotFoundCourseTypeException(courseTypeId);
            return courseType;
        }

        public async Task<CourseType> DeleteCourseType(int courseTypeId)
        {
            var courseTypeToDelete = await CheckCourseType(courseTypeId);
            return await _courseTypeRepository.DeleteCourseType(courseTypeToDelete);
        }
    }
}
