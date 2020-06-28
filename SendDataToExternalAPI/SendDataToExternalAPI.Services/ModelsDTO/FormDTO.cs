using System.ComponentModel.DataAnnotations;

namespace SendDataToExternalAPI.Services.ModelsDTO
{
    public class FormDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }
    }
}
