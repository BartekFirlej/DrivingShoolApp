using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLessonService
    {
        public Task<PagedList<DrivingLessonGetDTO>> GetDrivingLessons(int page, int size);
        public Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId);
        public Task<DrivingLessonResponseDTO> PostDrivingLesson(DrivingLessonRequestDTO drivingLessonDetails);
        public Task<DrivingLesson> DeleteDrivingLesson(int drivingLessonId);
        public Task<DrivingLesson> CheckDrivingLesson(int drivingLessonId);
        public Task<DrivingLessonGetDTO> UpdateDrivingLesson(int drivingLessonId, DrivingLessonRequestDTO drivingLessonUpdate);
    }
    public class DrivingLessonService : IDrivingLessonService
    {
        private readonly IDrivingLessonRepository _drivingLessonRepository;
        private readonly ICustomerService _customerService;
        private readonly ILecturerService _lecturerService;
        private readonly IAddressService _addressService;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public DrivingLessonService(IDrivingLessonRepository drivingLessonRepository, ICustomerService customerService, 
                                    ILecturerService lecturerService, IAddressService addressService, ICourseService courseService, IMapper mapper)
        {
            _drivingLessonRepository = drivingLessonRepository;
            _customerService = customerService;
            _lecturerService = lecturerService;
            _addressService = addressService;
            _courseService = courseService;
            _mapper = mapper;
        }

        public async Task<PagedList<DrivingLessonGetDTO>> GetDrivingLessons(int page, int size)
        {
            var drivingLessons = await _drivingLessonRepository.GetDrivingLessons(page, size);
            if (!drivingLessons.PagedItems.Any())
                throw new NotFoundDrivingLessonException();
            return drivingLessons;
        }

        public async Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId)
        {
            var drivingLesson = await _drivingLessonRepository.GetDrivingLesson(drivingLessonId);
            if (drivingLesson == null)
                throw new NotFoundDrivingLessonException(drivingLessonId);
            return drivingLesson;
        }

        public async Task<DrivingLessonResponseDTO> PostDrivingLesson(DrivingLessonRequestDTO drivingLessonDetails)
        {
            if (drivingLessonDetails.LessonDate == DateTime.MinValue)
                throw new DateTimeException("lesson date");
            await _customerService.CheckCustomer(drivingLessonDetails.CustomerId);
            await _lecturerService.CheckLecturer(drivingLessonDetails.LecturerId);
            await _addressService.CheckAddress(drivingLessonDetails.AddressId);
            await _courseService.CheckCourse(drivingLessonDetails.CourseId);
            var addedDrivingLesson = await _drivingLessonRepository.PostDrivingLesson(drivingLessonDetails);
            return _mapper.Map<DrivingLessonResponseDTO>(addedDrivingLesson);
        }

        public async Task<DrivingLesson> DeleteDrivingLesson(int drivingLessonId)
        {
            var drivingLessonToDelete = await CheckDrivingLesson(drivingLessonId);
            return await _drivingLessonRepository.DeleteDrivingLesson(drivingLessonToDelete);
        }

        public async Task<DrivingLesson> CheckDrivingLesson(int drivingLessonId)
        {
            var drivingLesson = await _drivingLessonRepository.CheckDrivingLesson(drivingLessonId);
            if (drivingLesson == null)
                throw new NotFoundDrivingLessonException(drivingLessonId);
            return drivingLesson;
        }

        public async Task<DrivingLessonGetDTO> UpdateDrivingLesson(int drivingLessonId, DrivingLessonRequestDTO drivingLessonUpdate)
        {
            await CheckDrivingLesson(drivingLessonId);
            if (drivingLessonUpdate.LessonDate == DateTime.MinValue)
                throw new DateTimeException("lesson date");
            await _customerService.CheckCustomer(drivingLessonUpdate.CustomerId);
            await _lecturerService.CheckLecturer(drivingLessonUpdate.LecturerId);
            await _addressService.CheckAddress(drivingLessonUpdate.AddressId);
            await _courseService.CheckCourse(drivingLessonUpdate.CourseId);
            await _drivingLessonRepository.UpdateDrivingLesson(drivingLessonId, drivingLessonUpdate);
            return await _drivingLessonRepository.GetDrivingLesson(drivingLessonId);
        }
    }
}
