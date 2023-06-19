using PrimeNumber.Core.Models;
using PrimeNumber.Data.Entity;
using PrimeNumber.Data.Repository;
using PrimeNumber.Data.UnitOfWork;

namespace PrimeNumber.Core.Services;

public class CalculationPrimeNumber : ICalculationPrimeNumber
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Input> _inputRepository;
    private readonly IGenericRepository<Result> _resultRepository;


    public CalculationPrimeNumber(IUnitOfWork unitOfWork, IGenericRepository<Input> inputRepository, IGenericRepository<Result> resultRepository)
    {
        _unitOfWork = unitOfWork;
        _inputRepository = inputRepository;
        _resultRepository = resultRepository;
    }

    public async Task<int> AddPrimeAsync(int number, Guid userId)
    {
        var maxPrimeNumber = 0;

        await _inputRepository.AddAsync(new Input
        {
            Number = number,
            CreatedDate = DateTime.Now,
            CreatedUser = userId
        });

        var user = await _resultRepository.GetAsync(x => x.CreatedUser == userId);

        if (IsPrime(number))
        {
            if (user != null)
            {
                maxPrimeNumber = user.MaxPrimeNumber;

                if (number > user.MaxPrimeNumber)
                {
                    maxPrimeNumber = number;
                    user.MaxPrimeNumber = number;

                    await _resultRepository.Update(user);
                }
            }
            else
            {
                maxPrimeNumber = number;
                await _resultRepository.AddAsync(new Result
                {
                    MaxPrimeNumber = number,
                    CreatedDate = DateTime.Now,
                    CreatedUser = userId
                });
            }
        }
        else if (user != null)
        {
            maxPrimeNumber = user.MaxPrimeNumber;
        }

        await _unitOfWork.SaveChangesAsync();

        return maxPrimeNumber;
    }

    public async Task<List<ResultResponseModel>> GetResultsAsync()
    {
        var results = await _resultRepository.GetAllAsync();
        var inputs = await _inputRepository.GetAllAsync();

        return results.Select(result =>
            new ResultResponseModel
            {
                UserId = result.CreatedUser,
                MaxPrime = result.MaxPrimeNumber,
                Inputs = inputs.Where(x => x.CreatedUser == result.CreatedUser).Select(x => x.Number).ToList()
            }
        ).ToList();

    }

    public async Task<int> Calculate(int number)
    {
        int maxPrime = CalculateMaxPrimeNumber(number);
        await _inputRepository.AddAsync(new Input
        {
            Number = number,
            CreatedDate = DateTime.Now,
            CreatedUser = Guid.NewGuid()
        });

        await _resultRepository.AddAsync(new Result
        {
            MaxPrimeNumber = maxPrime,
            CreatedDate = DateTime.Now,
            CreatedUser = Guid.NewGuid()
        });

        await _unitOfWork.SaveChangesAsync();
        return maxPrime;
    }

    private int CalculateMaxPrimeNumber(int number)
    {
        int maxPrime = -1;
        for (int i = 2; i <= number; i++)
        {
            if (IsPrime(i) && i > maxPrime)
            {
                maxPrime = i;
            }
        }
        return maxPrime;
    }

    private bool IsPrime(int number)
    {
        if (number < 2)
            return false;

        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }
}
