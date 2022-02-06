import pika
import time

import Adafruit_GPIO.SPI as SPI
import Adafruit_SSD1306

import RPi.GPIO as GPIO
import time

from PIL import Image
from PIL import ImageDraw
from PIL import ImageFont

import subprocess

RST = None

GPIO.setmode(GPIO.BCM)
blue = 22  #11
green = 27  #12
yellow = 18  #13
red = 17  #15

disp = Adafruit_SSD1306.SSD1306_128_32(rst=RST)
# create image with mode '1' for 1-bit color.
width = disp.width
height = disp.height
image = Image.new('1', (width, height))
# Get drawing object to draw on image.
draw = ImageDraw.Draw(image)
# Load default font.
font = ImageFont.load_default()


def setup_led():
    GPIO.setup(blue, GPIO.OUT)
    GPIO.setup(green, GPIO.OUT)
    GPIO.setup(yellow, GPIO.OUT)
    GPIO.setup(red, GPIO.OUT)
    GPIO.output(blue, GPIO.LOW)
    GPIO.output(green, GPIO.LOW)
    GPIO.output(yellow, GPIO.LOW)
    GPIO.output(red, GPIO.LOW)


def blink(led):
    print("going to blink on %r" % led)
    setup_led()
    for i in range(10):
        GPIO.output(led, GPIO.HIGH)
        time.sleep(.1)
        GPIO.output(led, GPIO.LOW)
        time.sleep(.1)


def clean_display():
    disp.begin()
    disp.clear()
    disp.display()
    # Create blank image for drawing.
    # Draw a black filled box to clear the image.
    draw.rectangle((0, 0, width, height), outline=0, fill=0)
    padding = -2
    top = padding
    bottom = height - padding


def display_line(top, text):
    x = 0
    draw.text((x, top), text, font=font, fill=255)
    disp.image(image)
    disp.display()


def main():
    setup_led()
    clean_display()
    queue_name = 'hello'
    host_name = 'localhost'
    connection = pika.BlockingConnection(pika.ConnectionParameters(host_name))
    channel = connection.channel()
    channel.queue_declare(queue=queue_name, durable=True)
    channel.basic_consume(queue=queue_name,
                          auto_ack=True,
                          on_message_callback=callback)
    channel.start_consuming()


def callback(ch, method, properties, body):
    print("[x] Received %r" % body)
    clean_display()
    #disp.clear()
    #disp.display()
    txt = body.decode().split('|')
    top = 0
    zero_index = True
    for i in txt:
        if zero_index:
            blink(int(i))
            zero_index = False
        else:
            display_line(top, i)
            top += 8
            if top > 23:
                top = 0


if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        print('Interrupted')
        try:
            sys.exit(0)
        except SystemExit:
            os._exit(0)

