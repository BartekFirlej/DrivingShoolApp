using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DrivingSchoolAppTests.Models
{
    [TestClass]
    public class PagedListTests
    {
        [TestMethod]
        public async Task PagedList_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            var source = Enumerable.Range(1, 10).AsQueryable();
            var pageIndex = -1;
            var pageSize = 5;

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await PagedList<int>.Create(source, pageIndex, pageSize));
        }

        [TestMethod]
        public async Task PagedList_ThrowsPageSizeMustBeGreaterThanZeroException()
        {
            var source = Enumerable.Range(1, 10).AsQueryable();
            var pageIndex = 1;
            var pageSize = -5;

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await PagedList<int>.Create(source, pageIndex, pageSize));
        }

        [TestMethod]
        public async Task PagedList_ReturnsOnePageWithoutSecond()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            var pageIndex = 1;
            var pageSize = 10;
            var _repository = new AddressRepository(_dbContext);

            var resultList = await PagedList<AddressGetDTO>.Create(
                 _dbContext.Addresses.Select(a => new AddressGetDTO
                 {
                     Id = a.Id,
                     City = a.City,
                     Street = a.Street,
                     Number = a.Number,
                     PostalCode = a.PostalCode
                 })
                .OrderBy(a => a.Id),
                 pageIndex, pageSize);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestCity1", resultList.PagedItems[0].City);
            Assert.AreEqual("TestStreet1", resultList.PagedItems[0].Street);
            Assert.AreEqual(10, resultList.PagedItems[0].Number);
            Assert.AreEqual("22-222", resultList.PagedItems[0].PostalCode);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("TestCity2", resultList.PagedItems[1].City);
            Assert.AreEqual("TestStreet2", resultList.PagedItems[1].Street);
            Assert.AreEqual(20, resultList.PagedItems[1].Number);
            Assert.AreEqual("44-444", resultList.PagedItems[1].PostalCode);
            Assert.AreEqual(pageIndex, resultList.PageIndex);
            Assert.AreEqual(pageSize, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);
        }

        [TestMethod]
        public async Task PagedList_ReturnsOnePageWithSecond()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            var pageIndex = 1;
            var pageSize = 1;
            var _repository = new AddressRepository(_dbContext);

            var resultList = await PagedList<AddressGetDTO>.Create(
                 _dbContext.Addresses.Select(a => new AddressGetDTO
                 {
                     Id = a.Id,
                     City = a.City,
                     Street = a.Street,
                     Number = a.Number,
                     PostalCode = a.PostalCode
                 })
                .OrderBy(a => a.Id),
                 pageIndex, pageSize);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(1, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestCity1", resultList.PagedItems[0].City);
            Assert.AreEqual("TestStreet1", resultList.PagedItems[0].Street);
            Assert.AreEqual(10, resultList.PagedItems[0].Number);
            Assert.AreEqual("22-222", resultList.PagedItems[0].PostalCode);
            Assert.AreEqual(pageIndex, resultList.PageIndex);
            Assert.AreEqual(pageSize, resultList.PageSize);
            Assert.IsTrue(resultList.HasNextPage);
        }

        [TestMethod]
        public async Task PagedList_ReturnsEmptyOnePage()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var _dbContext = new DrivingSchoolDbContext(options);
            var pageIndex = 1;
            var pageSize = 1;
            var _repository = new AddressRepository(_dbContext);

            var resultList = await PagedList<AddressGetDTO>.Create(
                 _dbContext.Addresses.Select(a => new AddressGetDTO
                 {
                     Id = a.Id,
                     City = a.City,
                     Street = a.Street,
                     Number = a.Number,
                     PostalCode = a.PostalCode
                 })
                .OrderBy(a => a.Id),
                 pageIndex, pageSize);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(0, resultList.PagedItems.Count);
            Assert.AreEqual(pageIndex, resultList.PageIndex);
            Assert.AreEqual(pageSize, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);
        }
    }
}
