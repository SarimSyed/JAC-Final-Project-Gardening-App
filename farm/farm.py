import asyncio
from interfaces.sensors import AReading
from connection_manager import ConnectionManager
from subsystems.subsystem_controller import SubsystemController



async def main():

    subsystems : SubsystemController = SubsystemController()

    # Instantiate & connect the connection manager
    connection_manager = ConnectionManager(subsystems)
    await connection_manager.connect()
  
    # Loop
    while True:
        sensor_data : list[AReading] = subsystems.read_sensors()
        #print(sensor_data)
        await connection_manager.send_readings(sensor_data)
        await asyncio.sleep(connection_manager.telemetry_interval)
        

if __name__ == "__main__":
    asyncio.run(main())