import time
from serial import Serial
import pynmea2
from geopy.geocoders import Nominatim
from sensors import ISensor, AReading

"""
I used this resource as a guide to connect & read the GPS (Air530) data with the Seeed reTerminal: 
https://github.com/microsoft/IoT-For-Beginners/blob/main/3-transport/lessons/1-location-tracking/pi-gps-sensor.md

I used this resource as a guide to understand how to decode the GPS data into latitude and longitude coordinates:
https://github.com/microsoft/IoT-For-Beginners/blob/main/3-transport/lessons/1-location-tracking/single-board-computer-gps-decode.md

I used this a resource to convert the coordinates into an actual address:
https://www.geeksforgeeks.org/get-the-city-state-and-country-names-from-latitude-and-longitude-using-python/
"""
class GPSLocation(ISensor):
    """Humidity class for a humidity sensor.
    :args ISensor (Interface): Implements the ISensor interface.
    """

    VALID_MESSAGE_TYPE = 'GGA'

    # gpio: int, 
    def __init__(self, model: str, type: AReading.Type):
        """Constructor for Sensor  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

        # Initialize object variables
        self._serial = Serial('/dev/ttyAMA0', 9600, timeout=5)
        self._geolocator = Nominatim(user_agent="geo-location")
        self._sensor_model = model or "GPS (Air530)"
        self.reading_type = type or AReading.Type.GPSLOCATION 

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
                    if gps_data.sentence_type == GPSLocation.VALID_MESSAGE_TYPE:
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
                            f'{latitude},{longitude} - from {gps_data.num_sats}satellites')

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
        


if __name__ == "__main__":

    gps_location = GPSLocation("GPS (Air530)", AReading.Type.GPSLOCATION)

    while True:
        readings = gps_location.read_sensor()

        for reading in readings:
            print(f"Address: {reading}")
            time.sleep(1)

        # try:
        #     line = serial.readline().decode('utf-8')

        #     while len(line) > 0:
        #         print_gps_data()

        #         # print(f"lat:{lat}, long:{long}")

        #         line = serial.readline().decode('utf-8')

        #         # time.sleep(1)

        # except UnicodeDecodeError:
        #     line = serial.readline().decode('utf-8')

        # time.sleep(1)
            