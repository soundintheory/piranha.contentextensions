using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piranha.Manager;
using Piranha.Manager.Localization;
using Piranha.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Controllers
{
    /// <summary>
    /// Api controller for getting menu related permissions.
    /// </summary>
    [Area("Manager")]
    [Route("manager/api/extensions/areas/permissions")]
    [Authorize(Policy = Permission.Admin)]
    [ApiController]
    [AutoValidateAntiforgeryToken]
    public class PermissionsApiController : Controller
    {
        private readonly IAuthorizationService _auth;

        public PermissionsApiController(IAuthorizationService auth)
        {
            _auth = auth;
        }

        /*
        [HttpGet]
        public async Task<ContentAreaPermissions> Get()
        {
            return null;
        }
        */
    }

}
