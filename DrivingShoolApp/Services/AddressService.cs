﻿using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using System.Text.RegularExpressions;

namespace DrivingSchoolApp.Services
{
    public interface IAddressService
    {
        public Task<ICollection<AddressGetDTO>> GetAddresses();
        public Task<AddressGetDTO> GetAddress(int addressId);
        public Task<AddressGetDTO> PostAddress(AddressPostDTO addressDetails);
    }
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<ICollection<AddressGetDTO>> GetAddresses()
        {
            var addresses = await _addressRepository.GetAddresses();
            if (!addresses.Any())
                throw new NotFoundAddressException();
            return addresses;
        }

        public async Task<AddressGetDTO> GetAddress(int addressId)
        {
            var address = await _addressRepository.GetAddress(addressId);
            if (address == null)
                throw new NotFoundAddressException(addressId);
            return address;
        }

        public async Task<AddressGetDTO> PostAddress(AddressPostDTO addressDetails)
        {
            string postalCodePattern = @"^\d{2}-\d{3}$";
            if (!Regex.IsMatch(addressDetails.PostalCode, postalCodePattern))
                throw new WrongPostalCodeFormatException(addressDetails.PostalCode);
            if (addressDetails.Number <= 0)
                throw new ValueMustBeGreaterThanZeroException("number");
            var addedAddress = await _addressRepository.PostAddress(addressDetails);
            return await _addressRepository.GetAddress(addedAddress.Id);
        }
    }
}
