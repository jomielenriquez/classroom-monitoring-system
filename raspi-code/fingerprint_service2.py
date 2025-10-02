from flask import Flask, jsonify, request
from pyfingerprint.pyfingerprint import PyFingerprint

app = Flask(__name__)

# Initialize the fingerprint sensor
sensor = PyFingerprint('/dev/ttyUSB0', 57600, 0xFFFFFFFF, 0x00000000)

@app.route('/enroll', methods=['POST'])
def enroll():
    try:
        user_id = request.json.get('userId')
        if not user_id:
            return jsonify({'error': 'userId is required'}), 400

        # Wait for finger
        print("Waiting for finger...")
        while not sensor.readImage():
            pass

        sensor.convertImage(0x01)

        # Check if already enrolled
        result = sensor.searchTemplate()
        position_number = result[0]
        if position_number >= 0:
            return jsonify({'message': f'Fingerprint already enrolled at position {position_number}'}), 200

        # Create template
        sensor.createTemplate()
        position_number = sensor.storeTemplate()
        return jsonify({'message': 'Enrolled successfully', 'position': position_number, 'userId': user_id})

    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/verify', methods=['POST'])
def verify():
    try:
        print("Waiting for finger...")
        while not sensor.readImage():
            pass

        sensor.convertImage(0x01)
        result = sensor.searchTemplate()
        position_number = result[0]

        if position_number == -1:
            return jsonify({'message': 'No match found'})
        else:
            return jsonify({'message': f'Match found at position {position_number}'})

    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)  # Expose to network
