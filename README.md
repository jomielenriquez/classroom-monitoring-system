### CLOUD – BASED TOUCHSCREEN INTERFACE FOR CLASSROOM MONITORING AND ROOM UTILIZATION TRACKING

#### How to host the python file in raspberry pi

##### Install the following
```bash
sudo apt update
sudo apt install python3-pip nginx -y
pip3 install flask pyfingerprint pyserial gunicorn --break-system-packages
```

##### Create systemd service
So it auto start after boot:
```bash
sudo nano /etc/systemd/system/fingerprint.service
```
Paste this:
```bash
[Unit]
Description=Fingerprint Flask API
After=network.target

[Service]
User=pi
WorkingDirectory=/home/pi/fingerprint
ExecStart=/usr/bin/gunicorn --workers 3 --bind 127.0.0.1:5000 fingerprint_api:app
Restart=always

[Install]
WantedBy=multi-user.target
```
Save and run:
```bash
sudo systemctl daemon-reload
sudo systemctl enable fingerprint
sudo systemctl start fingerprint
```
Check status
```bash
systemctl status fingerprint
```
Restart
```bash
sudo systemctl restart fingerprint.service
```

### How to setup raspberry pi
- 1. Install 32bit lite os
- 2. Update and install necessarry apps
```
sudo apt update && sudo apt upgrade -y
sudo apt install --no-install-recommends xorg openbox chromium-browser x11-xserver-utils unclutter lightdm -y
```
- 3. Enable Serial for R307
```
sudo raspi-config
```
then navigate
```
Interface Options → Serial Port → 
  Disable login shell over serial → Yes  
  Enable serial hardware port → Yes
```
Then reboot
```
sudo reboot
```
