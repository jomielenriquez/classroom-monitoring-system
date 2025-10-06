from flask import Flask, jsonify, request
from flask_cors import CORS
from pyfingerprint.pyfingerprint import PyFingerprint

app = Flask(__name__)
CORS(app)  # <-- Enable CORS for all routes
#30F32AF3-7AA7-49D2-A227-7A3D06401EF4

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
                    'userId': user_id
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
            return jsonify(
                {
                    'isSuccessful': False, 
                    'message': 'No match found', 
                    'position': position_number
                }
            )
        else:
            return jsonify(
                {
                    'isSuccessful': True, 
                    'message': 'Match found', 
                    'position': position_number
                }
            )

    except Exception as e:
        return jsonify({'error': str(e)}), 500


@app.route('/delete', methods=['DELETE'])
def delete():
    try:
        # Expecting a JSON body like { "position": 3 }
        position_number = request.json.get('position')

        if position_number is None:
            return jsonify(
                {
                    'isSuccessful': False, 
                    'message': 'Position is required', 
                    'position': position_number
                }
            ), 400

        # Check valid range
        if not (0 <= position_number < sensor.getTemplateCount()):
            return jsonify(
                {
                    'isSuccessful': False, 
                    'message': 'Invalid position number', 
                    'position': position_number
                }
            ), 400

        # Delete the template
        if sensor.deleteTemplate(position_number):
            return jsonify({
                'isSuccessful': True,
                'message': f'Template at position {position_number} deleted successfully',
                'position': position_number
            })
        else:
            return jsonify({
                'isSuccessful': False,
                'message': f'Failed to delete template at position {position_number}',
                'position': position_number
            })

    except Exception as e:
        return jsonify({'error': str(e)}), 500


if __name__ == '__main__':
    app.run()  # Expose to network
