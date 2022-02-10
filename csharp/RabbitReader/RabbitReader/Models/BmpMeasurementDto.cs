namespace RabbitReader.Models
{
    internal class BmpMeasurementDto
    {
        public DateTime DateTime { get; set; }
        public int Pressure { get; set; }
        public int Temperature { get; set; }
    }
}