using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Security.Claims;

namespace dev_in_house_basic_jwt.Middleware
{
    /// <summary>
    /// ExampleAuthorizationMiddlewareResultHandler
    /// </summary>
    public class ExampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler authorizationMiddlewareResultHandler = new();

        public async Task HandleAsync(RequestDelegate next,
                                      HttpContext context,
                                      AuthorizationPolicy policy,
                                      PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden && authorizeResult.AuthorizationFailure!
                                                            .FailedRequirements
                                                            .OfType<Show404Requirment>()
                                                            .Any())
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                System.Diagnostics.Debug.WriteLine("[AuthorizationMiddlewareResultHandler] COM ERRO");
            }

            if (context.User!.Claims!.ToList().Any())
            {
                System.Diagnostics.Debug.WriteLine("[ClaimsNaRequest] #### ROTINA PRA LISTAR CLAIMS RECEBIDA NO CONTEXT ####");
                System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] Retornar o status da autorização {authorizeResult.Succeeded}");

                foreach (var claim in context.User.Claims.ToList())
                {
                    if (claim.Type == "id")
                    {
                        System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] ID Recebido no Request {claim.Value}");
                    }
                    else if (claim.Type == ClaimTypes.Country)
                    {
                        System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] Claim do tipo País {claim.Value}");
                    }
                    else if (claim.Type == ClaimTypes.Role)
                    {
                        System.Diagnostics.Debug.WriteLine($"[ClaimsNaRequest] Claim do tipo Role {claim.Value}");
                    }
                }
            }

            await authorizationMiddlewareResultHandler.HandleAsync(next, context, policy, authorizeResult);
        }


        public class Show404Requirment : IAuthorizationRequirement { }
    }
}
