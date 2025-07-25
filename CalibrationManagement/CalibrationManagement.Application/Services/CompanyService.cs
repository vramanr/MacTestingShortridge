using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface ICompanyService
    {
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task<Company?> GetCompanyByIdAsync(Guid companyId);
        Task<Company?> GetCompanyByCoIdAsync(string coId);
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<IEnumerable<Company>> SearchCompaniesAsync(string searchTerm);
        Task<bool> DeleteCompanyAsync(Guid companyId);
        Task<Contact> AddContactAsync(Contact contact);
        Task<IEnumerable<Contact>> GetCompanyContactsAsync(string coId);
        Task<ModelNo> AddModelNumberAsync(ModelNo modelNo);
        Task<IEnumerable<ModelNo>> GetCompanyModelNumbersAsync(string coId);
    }

    public class CompanyService : ICompanyService
    {
        private readonly CalibrationDbContext _context;

        public CompanyService(CalibrationDbContext context)
        {
            _context = context;
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            company.CreatedDate = DateTime.UtcNow;
            company.ModifiedDate = DateTime.UtcNow;

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            company.ModifiedDate = DateTime.UtcNow;
            
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<Company?> GetCompanyByIdAsync(Guid companyId)
        {
            return await _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.ModelNumbers)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId && !c.Deleted);
        }

        public async Task<Company?> GetCompanyByCoIdAsync(string coId)
        {
            return await _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.ModelNumbers)
                .FirstOrDefaultAsync(c => c.CoId == coId && !c.Deleted);
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .Where(c => !c.Deleted && c.Active)
                .OrderBy(c => c.CoName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> SearchCompaniesAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            
            return await _context.Companies
                .Where(c => !c.Deleted && (
                    c.CoId.ToLower().Contains(term) ||
                    c.CoName!.ToLower().Contains(term) ||
                    c.City!.ToLower().Contains(term) ||
                    c.State!.ToLower().Contains(term)
                ))
                .OrderBy(c => c.CoName)
                .ToListAsync();
        }

        public async Task<bool> DeleteCompanyAsync(Guid companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
                return false;

            company.Deleted = true;
            company.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            contact.CreatedDate = DateTime.UtcNow;
            
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetCompanyContactsAsync(string coId)
        {
            return await _context.Contacts
                .Where(c => c.CoId == coId && !c.Deleted && c.Active)
                .OrderBy(c => c.ContactName)
                .ToListAsync();
        }

        public async Task<ModelNo> AddModelNumberAsync(ModelNo modelNo)
        {
            modelNo.CreatedDate = DateTime.UtcNow;
            
            _context.ModelNumbers.Add(modelNo);
            await _context.SaveChangesAsync();

            return modelNo;
        }

        public async Task<IEnumerable<ModelNo>> GetCompanyModelNumbersAsync(string coId)
        {
            return await _context.ModelNumbers
                .Where(m => m.CoId == coId && !m.Deleted && m.Active)
                .OrderBy(m => m.ModelNo)
                .ToListAsync();
        }
    }
}
