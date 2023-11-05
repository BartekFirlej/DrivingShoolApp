﻿using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using Azure.Core;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class DrivingLessonRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private DrivingLessonRepository _repository;

        public DrivingLessonRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLessons()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3) };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6) };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLessons();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(new DateTime(2023, 4, 3), resultList[0].LessonDate);
            Assert.AreEqual(1, resultList[0].AddressId);
            Assert.AreEqual(1, resultList[0].LecturerId);
            Assert.AreEqual("TestLecturerName1", resultList[0].LecturerName);
            Assert.AreEqual(1, resultList[0].CustomerId);
            Assert.AreEqual("TestCustomerName1", resultList[0].CustomerName);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual(new DateTime(2023, 5, 6), resultList[1].LessonDate);
            Assert.AreEqual(2, resultList[1].AddressId);
            Assert.AreEqual(2, resultList[1].LecturerId);
            Assert.AreEqual("TestLecturerName2", resultList[1].LecturerName);
            Assert.AreEqual(2, resultList[1].CustomerId);
            Assert.AreEqual("TestCustomerName2", resultList[1].CustomerName);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLessons();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLesson()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLessonToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3) };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6) };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 5, 6), result.LessonDate);
            Assert.AreEqual("TestLecturerName2", result.LecturerName);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual("TestCustomerName2", result.CustomerName);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual(2, result.AddressId);
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLessonToFind = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3) };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6) };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsAddedDrivingLesson()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3) };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6) };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            var drivingLessonToAdd = new DrivingLessonPostDTO { AddressId = 2, CustomerId = 1, LecturerId = 2, LessonDate = new DateTime(2023, 10, 10) };
            _repository = new DrivingLessonRepository(_dbContext);

            var addedDrivingLesson = await _repository.PostDrivingLesson(drivingLessonToAdd);

            var retrievedDrivingLesson = await _repository.GetDrivingLesson(addedDrivingLesson.Id);
            Assert.IsNotNull(addedDrivingLesson);
            Assert.IsNotNull(retrievedDrivingLesson);
            Assert.AreEqual(addedDrivingLesson.Id, addedDrivingLesson.Id);
            Assert.AreEqual(drivingLessonToAdd.LecturerId, addedDrivingLesson.LecturerId);
            Assert.AreEqual(drivingLessonToAdd.CustomerId, addedDrivingLesson.CustomerId);
            Assert.AreEqual(drivingLessonToAdd.AddressId, addedDrivingLesson.AddressId);
            Assert.AreEqual(drivingLessonToAdd.LessonDate, addedDrivingLesson.LessonDate);
            Assert.AreEqual(drivingLessonToAdd.LecturerId, retrievedDrivingLesson.LecturerId);
            Assert.AreEqual(drivingLessonToAdd.CustomerId, retrievedDrivingLesson.CustomerId);
            Assert.AreEqual(drivingLessonToAdd.AddressId, retrievedDrivingLesson.AddressId);
            Assert.AreEqual(drivingLessonToAdd.LessonDate, retrievedDrivingLesson.LessonDate);
            Assert.AreEqual(customer1.Name, retrievedDrivingLesson.CustomerName);
            Assert.AreEqual(lecturer2.Name, retrievedDrivingLesson.LecturerName);
            
        }
    }
}