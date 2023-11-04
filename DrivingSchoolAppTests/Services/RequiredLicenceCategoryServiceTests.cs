using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using FluentAssertions;
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
    public class RequiredLicenceCategoryServiceTests
    {
        private Mock<ILicenceCategoryService> _licenceCategoryServiceMock;
        private Mock<IRequiredLicenceCategoryRepository> _requiredLicenceCategoryRepositoryMock;
        private Fixture _fixture;
        private RequiredLicenceCategoryService _service;

        public RequiredLicenceCategoryServiceTests()
        {
            _fixture = new Fixture();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
            _requiredLicenceCategoryRepositoryMock = new Mock<IRequiredLicenceCategoryRepository>();
        }

        [TestMethod]
        public async Task Get_Requirements_ReturnsRequirements()
        {
            var requirement = new RequiredLicenceCategoryGetDTO();
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements()).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.GetRequirements();

            Assert.AreEqual(requirementsList, result);
        }

        [TestMethod]
        public async Task Get_Requirements_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements()).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.GetRequirements());
        }

        [TestMethod]
        public async Task Get_Requirement_ReturnsRequirement()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id);
            Assert.AreEqual(requirement, result);
        }

        [TestMethod]
        public async Task Get_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id));
        }

        [TestMethod]
        public async Task Get_Requirement_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requiredLicenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id));
        }

        [TestMethod]
        public async Task Get_Requirement_ThrowsNotFoundRequirementException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id)).ReturnsAsync((RequiredLicenceCategoryGetDTO)null);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id));
        }

        [TestMethod]
        public async Task Get_CategoryRequirements_ReturnsRequirements()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.GetRequirements(licenceCategory.Id);
            Assert.AreEqual(requirementsList, result);
        }

        [TestMethod]
        public async Task Get_CategoryRequirements_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetRequirements(licenceCategory.Id));
        }

        [TestMethod]
        public async Task Get_CategoryRequirements_ThrowsNotFoundRequirementsException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1 };
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.GetRequirements(licenceCategory.Id));
        }
        
        [TestMethod]
        public async Task Post_Requirement_ReturnsRequirement()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var addedRequiredLicenceCategoryDTO = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = licenceCategory.Id, LicenceCategoryName = licenceCategory.Name, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredLicenceCategoryName = requiredLicenceCategory.Name, RequiredYears = 2 };
            var addedRequiredLicence = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requireToAdd = new RequiredLicenceCategoryPostDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.SetupSequence(repo => repo.GetRequirement(requireToAdd.LicenceCategoryId, requireToAdd.RequiredLicenceCategoryId))
                .ReturnsAsync((RequiredLicenceCategoryGetDTO)null)
                .ReturnsAsync(addedRequiredLicenceCategoryDTO);
            _requiredLicenceCategoryRepositoryMock.Setup(service => service.PostRequirement(requireToAdd)).ReturnsAsync(addedRequiredLicence);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.PostRequirement(requireToAdd);

            Assert.AreEqual(addedRequiredLicenceCategoryDTO, result);
        }
        
        [TestMethod]
        public async Task Post_Requirement_ThrowsValueMustBeGreaterThanZeroException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requireToAdd = new RequiredLicenceCategoryPostDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 0 };
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Post_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requireToAdd = new RequiredLicenceCategoryPostDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requireToAdd.LicenceCategoryId)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Post_Requirement_ThrowsNotFoundRequireLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requireToAdd = new RequiredLicenceCategoryPostDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requireToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requiredLicenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Post_Requirement_ThrowsRequirementAlreadyExistsException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO();
            var requireToAdd = new RequiredLicenceCategoryPostDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requireToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirement(requireToAdd.LicenceCategoryId, requireToAdd.RequiredLicenceCategoryId)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<RequirementAlreadyExistsException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Meet_Requirement_NoRequirementsReturnsTrue()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var customerLicences = new List<DrivingLicenceGetDTO>();
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var customerLicences = new List<DrivingLicenceGetDTO>();
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1)));
        }

        [TestMethod]
        public async Task Meet_Requirement_NoCustomerLicencesReturnsFalse()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO();
            var customerLicences = new List<DrivingLicenceGetDTO>();
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_CustomerHasRequirementReturnsTrue()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 1 };
            var customerDrivingLicence = new DrivingLicenceGetDTO { LicenceCategoryId = 1, ReceivedDate = new DateTime(2015, 1, 1) };
            var customerLicences = new List<DrivingLicenceGetDTO> { customerDrivingLicence };            
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_CustomerHasntRequirementReturnsFalse()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 1 };
            var customerDrivingLicence = new DrivingLicenceGetDTO { LicenceCategoryId = 3, ReceivedDate = new DateTime(2015, 1, 1) };
            var customerLicences = new List<DrivingLicenceGetDTO> { customerDrivingLicence };
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_CustomerHasRequirementToShortReturnsFalse()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 1 };
            var customerDrivingLicence = new DrivingLicenceGetDTO { LicenceCategoryId = 1, ReceivedDate = new DateTime(2019, 11, 1) };
            var customerLicences = new List<DrivingLicenceGetDTO> { customerDrivingLicence };
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsFalse(result);
        }
    }
}
