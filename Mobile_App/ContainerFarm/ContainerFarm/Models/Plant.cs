using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System.ComponentModel;

namespace ContainerFarm.Models
{
    class Plant : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TemperatureSensor temperature;
        private HumiditySensor humidity;
        private SoilMoistureSensor soilMoisture;
        private WaterLevelSensor waterLevel;
        private LightActuator lightActuator;

        public Plant()
        {
            temperature = new TemperatureSensor();
            humidity = new HumiditySensor();
            soilMoisture = new SoilMoistureSensor();
            waterLevel = new WaterLevelSensor();
            lightActuator = new LightActuator();
        }

        public TemperatureSensor Temperature { get { return temperature; } }
        public HumiditySensor Humidity { get { return humidity; } }
        public SoilMoistureSensor SoilMoisture { get { return soilMoisture; } }
        public WaterLevelSensor WaterLevel { get { return waterLevel; } }
        public LightActuator LightActuator { get { return lightActuator; } }


    }
}
