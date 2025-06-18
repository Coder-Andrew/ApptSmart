using ApptSmartBackend.Utilities;
using Microsoft.AspNetCore.Mvc;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Helpers;
using Microsoft.EntityFrameworkCore;
using ApptSmartBackend.Extensions;
using Microsoft.AspNetCore.Identity;
using ApptSmartBackend.Models;
using ApptSmartBackend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApptSmartBackend.DAL.Abstract;

namespace ApptSmartBackend.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;
        private readonly IUserHelperService _userHelper;
        private readonly UserManager<AuthUser> _userManager; // Maybe move this off to a service
        private readonly IUserInfoRepositoryAsync _userInfoRepository;
        // TODO: Refactor this. I don't feel like I should be using this many services in this controller
        public CompanyController(
            ICompanyService companyService,
            ILogger<CompanyController> logger,
            IUserHelperService userHelper,
            UserManager<AuthUser> userManager,
            IUserInfoRepositoryAsync userInfoRepository
            )
        {
            _companyService = companyService;
            _logger = logger;
            _userHelper = userHelper;
            _userManager = userManager;
            _userInfoRepository = userInfoRepository;
        }

        [HttpGet("{companySlug}/exists")]
        public async Task<IActionResult> CompanyExists(string companySlug)
        {
            return Ok(await _companyService.CompanyExists(companySlug));
        }

        [HttpGet("{companySlug}")]
        public async Task<IActionResult> GetCompanyInfo(string companySlug)
        {
            try
            {
                var company = await _companyService.GetCompanyAsync(companySlug);

                if (company == null)
                {
                    return NotFound("Company not found");
                }

                var dto = new CompanyDto
                {
                    CompanyName = company.CompanyName,
                    CompanyDescription = company.CompanyDescription ?? string.Empty
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing Get()");
                return StatusCode(500, ErrorMessages.Generic);
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            try
            {
                // This management might be better off in an orchestrator class
                ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);
                if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;

                var ownerId = userIdResponse.Value;

                var userInfo = await _userInfoRepository.FindByIdAsync(ownerId);

                if (userInfo == null || userInfo.Companies.ToList().Count != 0)
                {
                    return Forbid("User cannot have more than one company");
                }

                Company company = new Company
                {
                    OwnerId = ownerId,
                    CompanyName = dto.CompanyName,
                    CompanySlug = SlugHelper.Slugify(dto.CompanyName),
                    CompanyDescription = dto.CompanyDescription,
                };

                var aspUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (aspUserId == null) return NotFound("User not found");

                var user = await _userManager.FindByIdAsync(aspUserId);
                if (user == null) return NotFound("User not found");


                GenericResponse<Company> response = await _companyService.CreateCompanyAsync(company);

                if (!response.Success)
                {
                    return BadRequest();
                }

                await _userManager.AddToRolesAsync(user, ["CompanyOwner"]);

                return Created();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Failed to add company ${dto.CompanyName}. Duplicate company.");
                return StatusCode(500, ErrorMessages.Generic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add company.");
                return StatusCode(500, ErrorMessages.Generic);
            }
        }
    }
}
