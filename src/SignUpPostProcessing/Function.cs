using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using SignUpPostProcessing.Models;

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace SignUpPostProcessing
{
    public class Function
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider = new AmazonCognitoIdentityProviderClient();

        public async Task<APIGatewayProxyResponse> FunctionHandler(
            PostConfirmationRequest postConfirmationRequest,
            ILambdaContext context)
        {
            await _identityProvider.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                Username = postConfirmationRequest.UserName,
                UserPoolId = null,
                GroupName = null
            });

            return CreateResponse();
        }

        private APIGatewayProxyResponse CreateResponse()
        {
            var response = new APIGatewayProxyResponse
            {
                Body = string.Empty,
                StatusCode = (int)HttpStatusCode.OK,
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