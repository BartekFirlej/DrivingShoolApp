using AutoFixture;
using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class CustomerLectureTests
    {
        private Mock<ICustomerLectureRepository> _customerLectureRepositoryMock;
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<ILectureService> _lectureServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private CustomerLectureService _service;

        public CustomerLectureTests()
        {
            _fixture = new Fixture();
            _customerLectureRepositoryMock = new Mock<ICustomerLectureRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _lectureServiceMock = new Mock<ILectureService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_CustomersLectures_ReturnsCustomersLectures()
        {
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLectures()).ReturnsAsync(customersLecturesList);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomersLectures();

            Assert.AreEqual(customersLecturesList, result);
        }

        [TestMethod]
        public async Task Get_CustomersLectures_ThrowsNotFoundCustomerLectureException()
        {
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>();
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLectures()).ReturnsAsync(customersLecturesList);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.GetCustomersLectures());
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ReturnsCustomerLectures()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customerLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLectures(idOfCustomerToFind)).ReturnsAsync(customerLecturesList);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomerLectures(idOfCustomerToFind);

            Assert.AreEqual(customerLecturesList, result);
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ThrowsNotFoundCustomerLecturesException()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLectures(idOfCustomerToFind)).ReturnsAsync(customersLecturesList);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.GetCustomerLectures(idOfCustomerToFind));
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomerLectures(idOfCustomerToFind));
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ReturnsCustomersLecture()
        {
            var idOfLectureToFind = 1;
            var lecture = new Lecture();
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLecture(idOfLectureToFind)).ReturnsAsync(customersLecturesList);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomersLecture(idOfLectureToFind);

            Assert.AreEqual(customersLecturesList, result);
        }
        
        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundCustomersLectureException()
        {
            var idOfLectureToFind = 1;
            var lecture = new Lecture();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() {};
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLecture(idOfLectureToFind)).ReturnsAsync(customersLecturesList);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomersLectureException>(async () => await _service.GetCustomersLecture(idOfLectureToFind));
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            var lecture = new Lecture();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { };
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetCustomersLecture(idOfLectureToFind));

        }

        [TestMethod]
        public async Task Get_CustomerLecture_ReturnsCustomerLecture()
        {
            var idOfLectureToFind = 1;
            var lecture = new Lecture();
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var customerLecture = new CustomerLectureGetDTO();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind)).ReturnsAsync(customerLecture);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.AreEqual(customerLecture, result);
        }

        [TestMethod]
        public async Task Get_CustomerLecture_NotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Get_CustomerLecture_NotFoundCustomerException()
        {
            var idOfLectureToFind = 1;
            var lecture = new Lecture();
            var idOfCustomerToFind = 1;
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Get_CustomerLecture_NotFoundCustomerLectureException()
        {
            var idOfLectureToFind = 1;
            var lecture = new Lecture();
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind)).ReturnsAsync((CustomerLectureGetDTO)null);
            
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Check_CustomerLecture_ReturnsCustomerLecture()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture();
            var customer = new Customer();
            var customerLecture = new CustomerLectureCheckDTO();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind)).ReturnsAsync(customerLecture);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.AreEqual(customerLecture, result);
        }

        [TestMethod]
        public async Task Check_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture();
            var customer = new Customer();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Check_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Check_CustomerLecture_ThrowsNotFoundCustomerLectureException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture();
            var customer = new Customer();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind)).ReturnsAsync((CustomerLectureCheckDTO)null);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ReturnsAddedCustomerLecture()
        {
            var lecture = new Lecture { Id = 1, CourseId = 1};
            var customer = new Customer { Id = 1};
            var registration = new Registration { CourseId = 1, CustomerId = 1};
            var customerLectureToAdd = new CustomerLectureRequestDTO { CustomerId =1 , LectureId = 1};
            var addedCustomerLecture = new CustomerLectureResponseDTO { CustomerId = 1, LectureId = 1};
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lectureServiceMock.Setup(service => service.CheckLecture(lecture.Id)).ReturnsAsync(lecture);
            _registrationServiceMock.Setup(service => service.CheckRegistration(customer.Id, lecture.CourseId)).ReturnsAsync(registration);
            _customerLectureRepositoryMock.Setup(repo => repo.CheckCustomerLecture(customer.Id, lecture.Id)).ReturnsAsync((CustomerLectureCheckDTO)null);
            _customerLectureRepositoryMock.Setup(repo => repo.PostCustomerLecture(customerLectureToAdd)).ReturnsAsync(addedCustomerLecture);

            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostCustomerLecture(customerLectureToAdd);

            Assert.AreEqual(addedCustomerLecture, result);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            var customerLectureToAdd = new CustomerLectureRequestDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            var customer = new Customer { Id = 1};
            var customerLectureToAdd = new CustomerLectureRequestDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundRegistrationException()
        {
            var lecture = new Lecture { Id = 1, CourseId = 1 };
            var customer = new Customer { Id = 1};
            var customerLectureToAdd = new CustomerLectureRequestDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lectureServiceMock.Setup(service => service.CheckLecture(lecture.Id)).ReturnsAsync(lecture);
            _registrationServiceMock.Setup(service => service.CheckRegistration(customer.Id, lecture.CourseId)).ThrowsAsync(new NotFoundRegistrationException());
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRegistrationException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsCustomerAlreadyAssignedToLectureException()
        {
            var lecture = new Lecture { Id = 1, CourseId = 1};
            var customer = new Customer { Id = 1};
            var registration = new Registration { CourseId = 1,CustomerId = 1};
            var customerLecture = new CustomerLectureCheckDTO { CustomerId = 1, LectureId = 1};
            var customerLectureToAdd = new CustomerLectureRequestDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lectureServiceMock.Setup(service => service.CheckLecture(lecture.Id)).ReturnsAsync(lecture);
            _registrationServiceMock.Setup(service => service.CheckRegistration(customer.Id, lecture.CourseId)).ReturnsAsync(registration);
            _customerLectureRepositoryMock.Setup(repo => repo.CheckCustomerLecture(customer.Id, lecture.Id)).ReturnsAsync(customerLecture); 
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerAlreadyAssignedToLectureException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ReturnsCustomerLecture()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture{ Id = 1 };
            var customer = new Customer { Id = 1};
            var customerLecture = new CustomerLectureCheckDTO();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind)).ReturnsAsync(customerLecture);
            _customerLectureRepositoryMock.Setup(repo => repo.DeleteCustomerLecture(customer, lecture)).ReturnsAsync(customerLecture);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.AreEqual(customerLecture, result);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.DeleteCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.DeleteCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsNotFoundCustomerLectureException()
        {
            var idOfLectureToFind = 1;
            var idOfCustomerToFind = 1;
            var lecture = new Lecture();
            var customer = new Customer();
            _lectureServiceMock.Setup(service => service.CheckLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _customerLectureRepositoryMock.Setup(repo => repo.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind)).ReturnsAsync((CustomerLectureCheckDTO)null);
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.DeleteCustomerLecture(idOfCustomerToFind, idOfLectureToFind));
        }
    }
}
