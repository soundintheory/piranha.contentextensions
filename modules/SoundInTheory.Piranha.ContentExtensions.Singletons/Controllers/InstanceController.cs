using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piranha.Manager;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Controllers
{
    [Area("Manager")]
    [Route("manager/instance")]
    [Authorize(Policy = Permission.Admin)]
    public class InstanceController : Controller
    {
        private readonly SingletonService _singletons;

        public InstanceController(SingletonService singletons)
        {
            _singletons = singletons;
        }

        [HttpGet("page/{typeId}")]
        public async Task<IActionResult> Page(string typeId)
        {
            var instance = await _singletons.GetPageAsync(typeId);

            if (instance != null)
            {
                return Redirect($"/manager/page/edit/{instance.Id}");
            }

            return Redirect("/manager/pages");
        }

        [HttpGet("content/{typeId}")]
        public async Task<IActionResult> Content(string typeId)
        {
            var area = await _singletons.GetContentAsync(typeId);

            if (area != null)
            {
                return Redirect($"/manager/content/edit/{typeId}/{area.Id}");
            }

            return Redirect("/manager/areas");
        }
    }
}
