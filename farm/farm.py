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

    connection_manager = ConnectionManager()
    await connection_manager.connect()
    #Get sensor readings and combine the lists you get by just adding them and send by passing it into the send_readings method
    plants: PlantSystem = PlantSystem()
    security: Security = Security()
    geolocation : GeoLocation = GeoLocation()

    sensor_data : list[AReading] = plants.read_sensors() + security.read_sensors() + geolocation.read_sensors()

    

    while True:
        # print(connection_manager.telemetry_interval)
        await connection_manager.send_readings(sensor_data)
        await asyncio.sleep(connection_manager.telemetry_interval)

    # # Define behavior for halting the application
    # def stdin_listener():
    #     while True:
    #         selection = input("Press Q to quit\n")
    #         if selection == "Q" or selection == "q":
    #             print("Quitting...")
    #             break

    # # Run the stdin listener in the event loop
    # loop = asyncio.get_running_loop()
    # user_finished = loop.run_in_executor(None, stdin_listener)

    # # Wait for user to indicate they are done listening for method calls
    # await user_finished

    # Finally, shut down the client
    # await connection_manager._client.shutdown()


if __name__ == "__main__":
    asyncio.run(main())

    # If using Python 3.6 use the following code instead of asyncio.run(main()):
    # loop = asyncio.get_event_loop()
    # loop.run_until_complete(main())
    # loop.close()
