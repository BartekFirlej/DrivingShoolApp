using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLessonService
    {
        public Task<ICollection<DrivingLessonGetDTO>> GetDrivingLessons();
        public Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId);
        public Task<DrivingLessonGetDTO> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails);
    }
    public class DrivingLessonService : IDrivingLessonService
    {
        private readonly IDrivingLessonService _drivingLessonService;
        private readonly ICustomerService _customerService;
        private readonly ILecturerService _lecturerService;
        private readonly IAddressService _addressService;

        public DrivingLessonService(IDrivingLessonService drivingLessonService, ICustomerService customerService, ILecturerService lecturerService, IAddressService addressService)
        {
            _drivingLessonService = drivingLessonService;
            _customerService = customerService;
            _lecturerService = lecturerService;
            _addressService = addressService;
        }

        public async Task<ICollection<DrivingLessonGetDTO>> GetDrivingLessons()
        {
            var drivingLessons = await _drivingLessonService.GetDrivingLessons();
            if (!drivingLessons.Any())
                throw new NotFoundDrivingLessonsException();
            return drivingLessons;
        }

        public async Task<DrivingLessonGetDTO> GetDrivingLesson(int drivingLessonId)
        {
            var drivingLesson = await _drivingLessonService.GetDrivingLesson(drivingLessonId);
            if (drivingLesson == null)
                throw new NotFoundDrivingLessonException(drivingLessonId);
            return drivingLesson;
        }

        public async Task<DrivingLessonGetDTO> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails)
        {
            var customer = await _customerService.GetCustomer(drivingLessonDetails.CustomerId);
            var lecturer = await _lecturerService.GetLecturer(drivingLessonDetails.LecturerId);
            var address = await _addressService.GetAddress(drivingLessonDetails.AddressId);
            var addedDrivingLesson = await _drivingLessonService.PostDrivingLesson(drivingLessonDetails);
            return await _drivingLessonService.GetDrivingLesson(addedDrivingLesson.Id);
        }
    }
}
