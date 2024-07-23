using MicroPayments.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroPayments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PaymentsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public ActionResult<PaymentResult> ProcessPayment(PaymentRequest paymentRequest)
        {
            if (paymentRequest is null)
            {
                return BadRequest();
            }
            // Aquí se integraría con una pasarela de pagos real.
            return Ok(new PaymentResult { Status = "Payment Successful" });
        }
    }
}
