using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService.DTO;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;
using Stripe.Forwarding;

namespace Store.API.Controllers
{

    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger logger;
        private const string endpointSecret = "whsec_18ce84c955064de1b204429e7c12d18a48279dabe90bed10c35c05b3081d1596";
        public PaymentController(IPaymentService paymentService, ILogger logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
        {
            return Ok(await paymentService.CreateOrUpdatePaymentIntentForExistingOrder(input));
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
        {
            return Ok(await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketId));
        }



        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        //

        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                PaymentIntent paymentIntent;
                OrderResultDto order;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    logger.LogInformation("Order Updated To Payment Failed : ", order.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                    order = await paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    logger.LogInformation("Order Updated To Payment Succeeded : ", order.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }


    }
}
