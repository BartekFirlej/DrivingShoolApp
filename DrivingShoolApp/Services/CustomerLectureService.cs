using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ICustomerLectureService
    {
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomersLectures();
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomerLectures(int customerId);
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId);
        public Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId);
        public Task<CustomerLectureGetDTO> PostCustomerLecture(CustomerLecturePostDTO customerLectureDetails);
    }
    public class CustomerLectureService : ICustomerLectureService
    {
        private readonly ICustomerLectureRepository _customerLectureRepository;
        private readonly ICustomerService _customerService;
        private readonly ILectureService _lectureService;
        private readonly IRegistrationService _registrationService;

        public CustomerLectureService(ICustomerLectureRepository customerLectureRepository, ICustomerService customerService, ILectureService lectureService, IRegistrationService registrationService)
        {
            _customerLectureRepository = customerLectureRepository;
            _customerService = customerService;
            _lectureService = lectureService;
            _registrationService = registrationService;
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomersLectures()
        {
            var customerLectures = await _customerLectureRepository.GetCustomersLectures();
            if (!customerLectures.Any())
                throw new NotFoundCustomerLectureException();
            return customerLectures;
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomerLectures(int customerId)
        {
            var customer = await _customerService.GetCustomer(customerId);
            var customerLectures = await _customerLectureRepository.GetCustomerLectures(customerId);
            if (!customerLectures.Any())
                throw new NotFoundCustomerLectureException(customerId);
            return customerLectures;
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId)
        {
            var lecture = await _lectureService.GetLecture(lectureId);
            var customersLecture = await _customerLectureRepository.GetCustomersLecture(lectureId);
            if (!customersLecture.Any())
                throw new NotFoundCustomersLectureException(lectureId);
            return customersLecture;
        }

        public async Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId)
        {
            var lecture = await _lectureService.GetLecture(lectureId);
            var customer = await _customerService.GetCustomer(customerId);
            var customerLecture = await _customerLectureRepository.GetCustomerLecture(customerId, lectureId);
            if (customerLecture == null)
                throw new NotFoundCustomerLectureException(customerId, lectureId);
            return customerLecture;
        }

        public async Task<CustomerLectureGetDTO> PostCustomerLecture(CustomerLecturePostDTO customerLectureDetails)
        {
            var customer = await _customerService.GetCustomer(customerLectureDetails.CustomerId);
            var lecture = await _lectureService.GetLecture(customerLectureDetails.LectureId);
            var customerLecture = await _customerLectureRepository.GetCustomerLecture(customerLectureDetails.CustomerId, customerLectureDetails.LectureId);
            if (customerLecture != null)
                throw new CustomerAlreadyAssignedToLectureException(customerLectureDetails.CustomerId, customerLectureDetails.LectureId);
            var registration = await _registrationService.GetRegistration(customer.Id, lecture.CourseId);
            return await _customerLectureRepository.PostCustomerLecture(customerLectureDetails);
        }
    }
}
