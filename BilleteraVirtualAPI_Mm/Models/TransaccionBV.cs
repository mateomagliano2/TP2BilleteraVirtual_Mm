using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilleteraVirtualAPI_Mm.Models
{
    public class TransaccionBV
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string crypto_code { get; set; } = string.Empty;

        public int client_id { get; set; }

        [Required]
        public string crypto_amount { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal money { get; set; } 

        [Required]
        public string action { get; set; } = string.Empty;  // "purchase" o "sale"

        [Required]
        public DateTime datetime { get; set; }
    }
}

