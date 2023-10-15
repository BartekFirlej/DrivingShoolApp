﻿using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Models;

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
                throw new NotFoundCourseTypesException();
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
            await _licenceCategoryService.GetLicenceCategory(newCourseType.LicenceCategoryId);
            var addedCourseType = await _courseTypeRepository.PostCourseType(newCourseType);
            return await _courseTypeRepository.GetCourseType(addedCourseType.Id);
        }
    }
}
