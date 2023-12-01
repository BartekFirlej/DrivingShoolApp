using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ICustomerLectureService
    {
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomersLectures();
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomerLectures(int customerId);
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId);
        public Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId);
        public Task<CustomerLectureCheckDTO> CheckCustomerLecture(int customerId, int lectureId);
        public Task<CustomerLectureCheckDTO> DeleteCustomerLecture(int customerId, int lectureId);
        public Task<CustomerLectureResponseDTO> PostCustomerLecture(CustomerLectureRequestDTO customerLectureDetails);
    }
    public class CustomerLectureService : ICustomerLectureService
    {
        private readonly ICustomerLectureRepository _customerLectureRepository;
        private readonly ICustomerService _customerService;
        private readonly ILectureService _lectureService;
        private readonly IRegistrationService _registrationService;
        private readonly IMapper _mapper;

        public CustomerLectureService(ICustomerLectureRepository customerLectureRepository, ICustomerService customerService, ILectureService lectureService, IRegistrationService registrationService, IMapper mapper)
        {
            _customerLectureRepository = customerLectureRepository;
            _customerService = customerService;
            _lectureService = lectureService;
            _registrationService = registrationService;
            _mapper = mapper;
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
            var customer = await _customerService.CheckCustomer(customerId);
            var customerLectures = await _customerLectureRepository.GetCustomerLectures(customerId);
            if (!customerLectures.Any())
                throw new NotFoundCustomerLectureException(customerId);
            return customerLectures;
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId)
        {
            var lecture = await _lectureService.CheckLecture(lectureId);
            var customersLecture = await _customerLectureRepository.GetCustomersLecture(lectureId);
            if (!customersLecture.Any())
                throw new NotFoundCustomersLectureException(lectureId);
            return customersLecture;
        }

        public async Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId)
        {
            var lecture = await _lectureService.CheckLecture(lectureId);
            var customer = await _customerService.CheckCustomer(customerId);
            var customerLecture = await _customerLectureRepository.GetCustomerLecture(customerId, lectureId);
            if (customerLecture == null)
                throw new NotFoundCustomerLectureException(customerId, lectureId);
            return customerLecture;
        }

        public async Task<CustomerLectureResponseDTO> PostCustomerLecture(CustomerLectureRequestDTO customerLectureDetails)
        {
            var customer = await _customerService.CheckCustomer(customerLectureDetails.CustomerId);
            var lecture = await _lectureService.CheckLecture(customerLectureDetails.LectureId);
            var registration = await _registrationService.CheckRegistration(customer.Id, lecture.CourseSubjectsCourseId);
            var customerLecture = await _customerLectureRepository.CheckCustomerLecture(customerLectureDetails.CustomerId, customerLectureDetails.LectureId);
            if (customerLecture != null)
                throw new CustomerAlreadyAssignedToLectureException(customerLectureDetails.CustomerId, customerLectureDetails.LectureId);
            return await _customerLectureRepository.PostCustomerLecture(customerLectureDetails);
        }

        public async Task<CustomerLectureCheckDTO> CheckCustomerLecture(int customerId, int lectureId)
        {
            var lecture = await _lectureService.CheckLecture(lectureId);
            var customer = await _customerService.CheckCustomer(customerId);
            var customerLecture = await _customerLectureRepository.CheckCustomerLecture(customerId, lectureId);
            if (customerLecture == null)
                throw new NotFoundCustomerLectureException(customerId, lectureId);
            return customerLecture;
        }

        public async Task<CustomerLectureCheckDTO> DeleteCustomerLecture(int customerId, int lectureId)
        {
            await CheckCustomerLecture(customerId, lectureId);
            var customer = await _customerService.CheckCustomer(customerId);
            var lecture = await _lectureService.CheckLecture(lectureId);
            return await _customerLectureRepository.DeleteCustomerLecture(customer, lecture);
        }
    }
}
