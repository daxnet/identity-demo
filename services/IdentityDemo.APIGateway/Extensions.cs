using Ocelot.Authentication.Middleware;
using Ocelot.Authorisation.Middleware;
using Ocelot.Cache.Middleware;
using Ocelot.Claims.Middleware;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Errors.Middleware;
using Ocelot.Headers.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using Ocelot.QueryStrings.Middleware;
using Ocelot.RateLimit.Middleware;
using Ocelot.Request.Middleware;
using Ocelot.Requester.Middleware;
using Ocelot.RequestId.Middleware;
using Ocelot.Responder.Middleware;
using Ocelot.Security.Middleware;
using Ocelot.WebSockets.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.APIGateway
{
    public static class Extensions
    {
        private static void UseIfNotNull(this IOcelotPipelineBuilder builder,
            Func<DownstreamContext, Func<Task>, Task> middleware)
        {
            if (middleware != null)
            {
                builder.Use(middleware);
            }
        }

        public static IOcelotPipelineBuilder BuildCustomOcelotPipeline(this IOcelotPipelineBuilder builder,
            OcelotPipelineConfiguration pipelineConfiguration)
        {
            builder.UseExceptionHandlerMiddleware();
            builder.MapWhen(context => context.HttpContext.WebSockets.IsWebSocketRequest,
                app =>
                {
                    app.UseDownstreamRouteFinderMiddleware();
                    app.UseDownstreamRequestInitialiser();
                    app.UseLoadBalancingMiddleware();
                    app.UseDownstreamUrlCreatorMiddleware();
                    app.UseWebSocketsProxyMiddleware();
                });
            builder.UseIfNotNull(pipelineConfiguration.PreErrorResponderMiddleware);
            builder.UseResponderMiddleware();
            builder.UseDownstreamRouteFinderMiddleware();
            builder.UseSecurityMiddleware();
            if (pipelineConfiguration.MapWhenOcelotPipeline != null)
            {
                foreach (var pipeline in pipelineConfiguration.MapWhenOcelotPipeline)
                {
                    builder.MapWhen(pipeline);
                }
            }
            builder.UseHttpHeadersTransformationMiddleware();
            builder.UseDownstreamRequestInitialiser();
            builder.UseRateLimiting();

            builder.UseRequestIdMiddleware();
            builder.UseIfNotNull(pipelineConfiguration.PreAuthenticationMiddleware);
            if (pipelineConfiguration.AuthenticationMiddleware == null)
            {
                builder.UseAuthenticationMiddleware();
            }
            else
            {
                builder.Use(pipelineConfiguration.AuthenticationMiddleware);
            }
            builder.UseClaimsToClaimsMiddleware();
            builder.UseIfNotNull(pipelineConfiguration.PreAuthorisationMiddleware);
            if (pipelineConfiguration.AuthorisationMiddleware == null)
            {
                builder.UseAuthorisationMiddleware();
            }
            else
            {
                builder.Use(pipelineConfiguration.AuthorisationMiddleware);
            }
            builder.UseClaimsToHeadersMiddleware();
            builder.UseIfNotNull(pipelineConfiguration.PreQueryStringBuilderMiddleware);
            builder.UseClaimsToQueryStringMiddleware();
            builder.UseLoadBalancingMiddleware();
            builder.UseDownstreamUrlCreatorMiddleware();
            builder.UseOutputCacheMiddleware();
            builder.UseHttpRequesterMiddleware();

            return builder;
        }
    }
}
