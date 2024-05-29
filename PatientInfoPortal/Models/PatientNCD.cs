namespace PatientInfoPortal.Models
{
    public class PatientNCD
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string NCD { get; set; }
    }
}
