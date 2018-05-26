using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.WebServiceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAnalysis
{
    public class CrmFunctionBase
    {
        public const string DefaultAuthparamValue = "/api/data/";
        public const string DefaultServiceUrlValue = "/xrmservices/2011/organization.svc/web?SdkClientVersion=8.2";

        public static async Task<IOrganizationService> AuthenticateAsAppUserAsync(ClientCredential clientCred, string organizationurl, string authparam = DefaultAuthparamValue, string serviceurl = DefaultServiceUrlValue)
        {
            return AuthenticateAsAppUser(organizationurl, await GetAuthResultAsync(clientCred, organizationurl), serviceurl);
        }
        public static IOrganizationService AuthenticateAsAppUser(string organizationurl, AuthenticationResult authorization, string serviceurl = DefaultServiceUrlValue)
        {
            // init the org service:
            var proxy = new OrganizationWebProxyClient(new Uri(organizationurl + (serviceurl ?? DefaultServiceUrlValue)), true);

            //set auth token as headertoken of service:
            proxy.HeaderToken = authorization.AccessToken;
            return proxy;
        }

        public static async Task<AuthenticationResult> GetAuthResultAsync(ClientCredential clientCred, string organizationurl, string authparam = DefaultAuthparamValue)
        {
            AuthenticationParameters ap = await AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(organizationurl + (authparam ?? DefaultAuthparamValue))).ConfigureAwait(false);
            var authenticationContext = new AuthenticationContext(ap.Authority, false);
            return await authenticationContext.AcquireTokenAsync(organizationurl, clientCred).ConfigureAwait(false);
        }
    }
}
