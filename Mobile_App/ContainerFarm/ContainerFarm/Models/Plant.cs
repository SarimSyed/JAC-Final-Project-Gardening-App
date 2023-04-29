using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System.ComponentModel;

namespace ContainerFarm.Models
{
    public class Plant : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TemperatureSensor temperature;
        private HumiditySensor humidity;
        private SoilMoistureSensor soilMoisture;
        private WaterLevelSensor waterLevel;
        private LightActuator lightActuator;
        private FanActuator fanActuator;

        public Plant()
        {
            temperature = new TemperatureSensor();
            humidity = new HumiditySensor();
            soilMoisture = new SoilMoistureSensor();
            waterLevel = new WaterLevelSensor();
            lightActuator = new LightActuator();
            fanActuator = new FanActuator();
        }


        public TemperatureSensor Temperature { get { return temperature; } set { temperature = value; } }
        public HumiditySensor Humidity { get { return humidity; } set { humidity = value; } }
        public SoilMoistureSensor SoilMoisture { get { return soilMoisture; } set { soilMoisture = value; } }
        public WaterLevelSensor WaterLevel { get { return waterLevel; } set { waterLevel = value; } }
        public LightActuator LightActuator { get { return lightActuator; } set { lightActuator= value; } }
        public FanActuator FanActuator { get { return fanActuator; } set {  fanActuator= value; } }

    }
}
