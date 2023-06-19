using PrimeNumber.Core.Models;
using PrimeNumber.Data.Entity;

namespace PrimeNumber.Core.Services;

public interface ICalculationPrimeNumber
{
    Task<int> Calculate(int number);
    Task<int> AddPrimeAsync(int number, Guid userId);
    Task<List<ResultResponseModel>> GetResultsAsync();
}
