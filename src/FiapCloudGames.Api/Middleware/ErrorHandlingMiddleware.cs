using FiapCloudGames.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Api.Middleware;

public sealed class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Erro de domínio");
            await Results.BadRequest(new { error = ex.Message }).ExecuteAsync(context);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Erro ao persistir dados");
            await Results.Conflict(new { error = "Não foi possível salvar os dados. Verifique duplicidades ou integridade." }).ExecuteAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado");
            await Results.Problem("Erro interno no servidor.", statusCode: StatusCodes.Status500InternalServerError).ExecuteAsync(context);
        }
    }
}
