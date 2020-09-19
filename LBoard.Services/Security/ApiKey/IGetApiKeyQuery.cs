using System.Threading.Tasks;
using LBoard.Models.Security.ApiKey;

namespace LBoard.Services.Security.ApiKey
{
    public interface IGetApiKeyQuery
    {
        Task<LBoardApiKey> Execute(string apiKey);
    }
}