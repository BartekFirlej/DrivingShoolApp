using AutoMapper;
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
        public Task<AddressResponseDTO> PostAddress(AddressRequestDTO addressDetails);
        public Task<Address> CheckAddress(int addressId);
        public Task<Address> CheckAddressTracking(int addressId);
        public Task<Address> DeleteAddress(int addressId);
        public Task<AddressResponseDTO> UpdateAddress(int addressId, AddressRequestDTO addressUpdate);
    }
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
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

        public async Task<AddressResponseDTO> PostAddress(AddressRequestDTO addressDetails)
        {
            string postalCodePattern = @"^\d{2}-\d{3}$";
            if (!Regex.IsMatch(addressDetails.PostalCode, postalCodePattern))
                throw new WrongPostalCodeFormatException(addressDetails.PostalCode);
            if (addressDetails.Number <= 0)
                throw new ValueMustBeGreaterThanZeroException("number");
            var addedAddress = await _addressRepository.PostAddress(addressDetails);
            return _mapper.Map<AddressResponseDTO>(addedAddress);
        }

        public async Task<Address> CheckAddress(int addressId)
        {
            var address = await _addressRepository.CheckAddress(addressId);
            if (address == null)
                throw new NotFoundAddressException(addressId);
            return address;
        }

        public async Task<Address> CheckAddressTracking(int addressId)
        {
            var address = await _addressRepository.CheckAddressTracking(addressId);
            if (address == null)
                throw new NotFoundAddressException(addressId);
            return address;
        }

        public async Task<Address> DeleteAddress(int addressId)
        {
            var addressToDelete = await CheckAddress(addressId);
            return await _addressRepository.DeleteAddress(addressToDelete);
        }

        public async Task<AddressResponseDTO> UpdateAddress(int addressId, AddressRequestDTO addressUpdate)
        {
            var addressToUpdate = await CheckAddressTracking(addressId);
            await _addressRepository.UpdateAddress(addressToUpdate, addressUpdate);
            return _mapper.Map<AddressResponseDTO>(addressToUpdate);
        }
    }
}
