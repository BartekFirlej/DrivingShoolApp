using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private Mock<IRegistrationRepository> _registrationRepositoryMock;
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<ICourseService> _courseServiceMock;
        private Mock<IDateTimeHelper> _dateTimeHelperServiceMock;
        private Fixture _fixture;
        private RegistrationService _service;

        public RegistrationServiceTests()
        {
            _fixture = new Fixture();
            _registrationRepositoryMock = new Mock<IRegistrationRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _courseServiceMock = new Mock<ICourseService>();
            _dateTimeHelperServiceMock = new Mock<IDateTimeHelper>();
        }

        [TestMethod]
        public async Task Get_Registrations_ReturnsRegistrations()
        {
            var registration = new RegistrationGetDTO();
            ICollection<RegistrationGetDTO> registrationsList = new List<RegistrationGetDTO>() { registration };
            _registrationRepositoryMock.Setup(repo => repo.GetRegistrations()).ReturnsAsync(registrationsList);
            _service = new RegistrationService( _registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.GetRegistrations();

            Assert.AreEqual(registrationsList, result);
        }

        [TestMethod]
        public async Task Get_Registrations_ThrowsNotFoundRegistrationException()
        {
            ICollection<RegistrationGetDTO> registrationsList = new List<RegistrationGetDTO>();
            _registrationRepositoryMock.Setup(repo => repo.GetRegistrations()).ReturnsAsync(registrationsList);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.GetRegistrations());
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ReturnsRegistrations()
        {
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO();
            var registrationsList = new PagedList<RegistrationGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<RegistrationGetDTO>{ registration }, HasNextPage = false };
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.GetCourseRegistrations(course.Id, 1 ,10)).ReturnsAsync(registrationsList);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.GetCourseRegistrations(course.Id, 1, 10);

            Assert.AreEqual(registrationsList, result);
            Assert.AreEqual(registrationsList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ThrowsNotFoundRegistrationException()
        {
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO();
            var registrationsList = new PagedList<RegistrationGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<RegistrationGetDTO>(), HasNextPage = false };
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.GetCourseRegistrations(course.Id, 1, 10)).ReturnsAsync(registrationsList);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.GetCourseRegistrations(course.Id, 1, 10));
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).Throws(new NotFoundCourseException(idOfCourseToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseRegistrations(idOfCourseToFind, 1, 10));
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            var course = new CourseGetDTO { Id = 1 };
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.GetCourseRegistrations(course.Id, -1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCourseRegistrations(idOfCourseToFind, -1, 10));
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ReturnsRegistrations()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO();
            var registrationsList = new PagedList<RegistrationGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<RegistrationGetDTO> { registration }, HasNextPage = false };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _registrationRepositoryMock.Setup(repo => repo.GetCustomerRegistrations(customer.Id, 1, 10)).ReturnsAsync(registrationsList);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.GetCustomerRegistrations(customer.Id, 1, 10);

            Assert.AreEqual(registrationsList, result);
            Assert.AreEqual(registrationsList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundRegistrationException()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var registrationsList = new PagedList<RegistrationGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<RegistrationGetDTO>(), HasNextPage = false };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _registrationRepositoryMock.Setup(repo => repo.GetCustomerRegistrations(customer.Id,1 ,10)).ReturnsAsync(registrationsList);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.GetCustomerRegistrations(customer.Id, 1, 10));
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).Throws(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomerRegistrations(idOfCustomerToFind, 1, 10));
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            var idOfCustomerToFind = 1;
            var customer = new CustomerGetDTO { Id = 1 };
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _registrationRepositoryMock.Setup(repo => repo.GetCustomerRegistrations(customer.Id, -1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCustomerRegistrations(idOfCustomerToFind, -1, 10));
        }

        [TestMethod]
        public async Task Get_Registration_ReturnsRegistrations()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO();
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.GetRegistration(customer.Id, course.Id)).ReturnsAsync(registration);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.GetRegistration(customer.Id, course.Id);

            Assert.AreEqual(registration, result);
        }

        [TestMethod]
        public async Task Get_Registration_ThrowsNotFoundRegistrationException()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var course = new CourseGetDTO { Id = 1 };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.GetRegistration(customer.Id, course.Id)).ReturnsAsync((RegistrationGetDTO)null);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.GetRegistration(customer.Id, course.Id));
        }

        [TestMethod]
        public async Task Get_Registration_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCustomerToFind)).Throws(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetRegistration(idOfCustomerToFind, idOfCourseToFind));
        }

        [TestMethod]
        public async Task Get_Registration_ThrowsNotFoundCourseException()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var idOfCourseToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).Throws(new NotFoundCourseException(idOfCourseToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetRegistration(customer.Id, idOfCourseToFind));
        }

        [TestMethod]
        public async Task Check_Registration_ReturnsRegistration()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            var course = new Course();
            var registration = new Registration();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.CheckRegistration(idOfCustomerToFind, idOfCourseToFind)).ReturnsAsync(registration);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.CheckRegistration(idOfCustomerToFind, idOfCourseToFind);

            Assert.AreEqual(registration, result);
        }

        [TestMethod]
        public async Task Check_Registration_ThrowsNotFoundRegistrationException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            var course = new Course();
            var registration = new Registration();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.CheckRegistration(idOfCustomerToFind, idOfCourseToFind)).ReturnsAsync((Registration)null);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.CheckRegistration(idOfCustomerToFind, idOfCourseToFind));
        }

        [TestMethod]
        public async Task Check_Registration_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            var course = new Course();
            var registration = new Registration();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.CheckRegistration(idOfCustomerToFind, idOfCourseToFind));
        }

        [TestMethod]
        public async Task Check_Registration_ThrowsNotFoundCourseException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer(); 
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.CheckRegistration(customer.Id, idOfCourseToFind));
        }


        [TestMethod]
        public async Task Post_Registration_ReturnsRegistration()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(1990, 1, 1) };
            var course = new CourseGetDTO { Id = 1, Limit = 50, CourseType = new CourseTypeGetDTO { Id = 1, MinimumAge = 18 } };
            var addedRegistrationDTO = new RegistrationGetDTO { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id};
            var addedRegistration = new Registration { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var registrationToAdd = new RegistrationPostDTO { CourseId = course.Id, CustomerId = customer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _courseServiceMock.Setup(service => service.GetCourseAssignedPeopleCount(course.Id)).ReturnsAsync(40);
            _dateTimeHelperServiceMock.Setup(service => service.GetDateTimeNow()).Returns(new DateTime(2023, 1, 1));
            _registrationRepositoryMock.SetupSequence(repo => repo.GetRegistration(registrationToAdd.CustomerId, registrationToAdd.CourseId))
                       .ReturnsAsync((RegistrationGetDTO)null)
                       .ReturnsAsync(addedRegistrationDTO);
            _registrationRepositoryMock.Setup(repo => repo.PostRegistration(registrationToAdd, _dateTimeHelperServiceMock.Object.GetDateTimeNow())).ReturnsAsync(addedRegistration);
            _customerServiceMock.Setup(service => service.CheckCustomerAgeRequirement(customer.BirthDate, course.CourseType.MinimumAge, _dateTimeHelperServiceMock.Object.GetDateTimeNow())).Returns(true);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.PostRegistration(registrationToAdd);

            Assert.AreEqual(addedRegistrationDTO, result);
        }
        
        [TestMethod]
        public async Task Post_Registration_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var registrationToAdd = new RegistrationPostDTO { CourseId = idOfCourseToFind, CustomerId = idOfCustomerToFind };
            _customerServiceMock.Setup(service => service.GetCustomer(registrationToAdd.CustomerId)).Throws(new NotFoundCustomerException(registrationToAdd.CustomerId));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostRegistration(registrationToAdd));
        }

        [TestMethod]
        public async Task Post_Registration_ThrowsNotFoundCourseException()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var idOfCourseToFind = 1;
            var registrationToAdd = new RegistrationPostDTO { CourseId = idOfCourseToFind, CustomerId = customer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(registrationToAdd.CourseId)).Throws(new NotFoundCourseException(registrationToAdd.CourseId));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostRegistration(registrationToAdd));
        }

        [TestMethod]
        public async Task Post_Registration_ThrowsAssignLimitReachedException()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var course = new CourseGetDTO { Id = 1, Limit = 50 };
            var addedRegistrationDTO = new RegistrationGetDTO { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var addedRegistration = new Registration { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var registrationToAdd = new RegistrationPostDTO { CourseId = course.Id, CustomerId = customer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _courseServiceMock.Setup(service => service.GetCourseAssignedPeopleCount(course.Id)).ReturnsAsync(50);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<AssignLimitReachedException>(async () => await _service.PostRegistration(registrationToAdd));
        }

        [TestMethod]
        public async Task Post_Registration_ThrowsCustomerAlreadyAssignedException()
        {
            var customer = new CustomerGetDTO { Id = 1 };
            var course = new CourseGetDTO { Id = 1, Limit = 50 };
            var addedRegistrationDTO = new RegistrationGetDTO { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var addedRegistration = new Registration { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var registrationToAdd = new RegistrationPostDTO { CourseId = course.Id, CustomerId = customer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _courseServiceMock.Setup(service => service.GetCourseAssignedPeopleCount(course.Id)).ReturnsAsync(40);
            _registrationRepositoryMock.Setup(repo => repo.GetRegistration(registrationToAdd.CustomerId, registrationToAdd.CourseId)).ReturnsAsync(new RegistrationGetDTO());
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerAlreadyAssignedToCourseException>(async () => await _service.PostRegistration(registrationToAdd));
        }

        [TestMethod]
        public async Task Post_Registration_ThrowsCustomerDoesntMeetRequirementsException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(1990,1,1) };
            var course = new CourseGetDTO { Id = 1, Limit = 50, CourseType = new CourseTypeGetDTO { Id = 1, MinimumAge = 18 } };
            var addedRegistrationDTO = new RegistrationGetDTO { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var addedRegistration = new Registration { RegistrationDate = new DateTime(2022, 1, 1), CourseId = course.Id, CustomerId = customer.Id };
            var registrationToAdd = new RegistrationPostDTO { CourseId = course.Id, CustomerId = customer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _courseServiceMock.Setup(service => service.GetCourseAssignedPeopleCount(course.Id)).ReturnsAsync(40);
            _registrationRepositoryMock.Setup(repo => repo.GetRegistration(registrationToAdd.CustomerId, registrationToAdd.CourseId)).ReturnsAsync((RegistrationGetDTO)null);
            _customerServiceMock.Setup(service => service.CheckCustomerAgeRequirement(customer.BirthDate, course.CourseType.MinimumAge, _dateTimeHelperServiceMock.Object.GetDateTimeNow())).Returns(false);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerDoesntMeetRequirementsException>(async () => await _service.PostRegistration(registrationToAdd));
        }

        [TestMethod]
        public async Task Delete_Registration_ReturnsRegistration()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            var course = new Course();
            var registration = new Registration();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.CheckRegistration(idOfCustomerToFind, idOfCourseToFind)).ReturnsAsync(registration);
            _registrationRepositoryMock.Setup(repo => repo.DeleteRegistration(registration)).ReturnsAsync(registration);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            var result = await _service.DeleteRegistration(idOfCustomerToFind, idOfCourseToFind);

            Assert.AreEqual(registration, result);
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsNotFoundRegistrationException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            var course = new Course();
            var registration = new Registration();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _registrationRepositoryMock.Setup(repo => repo.CheckRegistration(idOfCustomerToFind, idOfCourseToFind)).ReturnsAsync((Registration)null);
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.DeleteRegistration(idOfCustomerToFind, idOfCourseToFind));
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            var course = new Course();
            var registration = new Registration();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.DeleteRegistration(idOfCustomerToFind, idOfCourseToFind));
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsNotFoundCourseException()
        {
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            var customer = new Customer();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _service = new RegistrationService(_registrationRepositoryMock.Object, _customerServiceMock.Object, _courseServiceMock.Object, _dateTimeHelperServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.DeleteRegistration(customer.Id, idOfCourseToFind));
        }
    }
}
