﻿using DrivingSchoolApp.Services;
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

        [HttpPost]
        public async Task<IActionResult> PostLicenceCategory(LicenceCategoryPostDTO licenceCategoryDetails)
        {
            var addedLicenceCategory = await _licenceCategoryService.PostLicenceCategory(licenceCategoryDetails);
            return CreatedAtAction(nameof(PostLicenceCategory), addedLicenceCategory);
        }
    }
}