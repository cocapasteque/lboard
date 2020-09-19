using System.Threading.Tasks;
using LBoard.Models.Security.ApiKey;

namespace LBoard.Services.Security.ApiKey
{
    public class ContextGetApiKeyQuery :IGetApiKeyQuery
    {
        public Task<LBoardApiKey> Execute(string apiKey)
        {
            throw new System.NotImplementedException();
        }
    }
}