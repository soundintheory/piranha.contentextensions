using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.Manager;
using SoundInTheory.Piranha.ContentExtensions.Areas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Controllers
{
    [Area("Manager")]
    [Route("manager/areas")]
    [Authorize(Policy = Permission.Admin)]
    public class AreasController : Controller
    {
        private readonly ContentAreaService _areas;

        public AreasController(ContentAreaService areas)
        {
            _areas = areas;
        }

        [HttpGet("edit/{areaType}")]
        public async Task<IActionResult> Edit(string areaType, Guid? languageId = null)
        {
            var area = await _areas.GetAreaAsync(areaType);

            if (area != null)
            {
                return Redirect($"/manager/content/edit/{areaType}/{area.Id}");
            }

            return Redirect("/manager/areas");
        }
    }
}
