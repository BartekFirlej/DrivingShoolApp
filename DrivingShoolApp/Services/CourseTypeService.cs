using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolApp.Services
{
    public interface ICourseTypeService {
        public Task<ICollection<CourseTypeGetDTO>> GetCourseTypes();
        public Task<CourseTypeGetDTO> GetCourseType(int courseTypeId);
        public Task<CourseTypeGetDTO> PostCourseType(CourseTypePostDTO newCourseType);
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

        public async Task<ICollection<CourseTypeGetDTO>> GetCourseTypes()
        {
            var courseTypes = await _courseTypeRepository.GetCourseTypes();
            if(!courseTypes.Any())
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
            await _licenceCategoryService.GetLicenceCategory(newCourseType.LicenceCategoryId);
            var addedCourseType = await _courseTypeRepository.PostCourseType(newCourseType);
            return await _courseTypeRepository.GetCourseType(addedCourseType.Id);
        }
    }
}
