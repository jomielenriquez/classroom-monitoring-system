### CLOUD – BASED TOUCHSCREEN INTERFACE FOR CLASSROOM MONITORING AND ROOM UTILIZATION TRACKING

#### How to host the python file in raspberry pi



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

- 4. Install the following
```bash
sudo apt update
sudo apt install python3-pip -y

sudo apt update
sudo apt install python3-dev python3-pip python3-setuptools python3-wheel build-essential -y
sudo apt install libjpeg-dev zlib1g-dev libfreetype6-dev liblcms2-dev libopenjp2-7-dev libtiff5-dev tk-dev tcl-dev libwebp-dev libharfbuzz-dev libfribidi-dev libxcb1-dev -y

sudo pip3 install flask pyfingerprint pyserial gunicorn --break-system-packages
```

- 5. Clone this repository 
```
cd ~
git clone https://github.com/jomielenriquez/classroom-monitoring-system.git
```

- 6. Git sparse-checkout
```
cd classroom-monitoring-system
git sparse-checkout init --cone
git sparse-checkout set raspi-code
```

- 7. install flask cors
```
sudo pip3 install flask-cors --break-system-packages
```

- 8. Create systemd service
So it auto start after boot:
```bash
sudo nano /etc/systemd/system/fingerprint.service
```

- 9. Paste this:
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

- 10. Install Chromium
```
sudo apt update
sudo apt install chromium -y
```

