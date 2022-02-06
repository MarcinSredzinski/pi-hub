import Adafruit_GPIO.SPI as SPI
import Adafruit_BMP.BMP085 as BMP085


class BMPSensor:
    def read_preassure_and_temp(self): 
        sensor = BMP085.BMP085()
        measurement = BMPMeasurement()
        measurement.temperature = sensor.read_temperature()
        measurement.pressure = sensor.read_pressure()
        print('Temp = {0:0.2f} *C'.format(sensor.read_temperature()))
        print('Pressure = {0:0.2f} Pa'.format(sensor.read_pressure()))
        return measurement


class BMPMeasurement:
    pressure = 0
    temperature = 0

