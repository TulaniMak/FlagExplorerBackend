using FlagExplorerBackend.DTOs;
using FlagExplorerBackend.Models;
using FlagExplorerBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlagExplorerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(ICountryService countryService, ILogger<CountriesController> logger)
    {
        _countryService = countryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
    {
        try
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all countries");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<CountryDetailsDto>> GetCountryDetails(string name)
    {
        try
        {
            var country = await _countryService.GetCountryDetailsAsync(name);
            if (country == null)
                return NotFound($"Country '{name}' not found");
            return Ok(country);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Country not found: {name}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching details for country: {name}");
            return StatusCode(500, "Internal server error");
        }

    }

}
