namespace BilleteraVirtualAPI_Mm.DTOs
{
    public class EditarTransaccionBvDTO
    {
        public string? crypto_code {  get; set; }
        public string? crypto_amount { get; set; }  
        public int? client_id { get; set; }
        public decimal? money { get; set; }
        public string? action { get; set; }
        public string? datetime   { get; set; }

    }
}
