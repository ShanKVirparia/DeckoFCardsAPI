using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DeckofCardsAPI.Models;

namespace DeckofCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckOFCardsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Card>> GetCards()
        {
            using (var client = new HttpClient())
            {
                // Make a call to the API to generate a new deck
                var deckUrl = "https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1";
                var response = await client.GetAsync(deckUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic deck = JsonConvert.DeserializeObject(content);
                    string deckId = deck.deck_id;

                    // Draw 5 cards from the deck
                    var drawUrl = $"https://deckofcardsapi.com/api/deck/{deckId}/draw/?count=5";
                    response = await client.GetAsync(drawUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        dynamic cards = JsonConvert.DeserializeObject(content);
                        var result = new List<Card>();
                        foreach (var card in cards.cards)
                        {
                            result.Add(new Card
                            {
                                Value = card.value,
                                Suit = card.suit,
                                Image = card.image
                            });
                        }
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
