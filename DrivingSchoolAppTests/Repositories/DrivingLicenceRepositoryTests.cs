using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class DrivingLicenceRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private DrivingLicenceRepository _repository;

        public DrivingLicenceRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ReturnsDrivingLicences()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11)};
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var resultList = await _repository.GetDrivingLicences(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual(1, resultList.PagedItems[0].CustomerId);
            Assert.AreEqual("TestCustomerName1", resultList.PagedItems[0].CustomerName);
            Assert.AreEqual("TestCustomerSName1", resultList.PagedItems[0].CutomserSecondName);
            Assert.AreEqual(1, resultList.PagedItems[0].LicenceCategoryId);
            Assert.AreEqual("Test1", resultList.PagedItems[0].LicenceCategoryName);
            Assert.AreEqual(new DateTime(2020, 1, 1), resultList.PagedItems[0].ReceivedDate);
            Assert.AreEqual(new DateTime(2025, 1, 1), resultList.PagedItems[0].ExpirationDate);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual(2, resultList.PagedItems[1].CustomerId);
            Assert.AreEqual("TestCustomerName2", resultList.PagedItems[1].CustomerName);
            Assert.AreEqual("TestCustomerSName2", resultList.PagedItems[1].CutomserSecondName);
            Assert.AreEqual(2, resultList.PagedItems[1].LicenceCategoryId);
            Assert.AreEqual("Test2", resultList.PagedItems[1].LicenceCategoryName);
            Assert.AreEqual(new DateTime(2020, 11, 11), resultList.PagedItems[1].ReceivedDate);
            Assert.AreEqual(null, resultList.PagedItems[1].ExpirationDate);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.GetDrivingLicences(1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);
            Assert.IsFalse(result.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLicences_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new DrivingLicenceRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetDrivingLicences(-1, 10));

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsDrivingLicence()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLicenceToFind = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.GetDrivingLicence(idOfDrivingLicenceToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual("TestCustomerName2", result.CustomerName);
            Assert.AreEqual("TestCustomerSName2", result.CutomserSecondName);
            Assert.AreEqual(2, result.LicenceCategoryId);
            Assert.AreEqual("Test2", result.LicenceCategoryName);
            Assert.AreEqual(new DateTime(2020, 11, 11), result.ReceivedDate);
            Assert.AreEqual(new DateTime(2025, 11, 11), result.ExpirationDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLicenceToFind = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.GetDrivingLicence(idOfDrivingLicenceToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ReturnsDrivingLicences()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2021, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 1, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            var drivingLicence3 = new DrivingLicence { Id = 3, CustomerId = 1, LicenceCategoryId = 2, ReceivedDate = new DateTime(1990, 11, 11), ExpirationDate = new DateTime(2022, 11, 11) };
            await _dbContext.Customers.AddAsync(customer1);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2, drivingLicence3);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.GetCustomerDrivingLicences(1, new DateTime(2023, 10, 10));

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(1, resultList[0].CustomerId);
            Assert.AreEqual("TestCustomerName1", resultList[0].CustomerName);
            Assert.AreEqual("TestCustomerSName1", resultList[0].CutomserSecondName);
            Assert.AreEqual(1, resultList[0].LicenceCategoryId);
            Assert.AreEqual("Test1", resultList[0].LicenceCategoryName);
            Assert.AreEqual(new DateTime(2021, 1, 1), resultList[0].ReceivedDate);
            Assert.AreEqual(new DateTime(2025, 1, 1), resultList[0].ExpirationDate);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual(1, resultList[1].CustomerId);
            Assert.AreEqual("TestCustomerName1", resultList[1].CustomerName);
            Assert.AreEqual("TestCustomerSName1", resultList[1].CutomserSecondName);
            Assert.AreEqual(2, resultList[1].LicenceCategoryId);
            Assert.AreEqual("Test2", resultList[1].LicenceCategoryName);
            Assert.AreEqual(new DateTime(2020, 11, 11), resultList[1].ReceivedDate);
            Assert.AreEqual(new DateTime(2025, 11, 11), resultList[1].ExpirationDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2021, 1, 1), ExpirationDate = new DateTime(2022, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 1, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2022, 11, 11) };
            var drivingLicence3 = new DrivingLicence { Id = 3, CustomerId = 1, LicenceCategoryId = 2, ReceivedDate = new DateTime(1990, 11, 11), ExpirationDate = new DateTime(2022, 11, 11) };
            await _dbContext.Customers.AddAsync(customer1);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2, drivingLicence3);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.GetCustomerDrivingLicences(1, new DateTime(2023, 10, 10));

            Assert.IsFalse(result.Any());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ReturnsAddedDrivingLicence()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = 2, LicenceCategoryId = 1, ReceivedDate = new DateTime(2023, 11, 8), ExpirationDate = new DateTime(2025, 11, 8) };
            _repository = new DrivingLicenceRepository(_dbContext);

            var addedDrivingLicence = await _repository.PostDrivingLicence(drivingLicenceToAdd);

            var retrievedDrivingLicence = await _repository.GetDrivingLicence(addedDrivingLicence.Id);
            Assert.IsNotNull(addedDrivingLicence);
            Assert.IsNotNull(retrievedDrivingLicence);
            Assert.AreEqual(addedDrivingLicence.Id, addedDrivingLicence.Id);
            Assert.AreEqual(drivingLicenceToAdd.LicenceCategoryId, addedDrivingLicence.LicenceCategoryId);
            Assert.AreEqual(drivingLicenceToAdd.CustomerId, addedDrivingLicence.CustomerId);
            Assert.AreEqual(drivingLicenceToAdd.ReceivedDate, addedDrivingLicence.ReceivedDate);
            Assert.AreEqual(drivingLicenceToAdd.ExpirationDate, addedDrivingLicence.ExpirationDate);
            Assert.AreEqual(drivingLicenceToAdd.LicenceCategoryId, retrievedDrivingLicence.LicenceCategoryId);
            Assert.AreEqual(drivingLicenceToAdd.CustomerId, retrievedDrivingLicence.CustomerId);
            Assert.AreEqual(drivingLicenceToAdd.ReceivedDate, retrievedDrivingLicence.ReceivedDate);
            Assert.AreEqual(drivingLicenceToAdd.ExpirationDate, retrievedDrivingLicence.ExpirationDate);
            Assert.AreEqual(customer2.Name, retrievedDrivingLicence.CustomerName);
            Assert.AreEqual(customer2.SecondName, retrievedDrivingLicence.CutomserSecondName);
            Assert.AreEqual(licenceCategory1.Name, retrievedDrivingLicence.LicenceCategoryName);
            Assert.AreEqual(3, await _dbContext.DrivingLicences.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_DrivingLicenceWithoutExpirationDate_ReturnsAddedDrivingLicence()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = 2, LicenceCategoryId = 1, ReceivedDate = new DateTime(2023, 11, 8)};
            _repository = new DrivingLicenceRepository(_dbContext);

            var addedDrivingLicence = await _repository.PostDrivingLicence(drivingLicenceToAdd);

            var retrievedDrivingLicence = await _repository.GetDrivingLicence(addedDrivingLicence.Id);
            Assert.IsNotNull(addedDrivingLicence);
            Assert.IsNotNull(retrievedDrivingLicence);
            Assert.AreEqual(addedDrivingLicence.Id, addedDrivingLicence.Id);
            Assert.AreEqual(drivingLicenceToAdd.LicenceCategoryId, addedDrivingLicence.LicenceCategoryId);
            Assert.AreEqual(drivingLicenceToAdd.CustomerId, addedDrivingLicence.CustomerId);
            Assert.AreEqual(drivingLicenceToAdd.ReceivedDate, addedDrivingLicence.ReceivedDate);
            Assert.AreEqual(null, addedDrivingLicence.ExpirationDate);
            Assert.AreEqual(drivingLicenceToAdd.LicenceCategoryId, retrievedDrivingLicence.LicenceCategoryId);
            Assert.AreEqual(drivingLicenceToAdd.CustomerId, retrievedDrivingLicence.CustomerId);
            Assert.AreEqual(drivingLicenceToAdd.ReceivedDate, retrievedDrivingLicence.ReceivedDate);
            Assert.AreEqual(null, retrievedDrivingLicence.ExpirationDate);
            Assert.AreEqual(customer2.Name, retrievedDrivingLicence.CustomerName);
            Assert.AreEqual(customer2.SecondName, retrievedDrivingLicence.CutomserSecondName);
            Assert.AreEqual(licenceCategory1.Name, retrievedDrivingLicence.LicenceCategoryName);
            Assert.AreEqual(3, await _dbContext.DrivingLicences.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ReturnsDrivingLicence()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.DeleteDrivingLicence(drivingLicence2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual(new DateTime(2020, 11, 11), result.ReceivedDate);
            Assert.AreEqual(new DateTime(2025, 11, 11), result.ExpirationDate);
            Assert.AreEqual(1, await _dbContext.DrivingLicences.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_DrivingLicence_ReturnsDrivingLicence()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLicenceToFind = 2;
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.CheckDrivingLicence(idOfDrivingLicenceToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual(new DateTime(2020, 11, 11), result.ReceivedDate);
            Assert.AreEqual(new DateTime(2025, 11, 11), result.ExpirationDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_DrivingLicence_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLicenceToFind = 3;
            var drivingLicence1 = new DrivingLicence { Id = 1, CustomerId = 1, LicenceCategoryId = 1, ReceivedDate = new DateTime(2020, 1, 1), ExpirationDate = new DateTime(2025, 1, 1) };
            var drivingLicence2 = new DrivingLicence { Id = 2, CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2020, 11, 11), ExpirationDate = new DateTime(2025, 11, 11) };
            await _dbContext.DrivingLicences.AddRangeAsync(drivingLicence1, drivingLicence2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLicenceRepository(_dbContext);

            var result = await _repository.CheckDrivingLicence(idOfDrivingLicenceToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }
    }
}