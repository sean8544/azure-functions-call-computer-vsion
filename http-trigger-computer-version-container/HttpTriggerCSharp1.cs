using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;


using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace Company.Function
{
    public static class HttpTriggerCSharp1
    {
        [FunctionName("HttpTriggerCSharp1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            //see details about blobTrigger, name:
            //https://docs.microsoft.com/zh-cn/azure/azure-functions/functions-bindings-storage-blob-trigger?tabs=csharp&WT.mc_id=AZ-MVP-5003757#metadata 

           log.LogInformation("C# HTTP trigger function processed a request.");

            try{

            
            string FilePath = req.Query["filepath"];

            string subscriptionKey = Environment.GetEnvironmentVariable("ComputerVisionSubscriptionKey");
            string endpoint = Environment.GetEnvironmentVariable("ComputerVisionEndpoint");
          

            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);


            string ANALYZE_URL_IMAGE = FilePath;

             log.LogInformation($"Picture url:{ANALYZE_URL_IMAGE}");
           

              List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
                {
                    VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                
                };

            ImageAnalysis results = await client.AnalyzeImageAsync(ANALYZE_URL_IMAGE, features,null,"zh");

            // Sunmarizes the image content.
            log.LogInformation("Summary:");
            string Summary="";
            foreach (var caption in results.Description.Captions)
            {
                Summary+=($"识别结果：{caption.Text} \n置信度： {caption.Confidence}");
            }
            
            return new OkObjectResult( JsonConvert.SerializeObject(Summary) );
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ex.Message));
            }
                        
        }

            

                /*
                * AUTHENTICATE
                * Creates a Computer Vision client used by each example.
                */
                public static ComputerVisionClient Authenticate(string endpoint, string key)
                {
                    ComputerVisionClient client =
                    new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                    { Endpoint = endpoint };
                    return client;
                }



    }
}
