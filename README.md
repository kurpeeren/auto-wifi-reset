# WiFi Control Software for Hardware

This software is designed to control the network connectivity of a WiFi hardware device. It continuously monitors the network to ensure that the device has obtained an IP address. If the device fails to acquire an IP address, the software will automatically disable the hardware and restart it.

## How It Works

The software operates in the background, constantly checking the status of the WiFi connection. When the device successfully acquires an IP address, it remains active and continues monitoring the network. However, in cases where the device is unable to obtain an IP address, the software takes the following steps:

- Disables the hardware: The software initiates the process of disabling the WiFi hardware to prevent further connection attempts.
- Restarts the hardware: After disabling the hardware, the software initiates a restart sequence to reset the hardware and attempt a new connection.

## Usage Instructions

To run this software, it should be executed with administrative privileges to ensure smooth functionality. Follow these steps to use the software effectively:

1. **Run as Administrator:** Launch the software by right-clicking and selecting "Run as administrator" to enable full control over the hardware.
2. **Monitor Connectivity:** The software will automatically monitor the WiFi connection in the background.
3. **Automated Actions:** If the device fails to obtain an IP address, the software will disable and restart the hardware automatically.

## Note

This README file provides an overview of the WiFi control software. For detailed instructions on installation and usage, refer to the documentation provided in the repository.
