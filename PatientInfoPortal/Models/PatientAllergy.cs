namespace PatientInfoPortal.Models
{
    public class PatientAllergy
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Allergy { get; set; }
    }
}
