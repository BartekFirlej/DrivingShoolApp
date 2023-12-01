using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp
{
    public class DrivingSchoolMapper : Profile
    {
        public DrivingSchoolMapper() { 
            CreateMap<Classroom, ClassroomResponseDTO>();
            CreateMap<Address, AddressResponseDTO>();
            CreateMap<Course, CourseResponseDTO>();
            CreateMap<CourseType, CourseTypeResponseDTO>();
        }
    }
}
