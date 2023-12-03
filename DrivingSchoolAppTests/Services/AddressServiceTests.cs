using AutoFixture;
using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using EntityFramework.Exceptions.Common;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class AddressServiceTests
    {
        private Mock<IAddressRepository> _addressRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private AddressService _service;

        public AddressServiceTests()
        {
            _fixture = new Fixture();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Addresses_ReturnsAddresses()
        {
            var address1 = new AddressGetDTO();
            var address2 = new AddressGetDTO();
            var addressesList = new PagedList<AddressGetDTO>() { PagedItems = new List<AddressGetDTO> { address1, address2 }, HasNextPage = false, PageIndex = 1, PageSize = 10 };
            _addressRepositoryMock.Setup(repo => repo.GetAddresses(1,10)).ReturnsAsync(addressesList);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetAddresses(1,10);

            Assert.AreEqual(addressesList, result);
            Assert.AreEqual(addressesList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_Addresses_ThrowsNotFoundAddressesException()
        {
            var addressesList = new PagedList<AddressGetDTO> { PagedItems = new List<AddressGetDTO>(), HasNextPage = false, PageIndex = 1, PageSize = 10 };
            _addressRepositoryMock.Setup(repo => repo.GetAddresses(1,10)).ReturnsAsync(addressesList);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.GetAddresses(1,10));
        }

        [TestMethod]
        public async Task Get_Addresses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _addressRepositoryMock.Setup(repo => repo.GetAddresses(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetAddresses(-1, 10));
        }

        [TestMethod]
        public async Task Get_Address_ReturnsAddress()
        {
            var address = new AddressGetDTO();
            var idOfAddressToFind = 1;
            _addressRepositoryMock.Setup(repo => repo.GetAddress(idOfAddressToFind)).ReturnsAsync(address);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetAddress(idOfAddressToFind);

            Assert.AreEqual(address, result);
        }

        [TestMethod]
        public async Task Get_Address_ThrowsNotFoundAddressException()
        {
            var idOfAddressToFind = 1;
            _addressRepositoryMock.Setup(repo => repo.GetAddress(idOfAddressToFind)).ReturnsAsync((AddressGetDTO)null);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.GetAddress(idOfAddressToFind));
        }

        [TestMethod]
        public async Task Post_Address_ReturnsAddedAddress()
        {
            var addedAddress = new Address { Id = 1,Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var addedAddressDTO = new AddressResponseDTO { Id = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var addressToAdd = new AddressRequestDTO { Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            _addressRepositoryMock.Setup(repo => repo.PostAddress(addressToAdd)).ReturnsAsync(addedAddress);
            _mapperMock.Setup(m => m.Map<AddressResponseDTO>(It.IsAny<Address>())).Returns(addedAddressDTO);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.PostAddress(addressToAdd);

            Assert.AreEqual(addedAddressDTO, result);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsWrongPostalCodeFormatException()
        {
            var addressToAdd = new AddressRequestDTO { Street = "Mazowiecka", City = "Warszawa", PostalCode = "111111", Number = 1 };
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<WrongPostalCodeFormatException>(async () => await _service.PostAddress(addressToAdd));
        }

        [TestMethod]
        public async Task Post_Address_ThrowsValueMustBeGreaterThanZeroExceptionException()
        {
            var addressToAdd = new AddressRequestDTO { Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 0 };
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostAddress(addressToAdd));
        }

        [TestMethod]
        public async Task Delete_Address_ReturnsAddress()
        {
            var deletedAddress = new Address { Id = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var idOfAddressToDelete = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddress(idOfAddressToDelete)).ReturnsAsync(deletedAddress);
            _addressRepositoryMock.Setup(repo => repo.DeleteAddress(deletedAddress)).ReturnsAsync(deletedAddress);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.DeleteAddress(idOfAddressToDelete);

            Assert.AreEqual(deletedAddress, result);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsNotFoundSubjectException()
        {
            var idOfAddressToDelete = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddress(idOfAddressToDelete)).ReturnsAsync((Address)null);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.DeleteAddress(idOfAddressToDelete));
        }

        [TestMethod]
        public async Task Delete_Address_PropagatesReferenceConstraintExceptionException()
        {
            var deletedAddress = new Address { Id = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var idOfAddressToDelete = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddress(idOfAddressToDelete)).ReturnsAsync(deletedAddress);
            _addressRepositoryMock.Setup(repo => repo.DeleteAddress(deletedAddress)).ThrowsAsync(new ReferenceConstraintException());
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteAddress(idOfAddressToDelete));
        }

        [TestMethod]
        public async Task Check_Address_ReturnsAddress()
        {
            var address = new Address { Id = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var idOfAddress = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddress(idOfAddress)).ReturnsAsync(address);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckAddress(idOfAddress);

            Assert.AreEqual(address, result);
        }

        [TestMethod]
        public async Task Check_Address_ThrowsNotFoundAddressException()
        {
            var idOfAddress = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddress(idOfAddress)).ReturnsAsync((Address)null);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.CheckAddress(idOfAddress));
        }

        [TestMethod]
        public async Task Check_AddressTracking_ReturnsAddress()
        {
            var address = new Address { Id = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var idOfAddress = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddressTracking(idOfAddress)).ReturnsAsync(address);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckAddressTracking(idOfAddress);

            Assert.AreEqual(address, result);
        }

        [TestMethod]
        public async Task Check_AddressTracking_ThrowsNotFoundAddressException()
        {
            var idOfAddress = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddressTracking(idOfAddress)).ReturnsAsync((Address)null);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.CheckAddress(idOfAddress));
        }

        [TestMethod]
        public async Task Update_Address_ReturnsAddress()
        {
            var address = new Address { Id = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var updateAddress = new AddressRequestDTO { City = "UpdatedCity1", Street = "UpdatedStreet1", Number = 100, PostalCode = "99-999" };
            var updatedAddress = new AddressResponseDTO { Id = 1, City = "UpdatedCity1", Street = "UpdatedStreet1", Number = 100, PostalCode = "99-999" };
            var idOfAddress = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddressTracking(idOfAddress)).ReturnsAsync(address);
            _mapperMock.Setup(m => m.Map<AddressResponseDTO>(It.IsAny<Address>())).Returns(updatedAddress);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.UpdateAddress(idOfAddress, updateAddress);

            Assert.AreEqual(result, updatedAddress);
        }

        [TestMethod]
        public async Task Update_Address_ThrowsNotFoundAddressException()
        {
            var updateAddress = new AddressRequestDTO { City = "UpdatedCity1", Street = "UpdatedStreet1", Number = 100, PostalCode = "99-999" };
             var idOfAddress = 1;
            _addressRepositoryMock.Setup(repo => repo.CheckAddress(idOfAddress)).ReturnsAsync((Address)null);
            _service = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.UpdateAddress(1, updateAddress));
        }
    }
}
