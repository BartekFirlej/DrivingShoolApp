using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class AddressServiceTests
    {
        private Mock<IAddressRepository> _addressRepositoryMock;
        private Fixture _fixture;
        private AddressService _service;

        public AddressServiceTests()
        {
            _fixture = new Fixture();
            _addressRepositoryMock = new Mock<IAddressRepository>();
        }

        [TestMethod]
        public async Task Get_Addresses_ReturnsAddresses()
        {
            var address1 = new AddressGetDTO();
            var address2 = new AddressGetDTO();
            ICollection<AddressGetDTO> addressesList = new List<AddressGetDTO>() {address1, address2};
            _addressRepositoryMock.Setup(repo => repo.GetAddresses()).Returns(Task.FromResult(addressesList));
            _service = new AddressService(_addressRepositoryMock.Object);

            var result = await _service.GetAddresses();

            Assert.AreEqual(addressesList, result);
            Assert.AreEqual(addressesList.Count, result.Count);
        }

        [TestMethod]
        public async Task Get_Addresses_ThrowsNotFoundAddressesException()
        {
            ICollection<AddressGetDTO> addressesList = new List<AddressGetDTO>();
            _addressRepositoryMock.Setup(repo => repo.GetAddresses()).Returns(Task.FromResult(addressesList));
            _service = new AddressService(_addressRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.GetAddresses());
        }

        [TestMethod]
        public async Task Get_Address_ReturnsAddress()
        {
            var address = new AddressGetDTO();
            var idOfAddressToFind = 1;
            _addressRepositoryMock.Setup(repo => repo.GetAddress(idOfAddressToFind)).Returns(Task.FromResult(address));
            _service = new AddressService(_addressRepositoryMock.Object);

            var result = await _service.GetAddress(idOfAddressToFind);

            Assert.AreEqual(address, result);
        }

        [TestMethod]
        public async Task Get_Address_ThrowsNotFoundAddressException()
        {
            var idOfAddressToFind = 1;
            _addressRepositoryMock.Setup(repo => repo.GetAddress(idOfAddressToFind)).Returns(Task.FromResult<AddressGetDTO>(null));
            _service = new AddressService(_addressRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.GetAddress(idOfAddressToFind));
        }

        [TestMethod]
        public async Task Post_Address_ReturnsAddedAddress()
        {
            var addedAddress = new Address { Id = 1,Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var addedAddressDTO = new AddressGetDTO { ID = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var addressToAdd = new AddressPostDTO { Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            _addressRepositoryMock.Setup(repo => repo.PostAddress(addressToAdd)).Returns(Task.FromResult(addedAddress));
            _addressRepositoryMock.Setup(repo => repo.GetAddress(addedAddress.Id)).Returns(Task.FromResult(addedAddressDTO));
            _service = new AddressService(_addressRepositoryMock.Object);

            var result = await _service.PostAddress(addressToAdd);

            Assert.AreEqual(addedAddressDTO, result);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsWrongPostalCodeFormatException()
        {
            var addressToAdd = new AddressPostDTO { Street = "Mazowiecka", City = "Warszawa", PostalCode = "111111", Number = 1 };
            _service = new AddressService(_addressRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<WrongPostalCodeFormatException>(async () => await _service.PostAddress(addressToAdd));
        }

        [TestMethod]
        public async Task Post_Address_ThrowsValueMustBeGreaterThanZeroExceptionException()
        {
            var addressToAdd = new AddressPostDTO { Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 0 };
            _service = new AddressService(_addressRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostAddress(addressToAdd));
        }
    }
}
