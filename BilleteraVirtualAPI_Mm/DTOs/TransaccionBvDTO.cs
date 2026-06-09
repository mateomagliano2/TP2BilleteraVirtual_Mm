using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilleteraVirtualAPI_Mm.DTOs
{
    public class TransaccionBvDTO
    {
     
        public string crypto_code { get; set; } = string.Empty;
        public int client_id { get; set; }       
        public string crypto_amount { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal money { get; set; }    
        
        public string action { get; set; } = string.Empty;
        public string datetime { get; set; } = string.Empty;
    }
}
