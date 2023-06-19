using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrimeNumber.Core.Helper;

public static class TokenManagerService
{
    public const string TokenSecretKey = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed2";
    public const string AuthenticationHeaderName = "Token";
    public const string UserId = "UserId";
    public const string UserName = "UserName";
    public const string Roles = "Roles";
    public const string LoginDate = "LoginDate";
    public const string ExpireDate = "ExpireDate";

    public static string CreateToken(Dictionary<string, object> keyValues)
    {
        DateTime issuedAt = DateTime.UtcNow;
        DateTime expires = DateTime.UtcNow.AddYears(1);
        var tokenHandler = new JwtSecurityTokenHandler();
        ClaimsIdentity claimsIdentity = new();
        foreach (var key in keyValues)
        {
            claimsIdentity.AddClaim(new Claim(key.Key, key.Value.ToString()));
        }
        const string secret = TokenSecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var token = tokenHandler.CreateJwtSecurityToken(
                    issuer: "https://localhost:7181",
                    audience: "https://localhost:7181",
                    subject: claimsIdentity,
                    notBefore: issuedAt,
                    expires: expires,
                    signingCredentials: signingCredentials);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;

    }


    public static UserLoginInfo GetUserInfo(HttpContextAccessor _httpContextAccessor)
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Headers[AuthenticationHeaderName].ToString();
        if (string.IsNullOrEmpty(token))
            return new UserLoginInfo();

        return UserLoginInfoResponseModel(token);
    }

    public static UserLoginInfo GetUserInfo(string token)
    {
        if (string.IsNullOrEmpty(token))
            return new UserLoginInfo();

        return UserLoginInfoResponseModel(token);
    }

    private static UserLoginInfo UserLoginInfoResponseModel(string token)
    {
        JwtSecurityTokenHandler handler = new();
        var tokenDecodeJwt = handler.ReadJwtToken(token);
        var response = new UserLoginInfo()
        {
            Token = token,
            UserId = Guid.Parse(tokenDecodeJwt.Claims.First(a => a.Type == UserId).Value),
            UserName = tokenDecodeJwt.Claims.First(a => a.Type == UserName).Value,
            Roles = tokenDecodeJwt.Claims.First(a => a.Type == Roles).Value,
            LoginDate = DateTime.Parse(tokenDecodeJwt.Claims.First(a => a.Type == LoginDate).Value),
            ExpireDate = DateTime.Parse(tokenDecodeJwt.Claims.First(a => a.Type == ExpireDate).Value)
        };
        return response;
    }

    public static UserLoginInfo GetCurrentUser(HttpRequest request)
    {
        var authcookie = request.Cookies["Auth"];
        if (authcookie is null)
            return new UserLoginInfo();

        return GetUserInfo(authcookie);
    }
}


public class UserLoginInfo
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Roles { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime LoginDate { get; set; }
    public DateTime ExpireDate { get; set; }
}