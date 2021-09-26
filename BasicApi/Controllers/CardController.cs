using BasicApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BasicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TokenAuth]
    public class CardController : ControllerBase
    {
        private readonly ICardFactoryService _cardFactoryService;
        private readonly ILogger<WeatherForecastController> _logger;

        public CardController(ICardFactoryService cardFactoryService
                            , ILogger<WeatherForecastController> logger)
        {
            _cardFactoryService = cardFactoryService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Card> Get()
        {
            return _cardFactoryService.GetAll();
        }

        [HttpPost]
        public IActionResult Post(Card card)
        {
            try
            {
                card.Author = GetAuthName();
                var result = _cardFactoryService.Post(card);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult Put(Card card)
        {
            var author = GetAuthName();
            try
            {
                var currentdata = _cardFactoryService.GetAll().First(_ => _.Id == card.Id);
                if (currentdata.Author != author)
                {
                    ModelState.AddModelError("Permission denied", "You are not owner of card.");

                    return BadRequest(ModelState);
                }

                currentdata.Name = card.Name;
                currentdata.Status = card.Status;
                currentdata.Content = card.Content;
                currentdata.Category = card.Category;

                var result = _cardFactoryService.Put(currentdata);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public IActionResult Delete([Required,FromQuery] int id)
        {
            try
            {
                _cardFactoryService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        private string GetAuthName()
        {
            var token = HttpContext.Request.Headers.First(_ => _.Key == "Authorization").Value.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var data = tokenHandler.ReadJwtToken(token.Replace("Bearer ", string.Empty));
            return data.Claims.First(_ => _.Type == "id").Value;
        }
    }
}
