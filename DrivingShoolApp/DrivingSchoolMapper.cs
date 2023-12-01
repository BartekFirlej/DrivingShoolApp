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
            CreateMap<Customer, CustomerResponseDTO>();
            CreateMap<DrivingLicence, DrivingLicenceResponseDTO>();
            CreateMap<Lecture, LectureResponseDTO>();
            CreateMap<Lecturer, LecturerResponseDTO>();
            CreateMap<Subject, SubjectResponseDTO>();
        }
    }
}
