using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;
using user_bff.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_bff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardTokenController : ControllerBase
    {
        private readonly ICardTokenService _CardTokenService;
        private readonly IStripeService _stripeService;
        public CardTokenController(ICardTokenService CardTokenService, IStripeService stripeService)
        {
            _CardTokenService = CardTokenService;
            _stripeService = stripeService;
        }

        /// <summary>
        /// Get saved card by user id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>Card token entity</returns>
        // GET api/<CardTokenController>/5
        [HttpGet("{userId}")]
        public ActionResult Get(Guid userId)
        {
            try
            {
                var savedCard = new List<SavedCard>();
                var CardTokens = _CardTokenService.GetCardTokens(userId);
                foreach (var token in CardTokens)
                {
                    savedCard.Add(new SavedCard()
                    {
                        Id = token.CardTokenId,
                        Brand = token.Brand,
                        ExpMonth = token.ExpMonth,
                        ExpYear = token.ExpYear,
                        Last4 = token.Last4Digit,
                        Type = token.Type,
                    });
                }

                return Ok(new
                {
                    data = savedCard,
                    message = "Successfully returned card token list."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Use: Save new card into system
        /// </summary>
        /// <param name="card"></param>
        /// <returns>SavedCard</returns>
        [HttpPost]
        public ActionResult Post([FromBody] Card card)
        {
            try
            {
                var token = _stripeService.CreateStripeToken(card);
                var customer = _stripeService.CreateStripeCustomer(new PayModel()
                {
                    CardNumder = card.CardNumder,
                    EmailId = card.EmailId,
                    CVC = card.CVC,
                    Month = card.Month,
                    Year = card.Year,
                    SenderCognitoId = card.SenderCognitoId,
                }, token);

                var cardToken = _CardTokenService.Create(new CardToken()
                {
                    CognitoUserId = card.SenderCognitoId,
                    CustomerId = customer.Id,
                    Brand = token.Card.Brand,
                    ExpMonth = token.Card.ExpMonth,
                    ExpYear=token.Card.ExpYear,
                    Last4Digit=token.Card.Last4,
                    Type=token.Type
                });
                return Ok(new
                {
                    data = new SavedCard()
                    {
                        Id = cardToken.CardTokenId,
                        Brand = token.Card.Brand,
                        ExpMonth = token.Card.ExpMonth,
                        ExpYear = token.Card.ExpYear,
                        Last4 = token.Card.Last4,
                        Type = token.Card.Last4,

                    },
                    message = "Successfully saved a new card."
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// delete card token from system
        /// </summary>
        /// <param name="id">card token id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _CardTokenService.Remove(id);
                return Ok(new
                {
                    message = "Successfully removed card token"
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}