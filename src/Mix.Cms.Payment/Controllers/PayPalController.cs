using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Services;
using Mix.Cms.Payment.Domain.Dtos;
using Mix.Cms.Payment.Domain.Models;
using Mix.Cms.Payment.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Payment.Controllers
{
    [Route("api/paypal")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly PayPalService _paypalService;
        public PayPalController(PayPalService paypalService)
        {
            _paypalService = paypalService;
        }

        [HttpGet("token")]
        public async Task<ActionResult> GetAccessTokenAsync()
        {
            try
            {
                var token = await _paypalService.GetAccessTokenAsync();
                return Ok(token);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
