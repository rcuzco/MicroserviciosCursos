namespace MicroPayments.Model
{
    public sealed class PaymentRequest
    {
        public int EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
