using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;

namespace DocumentAnalysis
{
    public static class GetAccountNames
    {
        [FunctionName("GetAccountNames")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                string AppUserId = ConfigurationManager.AppSettings["ClientId"];
                string AppUserSecret = ConfigurationManager.AppSettings["ClientSecret"];
                
                var orgService = await CrmFunctionBase.AuthenticateAsAppUserAsync(new ClientCredential(AppUserId, AppUserSecret), "https://crm549233.crm.dynamics.com");

                log.Info("Conection successfull.");
                QueryExpression accountQuery = new QueryExpression
                {
                    EntityName = "account",
                    ColumnSet = new ColumnSet("name")
                };
                var accounts = orgService.RetrieveMultiple(accountQuery).Entities.Select(v=>new { name = v.GetAttributeValue<String>("name"), Id = v.Id.ToString() });
                log.Info("Accounts successfully retrieved.");

                var jsonToReturn = JsonConvert.SerializeObject(accounts);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
                };
            }
            catch (Exception exc)
            {
                log.Error($"Failed to connect: {exc.Message}");
               
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(exc.Message, Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
