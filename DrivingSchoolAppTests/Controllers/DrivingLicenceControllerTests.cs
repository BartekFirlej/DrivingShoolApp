using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class DrivingLicenceControllerTests
    {
        private Mock<IDrivingLicenceService> _drivingLicenceServiceMock;
        private Fixture _fixture;
        private DrivingLicenceController _controller;

        public DrivingLicenceControllerTests()
        {
            _fixture = new Fixture();
            _drivingLicenceServiceMock = new Mock<IDrivingLicenceService>();
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ReturnsOk()
        {
            var drivingLicencesList = new PagedList<DrivingLicenceGetDTO>();
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicences(1, 10)).Returns(Task.FromResult(drivingLicencesList));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLicences();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsNotFoundDrivingLicencesException()
        {
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicences(1, 10)).Throws(new NotFoundDrivingLicenceException());
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLicences(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicences(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetDrivingLicences(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsOk()
        {
            var foundDrivingLicence = new DrivingLicenceGetDTO();
            var idOfDrivingLicence = 1;
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicence(idOfDrivingLicence)).Returns(Task.FromResult(foundDrivingLicence));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicence = 1;
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicence(idOfDrivingLicence)).Throws(new NotFoundDrivingLicenceException(idOfDrivingLicence));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ReturnsCreatedAtAction()
        {
            var drivingLicenceToAdd = new DrivingLicencePostDTO();
            var addedDrivingLicence = new DrivingLicenceGetDTO();
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).Returns(Task.FromResult(addedDrivingLicence));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundCustomerException()
        {
            var drivingLicenceToAdd = new DrivingLicencePostDTO();
            var idOfCustomer = 1;
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).Throws(new NotFoundCustomerException(idOfCustomer));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundLicenceCategoryException()
        {
            var drivingLicenceToAdd = new DrivingLicencePostDTO();
            var idOfLicenceCategory = 1;
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).Throws(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsCustomerDoesntMeetRequirementException()
        {
            var drivingLicenceToAdd = new DrivingLicencePostDTO();
            var idOfCustomer = 1;
            var idOfLicenceCategory = 1;
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).Throws(new CustomerDoesntMeetRequirementsException(idOfCustomer,idOfLicenceCategory));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsDateTimeException()
        {
            var drivingLicenceToAdd = new DrivingLicencePostDTO();
            var propertyName = "expiration date";
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).Throws(new DateTimeException(propertyName));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}