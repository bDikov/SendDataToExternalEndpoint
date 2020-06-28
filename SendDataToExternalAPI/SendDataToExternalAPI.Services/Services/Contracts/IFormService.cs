using SendDataToExternalAPI.Services.ModelsDTO;
using System.Threading.Tasks;

namespace SendDataToExternalAPI.Services.Services.Contracts
{
    public interface IFormService
    {
        Task<bool> SendForm(FormDTO data);
    }
}
