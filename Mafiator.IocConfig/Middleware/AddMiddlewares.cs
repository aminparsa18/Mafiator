using Mafiator.IocConfig.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Mafiator.IocConfig.Middleware
{
    public static class AddMiddlewareExtentions
    {
        public static void AddCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMainMiddlewares();
            //var rewriteOptions = new RewriteOptions();
            //rewriteOptions.Rules.Add(new NonWwwRewriteRule());
            //app.UseRewriter(rewriteOptions);

        }
    }
}
