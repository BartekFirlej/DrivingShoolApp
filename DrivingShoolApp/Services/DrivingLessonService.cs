using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLessonService
    {
        public Task<PagedList<DrivingLessonGetDTO>> GetDrivingLessons(int page, int size);
        public Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId);
        public Task<DrivingLessonGetDTO> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails);
    }
    public class DrivingLessonService : IDrivingLessonService
    {
        private readonly IDrivingLessonRepository _drivingLessonRepository;
        private readonly ICustomerService _customerService;
        private readonly ILecturerService _lecturerService;
        private readonly IAddressService _addressService;
        private readonly ICourseService _courseService;

        public DrivingLessonService(IDrivingLessonRepository drivingLessonRepository, ICustomerService customerService, ILecturerService lecturerService, IAddressService addressService, ICourseService courseService)
        {
            _drivingLessonRepository = drivingLessonRepository;
            _customerService = customerService;
            _lecturerService = lecturerService;
            _addressService = addressService;
            _courseService = courseService;
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

        public async Task<DrivingLessonGetDTO> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails)
        {
            if (drivingLessonDetails.LessonDate == DateTime.MinValue)
                throw new DateTimeException("lesson date");
            var customer = await _customerService.GetCustomer(drivingLessonDetails.CustomerId);
            var lecturer = await _lecturerService.GetLecturer(drivingLessonDetails.LecturerId);
            var address = await _addressService.GetAddress(drivingLessonDetails.AddressId);
            var course = await _courseService.GetCourse(drivingLessonDetails.CourseId);
            var addedDrivingLesson = await _drivingLessonRepository.PostDrivingLesson(drivingLessonDetails);
            return await _drivingLessonRepository.GetDrivingLesson(addedDrivingLesson.Id);
        }
    }
}
