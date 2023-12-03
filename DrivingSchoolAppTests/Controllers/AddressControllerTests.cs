using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class AddressControllerTests
    {
        private Mock<IAddressService> _addressServiceMock;
        private Fixture _fixture;
        private AddressController _controller;

        public AddressControllerTests()
        {
            _fixture = new Fixture();
            _addressServiceMock = new Mock<IAddressService>();
        }

        [TestMethod]
        public async Task Get_Addresses_ReturnsOk()
        {
            PagedList<AddressGetDTO> addressesList = new PagedList<AddressGetDTO>();
            _addressServiceMock.Setup(service => service.GetAddresses(1,10)).ReturnsAsync(addressesList);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetAddresses();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Addresses_ThrowsNotFoundAddressException()
        {
            _addressServiceMock.Setup(service => service.GetAddresses(1,10)).ThrowsAsync(new NotFoundAddressException());
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetAddresses();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Addresses_ThrowsValueMustBeGreaterThanZeroException()
        {
            _addressServiceMock.Setup(service => service.GetAddresses(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetAddresses(-1,10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Address_ReturnsOk()
        {
            var address = new AddressGetDTO();
            var idOfAddressToGet = 1;
            _addressServiceMock.Setup(service => service.GetAddress(idOfAddressToGet)).ReturnsAsync(address);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetAddress(idOfAddressToGet);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Address_ThrowsNotFoundAddressException()
        {
            var idOfAddressToGet = 1;
            _addressServiceMock.Setup(service => service.GetAddress(idOfAddressToGet)).ThrowsAsync(new NotFoundAddressException(idOfAddressToGet));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetAddress(idOfAddressToGet);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Address_ReturnCreatedAtAction()
        {
            var addressToAdd = new AddressRequestDTO();
            var addedAddress = new AddressResponseDTO();
            _addressServiceMock.Setup(service => service.PostAddress(addressToAdd)).ReturnsAsync(addedAddress);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostAddress(addressToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsWrongPostalCodeException()
        {
            var addressToAdd = new AddressRequestDTO();
            var addedAddress = new AddressGetDTO();
            var postalCode = "22222";
            _addressServiceMock.Setup(service => service.PostAddress(addressToAdd)).ThrowsAsync(new WrongPostalCodeFormatException(postalCode));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostAddress(addressToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsValueMustBeGreaterThanZeroException()
        {
            var addressToAdd = new AddressRequestDTO();
            var addedAddress = new AddressGetDTO();
            var propertyName = "number";
            _addressServiceMock.Setup(service => service.PostAddress(addressToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(propertyName));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostAddress(addressToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_Address_ReturnNoContent()
        {
            var deletedAddress = new Address();
            var idOfAddressToDelete = 1;
            _addressServiceMock.Setup(service => service.DeleteAddress(idOfAddressToDelete)).ReturnsAsync(deletedAddress);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteAddress(idOfAddressToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsNotFoundAddressException()
        {
            var idOfAddressToDelete = 1;
            _addressServiceMock.Setup(service => service.DeleteAddress(idOfAddressToDelete)).ThrowsAsync(new NotFoundAddressException(idOfAddressToDelete));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteAddress(idOfAddressToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsReferenceConstraintException()
        {
            var idOfAddressToDelete = 1;
            _addressServiceMock.Setup(service => service.DeleteAddress(idOfAddressToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteAddress(idOfAddressToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsDbUpdateException()
        {
            var idOfAddressToDelete = 1;
            _addressServiceMock.Setup(service => service.DeleteAddress(idOfAddressToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteAddress(idOfAddressToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsException()
        {
            var idOfAddressToDelete = 1;
            _addressServiceMock.Setup(service => service.DeleteAddress(idOfAddressToDelete)).ThrowsAsync(new Exception());
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteAddress(idOfAddressToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Update_Address_ReturnNoContent()
        {
            var updateAddress = new AddressRequestDTO();
            var updatedAddress = new AddressResponseDTO();
            var idOfAddressToUpdate = 1;
            _addressServiceMock.Setup(service => service.UpdateAddress(idOfAddressToUpdate, updateAddress)).ReturnsAsync(updatedAddress);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (OkObjectResult)await _controller.UpdateAddress(idOfAddressToUpdate, updateAddress);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Update_Address_ThrowsNotFoundAddressException()
        {
            var updateAddress = new AddressRequestDTO();
            var idOfAddressToUpdate = 1;
            _addressServiceMock.Setup(service => service.UpdateAddress(idOfAddressToUpdate, updateAddress)).ThrowsAsync(new NotFoundAddressException(idOfAddressToUpdate));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateAddress(idOfAddressToUpdate, updateAddress);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_Address_ThrowsWrongPostalCodeException()
        {
            var updateAddress = new AddressRequestDTO();
            var idOfAddressToUpdate = 1;
            _addressServiceMock.Setup(service => service.UpdateAddress(idOfAddressToUpdate, updateAddress)).ThrowsAsync(new WrongPostalCodeFormatException("22222"));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.UpdateAddress(idOfAddressToUpdate, updateAddress);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Update_Address_ThrowsValueMustBeGreaterThanZeroException()
        {
            var updateAddress = new AddressRequestDTO();
            var idOfAddressToUpdate = 1;
            _addressServiceMock.Setup(service => service.UpdateAddress(idOfAddressToUpdate, updateAddress)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("number"));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.UpdateAddress(idOfAddressToUpdate, updateAddress);

            result.StatusCode.Should().Be(400);
        }
    }
}