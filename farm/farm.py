import os
import asyncio
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodResponse
from interfaces.sensors import AReading
from connection_manager import ConnectionManager
from subsystems.plants.PlantController import PlantSystem
from subsystems.geoLocation.GeoLocation import GeoLocation
from subsystems.security.Security import Security


async def main():

    # Instantiate & connect the connection manager
    connection_manager = ConnectionManager()
    await connection_manager.connect()

    # Instantiate the subsystems
    plants: PlantSystem = PlantSystem()
    security: Security = Security()
    geolocation : GeoLocation = GeoLocation()

    # Get sensor readings and combine the lists received by just adding them and send by passing it into the send_readings method  
    # Loop
    while True:
        sensor_data : list[AReading] = plants.read_sensors() + security.read_sensors() + geolocation.read_sensors()
        await connection_manager.send_readings(sensor_data)
        await asyncio.sleep(connection_manager.telemetry_interval)
        

if __name__ == "__main__":
    asyncio.run(main())
