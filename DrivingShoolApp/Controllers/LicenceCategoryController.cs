﻿using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Exceptions;
using System.Collections.Generic;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetLicenceCategories([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<LicenceCategoryGetDTO> licenceCategories;
            try
            {
                licenceCategories = await _licenceCategoryService.GetLicenceCategories(page, size);
            }
            catch(NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
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

        [HttpDelete("{licencecategoryid}")]
        public async Task<IActionResult> DeleteLicenceCategory(int licencecategoryid)
        {
            LicenceCategory deleted;
            try
            {
                deleted = await _licenceCategoryService.DeleteLicenceCategory(licencecategoryid);
            }
            catch (NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This licence category refers to something." } });
            }
            catch (DbUpdateException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something is wrong with your request or database." } });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something gone wrong." } });
            }
            return NoContent();
        }
    }
}