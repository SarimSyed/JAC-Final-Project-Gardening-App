using ContainerFarm.Models.Actuators;
using ContainerFarm.Models.Sensors;
using System.ComponentModel;

/// Connected Tractors (Team #5)
/// Winter 2023 - April 28th
/// AppDev III
/// <summary>
/// This class stores information related to the plants inside a container. It stores the 
/// temperature and humidity of the container and water level and soil level of the plants. 
/// It also keeps track of the status of the fan and light inside the container.
/// </summary>
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
