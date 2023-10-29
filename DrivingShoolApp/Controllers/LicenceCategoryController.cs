using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Exceptions;
using System.Collections.Generic;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("licencecategories")]
    public class LicenceCategoryController : ControllerBase
    {
        private readonly ILicenceCategoryService _licenceCategoryService;
        private readonly IRequiredLicenceCategoryService _requirededLicenceCategoryService;

        public LicenceCategoryController(ILicenceCategoryService licenceCategoryService, IRequiredLicenceCategoryService requirededLicenceCategoryService)
        {
            _licenceCategoryService = licenceCategoryService;
            _requirededLicenceCategoryService = requirededLicenceCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenceCategories()
        {
            ICollection<LicenceCategoryGetDTO> licenceCategories;
            try
            {
                licenceCategories = await _licenceCategoryService.GetLicenceCategories();
            }
            catch(NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(licenceCategories);
        }

        [HttpGet("{licencecategoryid}")]
        public async Task<IActionResult> GetLicenceCategory(int licencecategoryid)
        {
            LicenceCategoryGetDTO licenceCategory;
            try
            {
                licenceCategory = await _licenceCategoryService.GetLicenceCategory(licencecategoryid);
            }
            catch(NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(licenceCategory);
        }

        [HttpGet("{licencecategoryid}/requirements")]
        public async Task<IActionResult> GetLicenceCategoryRequirements(int licencecategoryid)
        {
            ICollection<RequiredLicenceCategoryGetDTO> requiredLicences;
            try
            {
                requiredLicences = await _requirededLicenceCategoryService.GetRequirements(licencecategoryid);
            }
            catch (NotFoundRequiredLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(requiredLicences);
        }

        [HttpPost]
        public async Task<IActionResult> PostLicenceCategory(LicenceCategoryPostDTO licenceCategoryDetails)
        {
            var addedLicenceCategory = await _licenceCategoryService.PostLicenceCategory(licenceCategoryDetails);
            return CreatedAtAction(nameof(PostLicenceCategory), addedLicenceCategory);
        }
    }
}