using CalibrationManagement.Application.Services;
using CalibrationManagement.Application.DTOs;
using CalibrationManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CalibrationManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }

        [HttpGet("by-co-id/{coId}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyByCoId(string coId)
        {
            var company = await _companyService.GetCompanyByCoIdAsync(coId);
            if (company == null)
                return NotFound();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> SearchCompanies([FromQuery] string searchTerm)
        {
            var companies = await _companyService.SearchCompaniesAsync(searchTerm ?? "");
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CreateCompanyDto createCompanyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var company = _mapper.Map<Company>(createCompanyDto);
            var createdCompany = await _companyService.CreateCompanyAsync(company);
            var createdCompanyDto = _mapper.Map<CompanyDto>(createdCompany);
            
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.CompanyId }, createdCompanyDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDto>> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto updateCompanyDto)
        {
            if (id != updateCompanyDto.CompanyId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCompany = await _companyService.GetCompanyByIdAsync(id);
            if (existingCompany == null)
                return NotFound();

            var company = _mapper.Map<Company>(updateCompanyDto);
            var updatedCompany = await _companyService.UpdateCompanyAsync(company);
            var updatedCompanyDto = _mapper.Map<CompanyDto>(updatedCompany);
            
            return Ok(updatedCompanyDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{coId}/contacts")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetCompanyContacts(string coId)
        {
            var contacts = await _companyService.GetCompanyContactsAsync(coId);
            var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);
            return Ok(contactDtos);
        }

        [HttpPost("contacts")]
        public async Task<ActionResult<ContactDto>> AddContact([FromBody] CreateContactDto createContactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = _mapper.Map<Contact>(createContactDto);
            var createdContact = await _companyService.AddContactAsync(contact);
            var createdContactDto = _mapper.Map<ContactDto>(createdContact);
            
            return CreatedAtAction(nameof(GetCompanyContacts), new { coId = contact.CoId }, createdContactDto);
        }

        [HttpGet("{coId}/model-numbers")]
        public async Task<ActionResult<IEnumerable<ModelNoDto>>> GetCompanyModelNumbers(string coId)
        {
            var modelNumbers = await _companyService.GetCompanyModelNumbersAsync(coId);
            var modelNumberDtos = _mapper.Map<IEnumerable<ModelNoDto>>(modelNumbers);
            return Ok(modelNumberDtos);
        }

        [HttpPost("model-numbers")]
        public async Task<ActionResult<ModelNoDto>> AddModelNumber([FromBody] CreateModelNoDto createModelNoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modelNo = _mapper.Map<ModelNo>(createModelNoDto);
            var createdModelNo = await _companyService.AddModelNumberAsync(modelNo);
            var createdModelNoDto = _mapper.Map<ModelNoDto>(createdModelNo);
            
            return CreatedAtAction(nameof(GetCompanyModelNumbers), new { coId = modelNo.CoId }, createdModelNoDto);
        }
    }
}
