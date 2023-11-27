using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using System.Text.RegularExpressions;

namespace DrivingSchoolApp.Services
{
    public interface IAddressService
    {
        public Task<PagedList<AddressGetDTO>> GetAddresses(int page, int size);
        public Task<AddressGetDTO> GetAddress(int addressId);
        public Task<AddressGetDTO> PostAddress(AddressPostDTO addressDetails);
        public Task<Address> CheckAddress(int addressId);
        public Task<Address> DeleteAddress(int addressId);
        public Task<AddressGetDTO> UpdateAddress(int addressId, AddressPostDTO addressUpdate);
    }
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<PagedList<AddressGetDTO>> GetAddresses(int page, int size)
        {
            var addresses = await _addressRepository.GetAddresses(page, size);
            if (!addresses.PagedItems.Any())
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

        public async Task<Address> CheckAddress(int addressId)
        {
            var address = await _addressRepository.CheckAddress(addressId);
            if (address == null)
                throw new NotFoundAddressException(addressId);
            return address;
        }

        public async Task<Address> DeleteAddress(int addressId)
        {
            var addressToDelete = await CheckAddress(addressId);
            return await _addressRepository.DeleteAddress(addressToDelete);
        }

        public async Task<AddressGetDTO> UpdateAddress(int addressId, AddressPostDTO addressUpdate)
        {
            await CheckAddress(addressId);
            await _addressRepository.UpdateAddress(addressId, addressUpdate);
            return await _addressRepository.GetAddress(addressId);
        }
    }
}
