﻿using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLicenceService
    {
        public Task<PagedList<DrivingLicenceGetDTO>> GetDrivingLicences(int page, int size);
        public Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId, DateTime date);
        public Task<DrivingLicenceGetDTO> GetDrivingLicence(int id);
        public Task<DrivingLicenceGetDTO> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails);
        public Task<DrivingLicence> DeleteDrivingLicence(int drivingLicenceId);
        public Task<DrivingLicence> CheckDrivingLicence(int drivingLicenceId);

    }
    public class DrivingLicenceService : IDrivingLicenceService
    {
        private readonly IDrivingLicenceRepository _drivingLicenceRepository;
        private readonly ICustomerService _customerService;
        private readonly ILicenceCategoryService _licenceCategoryService;
        private readonly IRequiredLicenceCategoryService _requiredLicenceCategoryService;


        public DrivingLicenceService(IDrivingLicenceRepository drivingLicenceRepository,
                                     ICustomerService customerService,
                                     ILicenceCategoryService licenceCategoryService,
                                     IRequiredLicenceCategoryService requiredLicenceCategoryService)
        {
            _drivingLicenceRepository = drivingLicenceRepository;
            _customerService = customerService;
            _licenceCategoryService = licenceCategoryService;
            _requiredLicenceCategoryService = requiredLicenceCategoryService;
        }

        public async Task<PagedList<DrivingLicenceGetDTO>> GetDrivingLicences(int page, int size)
        {
            var drivingLicences = await _drivingLicenceRepository.GetDrivingLicences(page, size);
            if(!drivingLicences.PagedItems.Any())
                throw new NotFoundDrivingLicenceException();
            return drivingLicences;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId, DateTime date)
        {
            var drivingLicences = await _drivingLicenceRepository.GetCustomerDrivingLicences(customerId, date);
            if (!drivingLicences.Any())
                throw new NotFoundDrivingLicenceException();
            return drivingLicences;
        }

        public async Task<DrivingLicenceGetDTO> GetDrivingLicence(int id)
        {
            var drivingLicence = await _drivingLicenceRepository.GetDrivingLicence(id);
            if(drivingLicence == null)
                throw new NotFoundDrivingLicenceException(id);
            return drivingLicence;
        }

        public async Task<DrivingLicenceGetDTO> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails)
        {
            if (drivingLicenceDetails.ReceivedDate == DateTime.MinValue)
                throw new DateTimeException("Received");
            if (drivingLicenceDetails.ExpirationDate == DateTime.MinValue)
                throw new DateTimeException("Expiration");
            if (drivingLicenceDetails.ReceivedDate > drivingLicenceDetails.ExpirationDate)
                throw new DateTimeException("Received", "Expiration");
            var customer = await _customerService.GetCustomer(drivingLicenceDetails.CustomerId);
            var licenceCategory = await _licenceCategoryService.GetLicenceCategory(drivingLicenceDetails.LicenceCategoryId);
            var customerDrivingLicences  = await _drivingLicenceRepository.GetCustomerDrivingLicences(drivingLicenceDetails.CustomerId, drivingLicenceDetails.ReceivedDate);
            var meetsRequirements = await _requiredLicenceCategoryService.MeetRequirements(customerDrivingLicences, drivingLicenceDetails.LicenceCategoryId, drivingLicenceDetails.ReceivedDate);
            if (!meetsRequirements)
                throw new CustomerDoesntMeetRequirementsException(drivingLicenceDetails.CustomerId, drivingLicenceDetails.LicenceCategoryId);
            var addedDrivingLicence = await _drivingLicenceRepository.PostDrivingLicence(drivingLicenceDetails);
            return await _drivingLicenceRepository.GetDrivingLicence(addedDrivingLicence.Id);
        }

        public async Task<DrivingLicence> DeleteDrivingLicence(int drivingLicenceId)
        {
            var drivingLicenceToDelete = await CheckDrivingLicence(drivingLicenceId);
            return await _drivingLicenceRepository.DeleteDrivingLicence(drivingLicenceToDelete);
        }

        public async Task<DrivingLicence> CheckDrivingLicence(int drivingLicenceId)
        {
            var drivingLicence = await _drivingLicenceRepository.CheckDrivingLicence(drivingLicenceId);
            if (drivingLicence == null)
                throw new NotFoundDrivingLicenceException(drivingLicenceId);
            return drivingLicence;
        }
    }
}
