from time import sleep
from serial import Serial 
import pynmea2
from geopy.geocoders import Nominatim

from interfaces.sensors import ISensor, AReading

"""
geopy: This is a Python client for several popular geocoding web services. It easy for Python developers to locate the coordinates of addresses, cities, countries, and landmarks across the globe using third-party geocoders and other data sources.
"""


"""
I used this resource as a guide to connect & read the GPS (Air530) data with the Seeed reTerminal: 
https://github.com/microsoft/IoT-For-Beginners/blob/main/3-transport/lessons/1-location-tracking/pi-gps-sensor.md

I used this resource as a guide to understand how to decode the GPS data into latitude and longitude coordinates:
https://github.com/microsoft/IoT-For-Beginners/blob/main/3-transport/lessons/1-location-tracking/single-board-computer-gps-decode.md

I used this a resource to convert the coordinates into an actual address:
https://www.geeksforgeeks.org/get-the-city-state-and-country-names-from-latitude-and-longitude-using-python/
"""
class GPSLocation(ISensor):
    """A GPS to track an address location in the Geo-Location subsytem.

    Args:
        ISensor (ISensor): Implements the interface.
    """

    VALID_MESSAGE_TYPE = 'GGA'

    # gpio: int, 
    def __init__(self, model: str, type: AReading.Type):
        """Constructor for GPSLocation  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. GPS (Air530)
        :param ReadingType type: Type of reading this sensor produces. Ex. 'GPSLOCATION'
        """

        # Initialize object variables
        self._serial = Serial('/dev/ttyAMA0', 9600, timeout=5)
        self._geolocator = Nominatim(user_agent="geo-location")

        self._sensor_model = model or "GPS (Air530)"
        self.reading_type = type or AReading.Type.GPSLOCATION 

        self._serial.reset_input_buffer()
        self._serial.flush()     

    def read_sensor(self) -> AReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """

        SATELLITE_LOCK_COUNT = 4

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

                        # Ensure the GPS is locked to a minimum of 'x' satellites
                        if gps_data.num_sats != SATELLITE_LOCK_COUNT:
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

                            print(full_address_location)

                            # Return a new reading
                            return AReading(AReading.Type.GPSLOCATION,
                                        AReading.Unit.LOCATION, 
                                        {
                                            'value': f'Address: {full_address_location}'
                                        })

                    # Read the data line from the GPS
                    data_line = self._serial.readline().decode('utf-8')

            except UnicodeDecodeError:
                data_line = self._serial.readline().decode('utf-8')
            except pynmea2.nmea.ParseError:
                data_line = self._serial.readline().decode('utf-8')


if __name__ == "__main__":

    gps_location = GPSLocation("GPS (Air530)", AReading.Type.GPSLOCATION)

    while True:
        reading = gps_location.read_sensor()
        print(f"Address: {reading}")
        sleep(1)