﻿using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IDrivingLicenceService
    {
        public Task<PagedList<DrivingLicenceGetDTO>> GetDrivingLicences(int page, int size);
        public Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId);
        public Task<ICollection<DrivingLicence>> CheckCustomerDrivingLicences(int customerId, DateTime date);
        public Task<DrivingLicenceGetDTO> GetDrivingLicence(int id);
        public Task<DrivingLicenceResponseDTO> PostDrivingLicence(DrivingLicenceRequestDTO drivingLicenceDetails);
        public Task<DrivingLicence> DeleteDrivingLicence(int drivingLicenceId);
        public Task<DrivingLicence> CheckDrivingLicence(int drivingLicenceId);
        public Task<DrivingLicence> CheckDrivingLicenceTracking(int drivingLicenceId);
        public Task<DrivingLicenceResponseDTO> UpdateDrivingLicence(int drivingLicenceId, DrivingLicenceRequestDTO drivingLicenceUpdate);

    }
    public class DrivingLicenceService : IDrivingLicenceService
    {
        private readonly IDrivingLicenceRepository _drivingLicenceRepository;
        private readonly ICustomerService _customerService;
        private readonly ILicenceCategoryService _licenceCategoryService;
        private readonly IRequiredLicenceCategoryService _requiredLicenceCategoryService;
        private readonly IMapper _mapper;


        public DrivingLicenceService(IDrivingLicenceRepository drivingLicenceRepository, ICustomerService customerService, ILicenceCategoryService licenceCategoryService,
                                     IRequiredLicenceCategoryService requiredLicenceCategoryService, IMapper mapper)
        {
            _drivingLicenceRepository = drivingLicenceRepository;
            _customerService = customerService;
            _licenceCategoryService = licenceCategoryService;
            _requiredLicenceCategoryService = requiredLicenceCategoryService;
            _mapper = mapper;
        }

        public async Task<PagedList<DrivingLicenceGetDTO>> GetDrivingLicences(int page, int size)
        {
            var drivingLicences = await _drivingLicenceRepository.GetDrivingLicences(page, size);
            if(!drivingLicences.PagedItems.Any())
                throw new NotFoundDrivingLicenceException();
            return drivingLicences;
        }

        public async Task<ICollection<DrivingLicenceGetDTO>> GetCustomerDrivingLicences(int customerId)
        {
            var customer = await _customerService.CheckCustomer(customerId);
            var drivingLicences = await _drivingLicenceRepository.GetCustomerDrivingLicences(customerId);
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

        public async Task<DrivingLicenceResponseDTO> PostDrivingLicence(DrivingLicenceRequestDTO drivingLicenceDetails)
        {
            if (drivingLicenceDetails.ReceivedDate == DateTime.MinValue)
                throw new DateTimeException("Received");
            if (drivingLicenceDetails.ExpirationDate == DateTime.MinValue)
                throw new DateTimeException("Expiration");
            if (drivingLicenceDetails.ReceivedDate > drivingLicenceDetails.ExpirationDate)
                throw new DateTimeException("Received", "Expiration");
            await _customerService.CheckCustomer(drivingLicenceDetails.CustomerId);
            await _licenceCategoryService.CheckLicenceCategory(drivingLicenceDetails.LicenceCategoryId);
            var customerDrivingLicences  = await _drivingLicenceRepository.CheckCustomerDrivingLicences(drivingLicenceDetails.CustomerId, drivingLicenceDetails.ReceivedDate);
            var meetsRequirements = await _requiredLicenceCategoryService.MeetRequirements(customerDrivingLicences, drivingLicenceDetails.LicenceCategoryId, drivingLicenceDetails.ReceivedDate);
            if (!meetsRequirements)
                throw new CustomerDoesntMeetRequirementsException(drivingLicenceDetails.CustomerId, drivingLicenceDetails.LicenceCategoryId);
            var addedDrivingLicence = await _drivingLicenceRepository.PostDrivingLicence(drivingLicenceDetails);
            return _mapper.Map<DrivingLicenceResponseDTO>(addedDrivingLicence);
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

        public async Task<DrivingLicence> CheckDrivingLicenceTracking(int drivingLicenceId)
        {
            var drivingLicence = await _drivingLicenceRepository.CheckDrivingLicenceTracking(drivingLicenceId);
            if (drivingLicence == null)
                throw new NotFoundDrivingLicenceException(drivingLicenceId);
            return drivingLicence;
        }
        public async Task<ICollection<DrivingLicence>> CheckCustomerDrivingLicences(int customerId, DateTime date)
        {
            var customer = await _customerService.CheckCustomer(customerId);
            var drivingLicences = await _drivingLicenceRepository.CheckCustomerDrivingLicences(customerId, date);
            return drivingLicences;
        }

        public async Task<DrivingLicenceResponseDTO> UpdateDrivingLicence(int drivingLicenceId, DrivingLicenceRequestDTO drivingLicenceUpdate)
        {
            var drivingLicenceToUpdate = await CheckDrivingLicenceTracking(drivingLicenceId);
            if (drivingLicenceUpdate.ReceivedDate == DateTime.MinValue)
                throw new DateTimeException("Received");
            if (drivingLicenceUpdate.ExpirationDate == DateTime.MinValue)
                throw new DateTimeException("Expiration");
            if (drivingLicenceUpdate.ReceivedDate > drivingLicenceUpdate.ExpirationDate)
                throw new DateTimeException("Received", "Expiration");
            await _customerService.CheckCustomer(drivingLicenceUpdate.CustomerId);
            await _licenceCategoryService.CheckLicenceCategory(drivingLicenceUpdate.LicenceCategoryId);
            var customerDrivingLicences = await _drivingLicenceRepository.CheckCustomerDrivingLicences(drivingLicenceUpdate.CustomerId, drivingLicenceUpdate.ReceivedDate);
            var meetsRequirements = await _requiredLicenceCategoryService.MeetRequirements(customerDrivingLicences, drivingLicenceUpdate.LicenceCategoryId, drivingLicenceUpdate.ReceivedDate);
            if (!meetsRequirements)
                throw new CustomerDoesntMeetRequirementsException(drivingLicenceUpdate.CustomerId, drivingLicenceUpdate.LicenceCategoryId);
            var updatedDrivingLicence = await _drivingLicenceRepository.UpdateDrivingLicence(drivingLicenceToUpdate, drivingLicenceUpdate);
            return _mapper.Map<DrivingLicenceResponseDTO>(updatedDrivingLicence);
        }
    }
}
