from flask import Flask, jsonify, request
from pyfingerprint.pyfingerprint import PyFingerprint

app = Flask(__name__)

# Initialize the fingerprint sensor
try:
    sensor = PyFingerprint('/dev/ttyUSB0', 57600, 0xFFFFFFFF, 0x00000000)
except Exception as e:
    print("Fingerprint sensor could not be initialized!")
    print("Exception message: " + str(e))
    sensor = None

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
            return jsonify(
                {
                    'isSuccessful': False, 
                    'message': f'Fingerprint already enrolled at position {position_number}', 
                    'position': position_number, 
                    'userId': None
                }
            ), 200

        # Create template
        sensor.createTemplate()
        position_number = sensor.storeTemplate()
        return jsonify(
            {
                'isSuccessful': True, 
                'message': 'Enrolled successfully', 
                'position': position_number, 
                'userId': user_id
            }
        )

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
    app.run()  # Expose to network
