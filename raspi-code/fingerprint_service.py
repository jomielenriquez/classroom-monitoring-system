from flask import Flask, request, jsonify
from pyfingerprint.pyfingerprint import PyFingerprint

app = Flask(__name__)
f = PyFingerprint('/dev/ttyUSB0', 57600, 0xFFFFFFFF, 0x00000000)

@app.route('/enroll', methods=['POST'])
def enroll():
    userid = request.args.get('userid')
    if not userid:
        return jsonify({'error': 'userid required'}), 400

    try:
        # Wait for finger
        while f.readImage() == False:
            pass
        f.convertImage(0x01)

        result = f.searchTemplate()
        if result[0] >= 0:
            return jsonify({'error': 'Finger already enrolled'}), 400

        # Wait for same finger again
        while f.readImage() == True:
            pass
        while f.readImage() == False:
            pass
        f.convertImage(0x02)

        if f.compareCharacteristics() == 0:
            return jsonify({'error': 'Fingers do not match'}), 400

        f.createTemplate()
        positionNumber = f.storeTemplate()

        # Respond with slot number
        return jsonify({'success': True, 'userid': userid, 'pageid': positionNumber})

    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/verify', methods=['POST'])
def verify():
    try:
        while f.readImage() == False:
            pass
        f.convertImage(0x01)

        result = f.searchTemplate()
        positionNumber = result[0]

        if positionNumber == -1:
            return jsonify({'match': False})

        return jsonify({'match': True, 'pageid': positionNumber})

    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001)
