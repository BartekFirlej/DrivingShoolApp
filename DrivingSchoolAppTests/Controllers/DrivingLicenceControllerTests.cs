using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

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
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicences(1, 10)).ReturnsAsync(drivingLicencesList);
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLicences();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsNotFoundDrivingLicencesException()
        {
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicences(1, 10)).ThrowsAsync(new NotFoundDrivingLicenceException());
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLicences(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicences(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetDrivingLicences(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsOk()
        {
            var foundDrivingLicence = new DrivingLicenceGetDTO();
            var idOfDrivingLicence = 1;
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicence(idOfDrivingLicence)).ReturnsAsync(foundDrivingLicence);
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicence = 1;
            _drivingLicenceServiceMock.Setup(service => service.GetDrivingLicence(idOfDrivingLicence)).ThrowsAsync(new NotFoundDrivingLicenceException(idOfDrivingLicence));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ReturnsCreatedAtAction()
        {
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO();
            var addedDrivingLicence = new DrivingLicenceResponseDTO();
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).ReturnsAsync(addedDrivingLicence);
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundCustomerException()
        {
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO();
            var idOfCustomer = 1;
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).ThrowsAsync(new NotFoundCustomerException(idOfCustomer));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundLicenceCategoryException()
        {
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO();
            var idOfLicenceCategory = 1;
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsCustomerDoesntMeetRequirementException()
        {
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO();
            var idOfCustomer = 1;
            var idOfLicenceCategory = 1;
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).ThrowsAsync(new CustomerDoesntMeetRequirementsException(idOfCustomer,idOfLicenceCategory));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsDateTimeException()
        {
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO();
            var propertyName = "expiration date";
            _drivingLicenceServiceMock.Setup(service => service.PostDrivingLicence(drivingLicenceToAdd)).ThrowsAsync(new DateTimeException(propertyName));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostDrivingLicence(drivingLicenceToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ReturnsNoContent()
        {
            var idOfDrivingLicence = 5;
            var deletedDrivingLicence = new DrivingLicence();
            _drivingLicenceServiceMock.Setup(service => service.DeleteDrivingLicence(idOfDrivingLicence)).ReturnsAsync(deletedDrivingLicence);
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicence = 5;
            _drivingLicenceServiceMock.Setup(service => service.DeleteDrivingLicence(idOfDrivingLicence)).ThrowsAsync(new NotFoundDrivingLicenceException(idOfDrivingLicence));
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ThrowsDbUpdateException()
        {
            var idOfDrivingLicence = 5;
            _drivingLicenceServiceMock.Setup(service => service.DeleteDrivingLicence(idOfDrivingLicence)).ThrowsAsync(new DbUpdateException());
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ThrowsException()
        {
            var idOfDrivingLicence = 5;
            _drivingLicenceServiceMock.Setup(service => service.DeleteDrivingLicence(idOfDrivingLicence)).ThrowsAsync(new Exception());
            _controller = new DrivingLicenceController(_drivingLicenceServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteDrivingLicence(idOfDrivingLicence);

            result.StatusCode.Should().Be(500);
        }
    }
}