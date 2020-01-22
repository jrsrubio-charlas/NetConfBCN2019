using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace NetConfBCN2019Demo2.PublishToSignalR
{
    public static class Function1
    {
        [FunctionName("negotiatevoteraw")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "voteraw")] SignalRConnectionInfo connectionInfo)
        {

            return connectionInfo;
        }
        
        
        [FunctionName("voteraw")]
        public static async Task SendVoteRawMessage(
            [EventHubTrigger("voteraw", Connection = "EhConnectionString")] EventData[] events,
            [SignalR(HubName = "voteraw")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    await signalRMessages.AddAsync(
                        new SignalRMessage
                        {
                            //Para enviar el mensaje a un grupo determinado
                            //GroupName = "groupName",
                            //Para enviar el mensaje a un usuario determinado
                            //UserId = "userId",
                            //Para enviar el mensaje a una connectionId determinada
                            //ConnectionId = "connectionId",                            

                            Target = "newVoteRaw",
                            Arguments = new[] { messageBody }
                        });

                    Console.WriteLine(messageBody);

                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }




        //*****************************************************************************************************************
        //*****************************************************************************************************************
        //*****************************************************************************************************************
        //*****************************************************************************************************************
        //*****************************************************************************************************************
        //*****************************************************************************************************************

        [FunctionName("negotiatevotingresults")]
        public static SignalRConnectionInfo GetSignalRInfo2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "votingresults")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }


        [FunctionName("votingresults")]
        public static async Task SendVotingResultsMessage(
            [EventHubTrigger("votingresults", Connection = "EhConnectionString")] EventData[] events,
            [SignalR(HubName = "votingresults")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    await signalRMessages.AddAsync(
                        new SignalRMessage
                        {
                            //Para enviar el mensaje a un grupo determinado
                            //GroupName = "groupName",
                            //Para enviar el mensaje a un usuario determinado
                            //UserId = "userId",
                            //Para enviar el mensaje a una connectionId determinada
                            //ConnectionId = "connectionId",

                            Target = "VotingResults",
                            Arguments = new[] { messageBody }
                        });

                    Console.WriteLine(messageBody);

                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
