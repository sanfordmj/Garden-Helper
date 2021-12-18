

namespace GardenHelper.Models
{
    public class SensorReading
    {
        public int IX_SensorReading { get; set; }

        public float Value { get; set; }

        public DateTime? EnteredDate { get; set; }

        public int IX_Sensor { get; set; }

    }
}