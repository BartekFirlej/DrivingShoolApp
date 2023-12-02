using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp
{
    public class DrivingSchoolMapper : Profile
    {
        public DrivingSchoolMapper() {
            CreateMap<Address, AddressResponseDTO>();
            CreateMap<Classroom, ClassroomResponseDTO>();
            CreateMap<Course, CourseResponseDTO>();
            CreateMap<CourseSubject, CourseSubjectResponseDTO>();
            CreateMap<CourseType, CourseTypeResponseDTO>();
            CreateMap<Customer, CustomerResponseDTO>();
            CreateMap<DrivingLesson, DrivingLessonResponseDTO>();
            CreateMap<DrivingLicence, DrivingLicenceResponseDTO>();
            CreateMap<Lecture, LectureResponseDTO>();
            CreateMap<Lecturer, LecturerResponseDTO>();
            CreateMap<LicenceCategory, LicenceCategoryResponseDTO>();
            CreateMap<Registration, RegistrationResponseDTO>();
            CreateMap<RequiredLicenceCategory, RequiredLicenceCategoryResponseDTO>();
            CreateMap<Subject, SubjectResponseDTO>();
        }
    }
}
