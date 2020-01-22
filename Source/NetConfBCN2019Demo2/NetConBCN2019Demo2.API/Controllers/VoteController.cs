using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetConBCN2019Demo2.API.Services;
using NetConfBCN2019Demo2.CrossCutting.Models;

namespace NetConBCN2019Demo2.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class VoteController : ControllerBase
    {
        private readonly ISendToEventHubService _sendToEventHubService;

        const string EhEntityPathVoteRaw = "voteraw";
        const string EhEntityPathVotingResults = "votingresults";

        public VoteController(ISendToEventHubService sendToEventHubService)
        {
            _sendToEventHubService = sendToEventHubService;
        }
           
        [HttpPost]
        [Route("newVote")]
        public async Task<IActionResult> PostNewVote([FromBody] VoteRawModel voteModelRaw)
        {
            if (ModelState.IsValid)
            {
                if (voteModelRaw != null)
                {
                    try
                    {                                             
                        await _sendToEventHubService.SendMessageToEventHub(EhEntityPathVoteRaw, voteModelRaw);

                        //TODO Insertar y recuperar valores
                        var votingResults = new VotingResultsModel
                            {
                                PositiveVote = new Random().Next(10,20),
                                NegativeVote = new Random().Next(1, 9)
                        };
                        await _sendToEventHubService.SendMessageToEventHub(EhEntityPathVotingResults, votingResults);
                       
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        //Log.Info($"Internal Server Error - {ex.Message}");
                        return StatusCode(500);
                    }
                }

                return BadRequest();
            }

            return BadRequest();
        }
    }
}