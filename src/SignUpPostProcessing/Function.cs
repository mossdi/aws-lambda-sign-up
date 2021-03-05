using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using SignUpPostProcessing.Models;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SignUpPostProcessing
{
    public class Function
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        
        public Function()
        {
            _identityProvider = new AmazonCognitoIdentityProviderClient();
        }
        
        public async Task<APIGatewayProxyResponse> FunctionHandler(
            PostConfirmationRequest postConfirmationRequest, 
            ILambdaContext context)
        {
            await _identityProvider.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                Username = postConfirmationRequest.UserName,
                UserPoolId = null,
                GroupName = null,
            });
            
            return CreateResponse();
        }
        
        private APIGatewayProxyResponse CreateResponse()
        {
            var response = new APIGatewayProxyResponse
            {
                Body = string.Empty,
                StatusCode = (int) HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                }
            };
    
            return response;
        }
    }
}
