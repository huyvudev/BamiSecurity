using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.Common;
using CR.InfrastructureBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;

namespace CR.Core.ApplicationServices.AuthenticationModule.Implements;

public class ManagerTokenService : CoreServiceBase, IManagerTokenService
{
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IOpenIddictAuthorizationManager _openIddictAuthorizationManager;

    public ManagerTokenService(
        ILogger<ManagerTokenService> logger,
        IHttpContextAccessor httpContext,
        IOpenIddictTokenManager tokenManager,
        IOpenIddictAuthorizationManager openIddictAuthorizationManager
    )
        : base(logger, httpContext)
    {
        _tokenManager = tokenManager;
        _openIddictAuthorizationManager = openIddictAuthorizationManager;
    }

    public async Task RevokeOtherToken()
    {
        var subject = _httpContext.GetCurrentSubject();
        var authorizationId = _httpContext.GetCurrentAuthorizationId();

        var tokenByAuthorizationId = _tokenManager.FindByAuthorizationIdAsync(authorizationId);
        List<string?> currentTokenIds = new();
        await foreach (var token in tokenByAuthorizationId)
        {
            if (token is OpenIddictEntityFrameworkCoreToken openIdDictToken)
            {
                currentTokenIds.Add(openIdDictToken.Id);
            }
        }

        var tokens = _tokenManager.FindBySubjectAsync(subject);
        await foreach (var token in tokens)
        {
            if (
                token is OpenIddictEntityFrameworkCoreToken openIdDictToken
                && openIdDictToken.Status == OpenIddictConstants.Statuses.Valid
                && !currentTokenIds.Contains(openIdDictToken.Id)
            )
            {
                await _tokenManager.TryRevokeAsync(token);
            }
        }
    }

    public async Task RevokeAllTokenBySubject()
    {
        var subject = _httpContext.GetCurrentSubject();
        //remove authorizations
        var authorizations = _openIddictAuthorizationManager.FindBySubjectAsync(subject);
        await foreach (var item in authorizations)
        {
            if (
                item is OpenIddictEntityFrameworkCoreAuthorization authorization
                && authorization.Status == OpenIddictConstants.Statuses.Valid
            )
            {
                await _openIddictAuthorizationManager.TryRevokeAsync(item);
                //remove token
                var tokens = _tokenManager.FindByAuthorizationIdAsync(authorization.Id!);
                await foreach (var token in tokens)
                {
                    if (
                        token is OpenIddictEntityFrameworkCoreToken openIdDictToken
                        && openIdDictToken.Status == OpenIddictConstants.Statuses.Valid
                    )
                    {
                        await _tokenManager.TryRevokeAsync(token);
                    }
                }
            }
        }
    }

    public async Task RevokeAllToken()
    {
        var authorizationId = _httpContext.GetCurrentAuthorizationId();
        var tokens = _tokenManager.FindByAuthorizationIdAsync(authorizationId);

        await foreach (var token in tokens)
        {
            if (
                token is OpenIddictEntityFrameworkCoreToken openIdDictToken
                && openIdDictToken.Status == OpenIddictConstants.Statuses.Valid
            )
            {
                await _tokenManager.TryRevokeAsync(token);
            }
        }
    }
}
