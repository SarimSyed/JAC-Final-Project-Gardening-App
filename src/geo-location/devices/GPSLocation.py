import time
import serial
import pynmea2
from geopy.geocoders import Nominatim

serial = serial.Serial('/dev/ttyAMA0', 9600, timeout=1)
serial.reset_input_buffer()
serial.flush()

def print_gps_data():
    msg = pynmea2.parse(line)

    if msg.sentence_type == 'GGA':
        lat = pynmea2.dm_to_sd(msg.lat)
        lon = pynmea2.dm_to_sd(msg.lon)

        if msg.lat_dir == 'S':
            lat = lat * -1

        if msg.lon_dir == 'W':
            lon = lon * -1

        print(f'{lat},{lon} - from {msg.num_sats} satellites')

        geolocator = Nominatim(user_agent="my_app") 
        location = geolocator.reverse(f"{lat}, {lon}") 
        # address = location.raw['address']
        print(location)

        # print(location)

while True:
    try:
        line = serial.readline().decode('utf-8')

        while len(line) > 0:
            print_gps_data()

            # print(f"lat:{lat}, long:{long}")

            line = serial.readline().decode('utf-8')

            # time.sleep(1)

    except UnicodeDecodeError:
        line = serial.readline().decode('utf-8')

    time.sleep(1)