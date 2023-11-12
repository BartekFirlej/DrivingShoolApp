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
            ICollection<AddressGetDTO> addressesList = new List<AddressGetDTO>();
            _addressServiceMock.Setup(service => service.GetAddresses()).ReturnsAsync(addressesList);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetAddresses();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Addresses_ThrowsNotFoundAddressException()
        {
            _addressServiceMock.Setup(service => service.GetAddresses()).Throws(new NotFoundAddressException());
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetAddresses();

            result.StatusCode.Should().Be(404);
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
            _addressServiceMock.Setup(service => service.GetAddress(idOfAddressToGet)).Throws(new NotFoundAddressException(idOfAddressToGet));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetAddress(idOfAddressToGet);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Address_ReturnCreatedAtAction()
        {
            var addressToAdd = new AddressPostDTO();
            var addedAddress = new AddressGetDTO();
            _addressServiceMock.Setup(service => service.PostAddress(addressToAdd)).ReturnsAsync(addedAddress);
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostAddress(addressToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsWrongPostalCodeException()
        {
            var addressToAdd = new AddressPostDTO();
            var addedAddress = new AddressGetDTO();
            var postalCode = "22222";
            _addressServiceMock.Setup(service => service.PostAddress(addressToAdd)).Throws(new WrongPostalCodeFormatException(postalCode));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostAddress(addressToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsValueMustBeGreaterThanZeroException()
        {
            var addressToAdd = new AddressPostDTO();
            var addedAddress = new AddressGetDTO();
            var propertyName = "number";
            _addressServiceMock.Setup(service => service.PostAddress(addressToAdd)).Throws(new ValueMustBeGreaterThanZeroException(propertyName));
            _controller = new AddressController(_addressServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostAddress(addressToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}