from flask import Flask, jsonify, request
from flask_cors import CORS  # 👈 import CORS
import time

app = Flask(__name__)
CORS(app)  # 👈 enable CORS for all routes

@app.route('/enroll', methods=['GET', 'POST'])
def enroll():
    # Example position number
    position_number = 3
    
    # 👇 Delay for 2 seconds
    time.sleep(2)

    return jsonify({
        'isSuccessful': False,
        'message': f'Fingerprint already enrolled at position {position_number}',
        'position': position_number,
        'userId': None
    }), 200

@app.route('/verify', methods=['POST'])
def verify():
    # Example position number
    position_number = 5
    
    # 👇 Delay for 2 seconds
    time.sleep(2)

    return jsonify(
        {
            'isSuccessful': True, 
            'message': 'Match found', 
            'position': position_number
        }
    ), 200

if __name__ == '__main__':
    app.run(host='localhost', port=5000, debug=True)
