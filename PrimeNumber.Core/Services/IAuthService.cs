using PrimeNumber.Core.Models;

namespace PrimeNumber.Core.Services;

public interface IAuthService
{
    Task<ResponseModel> Login(LoginRequest request, CancellationToken cancellationToken);
}
