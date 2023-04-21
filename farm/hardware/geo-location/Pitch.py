import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from time import sleep
import math
import asyncio
from serial import Serial 
import pynmea2
from geopy.geocoders import Nominatim

from sensors import ISensor, AReading

class Pitch(ISensor):
    """The pitch of the reTerminal accelerometer in the Geo-Location subsytem.

    Args:
        ISensor (ISensor): Implements the interface.
    """

    VALID_MESSAGE_TYPE = 'GGA'

    def __init__(self, model: str, type: AReading.Type):
        """Constructor for Pitch  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. GPS (Air530)
        :param ReadingType type: Type of reading this sensor produces. Ex. 'PITCH'
        """

        # Initialize object variables
        self._serial = Serial('/dev/ttyAMA0', 9600, timeout=5)
        self._geolocator = Nominatim(user_agent="geo-location")

        self._sensor_model = model or "GPS (Air530)"
        self.reading_type = type or AReading.Type.PITCH 

        self._serial.reset_input_buffer()
        self._serial.flush()     

    def read_sensor(self) -> list[AReading]:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """

        data_line = ''
        full_address_location = ''

        while True:
            try:
                # Read the data line from the GPS
                data_line = self._serial.readline().decode('utf-8')

                while len(data_line) > 0:
                    # Parses the data line from the UART serial port into gpsdata
                    gps_data = pynmea2.parse(data_line)

                    # If the parsed data is a position fix message, and isprocessed
                    if gps_data.sentence_type == Pitch.VALID_MESSAGE_TYPE:
                        # raise Exception(
                        #     f"The sentence type does not match the correcttype ({GPSLocation.VALID_MESSAGE_TYPE}):{gps_data.sentence_type}")

                        # Get the latitude value from the gps data
                        latitude = pynmea2.dm_to_sd(gps_data.lat)
                        # Get the longitude value from the gps data
                        longitude = pynmea2.dm_to_sd(gps_data.lon)

                        # Correct invalid latitude value
                        if gps_data.lat_dir == 'S':
                            latitude = latitude * -1
                        # Correct invalid longitude value
                        if gps_data.lon_dir == 'W':
                            longitude = longitude * -1

                        # Print the coordinates with the number of satellites
                        print(
                            f'{latitude},{longitude} - from {gps_data.num_sats} satellites')

                        # Get the full address location
                        full_address_location = self._geolocator.reverse(
                            f"{latitude}, {longitude}")

                        # print(full_address_location)

                        # Return a new reading
                        return [
                            AReading(AReading.Type.GPSLOCATION,
                                    AReading.Unit.LOCATION, str(full_address_location))
                        ]

                    # Read the data line from the GPS
                    data_line = self._serial.readline().decode('utf-8')

            except UnicodeDecodeError:
                data_line = self._serial.readline().decode('utf-8')
                
            return [
                    AReading(AReading.Type.GPSLOCATION,
                            AReading.Unit.LOCATION, str(full_address_location))
                ]



x_values = []
y_values = []
z_values = []

while True:
    async def accel_coroutine(device):
        async for event in device.async_read_loop():
            accelEvent = rt_accel.AccelerationEvent(event)

            if accelEvent.name != None:
                print(f"name={str(accelEvent.name)} value={accelEvent.value}")

                if accelEvent.name == rt_accel.AccelerationName.X:
                    x_values.append(accelEvent.value)
                elif accelEvent.name == rt_accel.AccelerationName.Y:
                    y_values.append(accelEvent.value)
                elif accelEvent.name == rt_accel.AccelerationName.Z:
                    z_values.append(accelEvent.value)

                if len(x_values) >= 1 and len(y_values) >= 1 and len(z_values) >= 1:
                    ax = x_values.pop()
                    ay = y_values.pop()
                    az = z_values.pop()

                    pitch = math.atan2(-ax, math.sqrt(ay*ay + az*az))
                    roll_angle = math.atan2(ay, az)

                    print(f"Pitch: {pitch}")
                    print(f"Roll angle: {roll_angle}")

                    x_values.clear()
                    y_values.clear()
                    z_values.clear()

    accel_device = rt.get_acceleration_device()

    asyncio.ensure_future(accel_coroutine(accel_device))

    loop = asyncio.get_event_loop()
    loop.run_forever()