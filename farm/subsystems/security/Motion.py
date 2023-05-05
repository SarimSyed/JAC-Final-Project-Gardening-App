from grove import grove_mini_pir_motion_sensor
from time import sleep
from interfaces.sensors import ISensor, AReading


class Motion(ISensor):
   """ Detects any movement in the container. This motion sensor is used in the security subsystem.
   Args:
      ISensor (_type_): Implements the ISensor interface.
   """

   def __init__(self, gpio: int, type: AReading.Type = AReading.Type.MOTION, model: str = "PIR Motion Sensor"):
      """Constructor for the motion sensor class. Defines the interface's properties. 
      :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
      :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
      """

      self._sensor_model = model
      self.reading_type = type

      #Inizialize sensor
      self.motion_sensor = grove_mini_pir_motion_sensor.GroveMiniPIRMotionSensor(gpio)

   def read_sensor(self) -> AReading:
      """Takes a reading from the motion sensor
      :return list[AReading]: List of readings measured by the sensor. Most sensors return a list with a single item.
      """

      # Get the sensor reading and returns value
      if (self.motion_sensor.read() == 0):
         return AReading(AReading.Type.MOTION,  AReading.Unit.NONE, {"value": AReading.Response.NOT_DETECTED.value})
      else:
         return AReading(AReading.Type.MOTION,  AReading.Unit.NONE, {"value": AReading.Response.DETECTED.value})

if __name__ == "__main__":
   motion = Motion(16)
   while True:
      print(motion.read_sensor())
      sleep(1)