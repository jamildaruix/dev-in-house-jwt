using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_in_house_basic_jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Financeiro")]
    public class CalculoController : ControllerBase
    {
        [HttpGet("{valorA}/{valorB}")]
        public IActionResult Get(int valorA, int valorB)
        {
            return Ok(valorA + valorB);
        }
    }
}
