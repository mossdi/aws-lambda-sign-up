using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SignUpPostProcessing.Models;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SignUpPostProcessing
{
    public class Function
    {
        private readonly HttpClient _client;
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        
        public Function()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://e6723941f951.ngrok.io") };
            _identityProvider = new AmazonCognitoIdentityProviderClient();
        }
        
        public async Task FunctionHandler(
            PostConfirmationRequest postConfirmationRequest, 
            ILambdaContext context)
        {
            var user = new User
            {
                Id = Guid.Parse(postConfirmationRequest.Request.UserAttributes.Sub)
            };
            
            await _identityProvider.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                Username = postConfirmationRequest.UserName,
                UserPoolId = "us-east-2_Fwz3k0ZqX",
                GroupName = "Clients",
            });
            
            await _client.PostAsJsonAsync("api/Users", user);
        }
    }
}
