using EduOnline.DAL.Entities;
using EduOnline.DAL;
using EduOnline.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduOnline.Services
{
    public class DropDownListsHelper : IDropDownListsHelper
    {
        #region Dependencies & Properties
        private readonly DatabaseContext _context;

        public DropDownListsHelper(DatabaseContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync()
        {
            List<SelectListItem> listCategories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name, 
                    Value = c.Id.ToString(),                 
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCategories.Insert(0, new SelectListItem
            {
                Text = "Seleccione una categoría...",
                Value = Guid.Empty.ToString(), 
                Selected = true 
            });

            return listCategories;
        }
        
        public async Task<IEnumerable<SelectListItem>> GetDDLCountriesAsync()
        {
            List<SelectListItem> listCountries = await _context.Countries
                .Select(c => new SelectListItem
                {
                    Text = c.Name, 
                    Value = c.Id.ToString(),                 
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCountries.Insert(0, new SelectListItem
            {
                Text = "Seleccione un país...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listCountries;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLStatesAsync(Guid countryId)
        {
            List<SelectListItem> listStatesByCountryId = await _context.States
                .Where(s => s.Country.Id == countryId)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .OrderBy(s => s.Text)
                .ToListAsync();

            listStatesByCountryId.Insert(0, new SelectListItem
            {
                Text = "Seleccione un estado...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listStatesByCountryId;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCitiesAsync(Guid stateId)
        {
            List<SelectListItem> listCitiesByStateId = await _context.Cities
                .Where(c => c.State.Id == stateId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCitiesByStateId.Insert(0, new SelectListItem
            {
                Text = "Seleccione una ciudad...",
                Value = Guid.Empty.ToString(),
                Selected = true 
            });

            return listCitiesByStateId;
        }
        #endregion
    }
}