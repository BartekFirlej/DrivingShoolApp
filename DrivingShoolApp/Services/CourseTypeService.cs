using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using AutoMapper;

namespace DrivingSchoolApp.Services
{
    public interface ICourseTypeService {
        public Task<PagedList<CourseTypeGetDTO>> GetCourseTypes(int page, int size);
        public Task<CourseTypeGetDTO> GetCourseType(int courseTypeId);
        public Task<CourseTypeResponseDTO> PostCourseType(CourseTypeRequestDTO newCourseType);
        public Task<CourseType> CheckCourseType(int courseTypeId);
        public Task<CourseType> DeleteCourseType(int courseTypeId);
        public Task<CourseTypeGetDTO> UpdateCourseType(int courseTypeId, CourseTypeRequestDTO courseTypeUpdate);
    }
    public class CourseTypeService : ICourseTypeService
    {
        private readonly ICourseTypeRepository _courseTypeRepository;
        private readonly ILicenceCategoryService _licenceCategoryService;
        private readonly IMapper _mapper;
        
        public CourseTypeService(ICourseTypeRepository courseTypeRepository, ILicenceCategoryService licenceCategoryService, IMapper mapper)
        {
            _courseTypeRepository = courseTypeRepository;
            _licenceCategoryService = licenceCategoryService;
            _mapper = mapper;
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

        public async Task<CourseTypeResponseDTO> PostCourseType(CourseTypeRequestDTO newCourseType)
        {
            if (newCourseType.LecturesHours <= 0)
                throw new ValueMustBeGreaterThanZeroException("Lecture hours");
            if(newCourseType.DrivingHours <= 0)
                throw new ValueMustBeGreaterThanZeroException("Driving hours");
            if (newCourseType.MinimumAge <= 0)
                throw new ValueMustBeGreaterThanZeroException("Minimum age");
            await _licenceCategoryService.CheckLicenceCategory(newCourseType.LicenceCategoryId);
            var addedCourseType = await _courseTypeRepository.PostCourseType(newCourseType);
            return _mapper.Map<CourseTypeResponseDTO>(addedCourseType);
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

        public async Task<CourseTypeGetDTO> UpdateCourseType(int courseTypeId, CourseTypeRequestDTO courseTypeUpdate)
        {
            await CheckCourseType(courseTypeId);
            if (courseTypeUpdate.LecturesHours <= 0)
                throw new ValueMustBeGreaterThanZeroException("Lecture hours");
            if (courseTypeUpdate.DrivingHours <= 0)
                throw new ValueMustBeGreaterThanZeroException("Driving hours");
            if (courseTypeUpdate.MinimumAge <= 0)
                throw new ValueMustBeGreaterThanZeroException("Minimum age");
            await _licenceCategoryService.CheckLicenceCategory(courseTypeUpdate.LicenceCategoryId);
            await _courseTypeRepository.UpdateCourseType(courseTypeId, courseTypeUpdate);
            return await _courseTypeRepository.GetCourseType(courseTypeId);
        }
    }
}
