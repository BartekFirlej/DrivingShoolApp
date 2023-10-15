using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("licencecategories")]
    public class LicenceCategoryController : ControllerBase
    {
        private readonly ILicenceCategoryService _licenceCategoryService;

        public LicenceCategoryController(ILicenceCategoryService licenceCategoryService)
        {
            _licenceCategoryService = licenceCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenceCategories()
        {
            ICollection<LicenceCategoryGetDTO> licenceCategories;
            try
            {
                licenceCategories = await _licenceCategoryService.GetLicenceCategories();
            }
            catch(NotFoundLicenceCategoriesException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(licenceCategories);
        }

        [HttpGet("{licenceCategoryId}")]
        public async Task<IActionResult> GetLicenceCategory(int licenceCategoryId)
        {
            LicenceCategoryGetDTO licenceCategory;
            try
            {
                licenceCategory = await _licenceCategoryService.GetLicenceCategory(licenceCategoryId);
            }
            catch(NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(licenceCategory);
        }

        [HttpPost]
        public async Task<IActionResult> AddLicenceCategory(LicenceCategoryPostDTO licenceCategoryDetails)
        {
            var addedLicenceCategory = await _licenceCategoryService.AddLicenceCategory(licenceCategoryDetails);
            return CreatedAtAction(nameof(AddLicenceCategory), addedLicenceCategory);
        }
    }
}