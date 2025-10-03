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