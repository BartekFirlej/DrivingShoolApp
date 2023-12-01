using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Services;

namespace DrivingSchoolApp.Repositories
{
    public interface IRegistrationService
    {
        public Task<ICollection<RegistrationGetDTO>> GetRegistrations();
        public Task<PagedList<RegistrationGetDTO>> GetCourseRegistrations(int courseId, int page, int size);
        public Task<PagedList<RegistrationGetDTO>> GetCustomerRegistrations(int customerId, int page, int size);
        public Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId);
        public Task<Registration> CheckRegistration(int customerId, int courseId);
        public Task<Registration> DeleteRegistration(int customerId, int courseId);
        public Task<RegistrationResponseDTO> PostRegistration(RegistrationRequestDTO registrationDetails);
    }
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICustomerService _customerService;
        private readonly ICourseService _courseService;
        private readonly IDateTimeHelper _dateTimeHelperService;
        private readonly IMapper _mapper;

        public RegistrationService(IRegistrationRepository registrationRepository, ICustomerService customerService, ICourseService courseService, IDateTimeHelper dateTimeHelperService, IMapper mapper)
        {
            _registrationRepository = registrationRepository;
            _customerService = customerService;
            _courseService = courseService;
            _dateTimeHelperService = dateTimeHelperService;
            _mapper = mapper;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetRegistrations()
        {
            var registrations = await _registrationRepository.GetRegistrations();
            if (!registrations.Any())
                throw new NotFoundRegistrationException();
            return registrations;
        }

        public async Task<PagedList<RegistrationGetDTO>> GetCourseRegistrations(int courseId, int page, int size)
        {
            var course = await _courseService.CheckCourse(courseId);
            var registration = await _registrationRepository.GetCourseRegistrations(courseId, page, size);
            if (!registration.PagedItems.Any())
                throw new NotFoundRegistrationException();
            return registration;
        }

        public async Task<PagedList<RegistrationGetDTO>> GetCustomerRegistrations(int customerId, int page, int size)
        {
            var customer = await _customerService.CheckCustomer(customerId);
            var registrations = await _registrationRepository.GetCustomerRegistrations(customerId, page, size);
            if (!registrations.PagedItems.Any())
                throw new NotFoundRegistrationException();
            return registrations;
        }

        public async Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId)
        {
            var customer = await _customerService.CheckCustomer(customerId);
            var course = await _courseService.CheckCourse(courseId);
            var registration = await _registrationRepository.GetRegistration(customerId, courseId);
            if (registration == null)
                throw new NotFoundRegistrationException(customerId, courseId);
            return registration;
        }
        public async Task<RegistrationResponseDTO> PostRegistration(RegistrationRequestDTO registrationDetails)
        {
            var customer = await _customerService.CheckCustomer(registrationDetails.CustomerId);
            var course = await _courseService.GetCourse(registrationDetails.CourseId);
            var assignedPeopleCount = await _courseService.GetCourseAssignedPeopleCount(registrationDetails.CourseId);
            if (assignedPeopleCount >= course.Limit)
                throw new AssignLimitReachedException(course.Id);
            var registration = await _registrationRepository.CheckRegistration(registrationDetails.CustomerId, registrationDetails.CourseId);
            if (registration != null)
                throw new CustomerAlreadyAssignedToCourseException(registrationDetails.CustomerId, registrationDetails.CourseId);
            var meetAgeRequirement = _customerService.CheckCustomerAgeRequirement(customer.BirthDate, course.CourseType.MinimumAge, _dateTimeHelperService.GetDateTimeNow());
            if (meetAgeRequirement == false)
                throw new CustomerDoesntMeetRequirementsException(customer.Id);
            var createdRegistration = await _registrationRepository.PostRegistration(registrationDetails, _dateTimeHelperService.GetDateTimeNow());
            return _mapper.Map<RegistrationResponseDTO>(createdRegistration);
        }

        public async Task<Registration> CheckRegistration(int customerId, int courseId)
        {
            var customer = await _customerService.CheckCustomer(customerId);
            var course = await _courseService.CheckCourse(courseId);
            var registration = await _registrationRepository.CheckRegistration(customerId, courseId);
            if (registration == null)
                throw new NotFoundRegistrationException(customerId, courseId);
            return registration;
        }

        public async Task<Registration> DeleteRegistration(int customerId, int courseId)
        {
            var registrationToDelete = await CheckRegistration(customerId, courseId);
            return await _registrationRepository.DeleteRegistration(registrationToDelete);
        }
    }
}
