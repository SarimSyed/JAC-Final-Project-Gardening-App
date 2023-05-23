

class SecurityGPIOPins:
    DOOR_SENSOR :int = 5
    MOTION_SENSOR :int = 16
    DOOR_LOCK: int = 12

class PlantGPIOPins:
    LIQUIDLEVEL_SENSOR :int = 4
    SOILMOISTURE_SENSOR :int = 2
    #Pin cant be changed as it relies on the i2c bus of that port
    HUMIDITY_AND_TEMPERATURE_SENSOR = 26
    LED : int = 18
    FAN : int = 22



    
