namespace PatientInfoPortal.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DiseaseName { get; set; }
        public bool Epilepsy { get; set; }
        public List<PatientNCD> PatientNCDs { get; set; } = new();
        public List<PatientAllergy> PatientAllergies { get; set; } = new();
    }
}
