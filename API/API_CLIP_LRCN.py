import os
import cv2
import numpy as np
import tensorflow as tf
from tensorflow.keras.models import load_model
from flask import Flask, request, jsonify
import tempfile
from PIL import Image
import torch
import clip

# Các tham số cho mô hình
IMAGE_HEIGHT, IMAGE_WIDTH = 224, 224  # Thay đổi kích thước cho CLIP
SEQUENCE_LENGTH = 20
# Danh sách các lớp phân loại - phải khớp với danh sách đã sử dụng khi huấn luyện
CLASSES_LIST = [
    "ApplyEyeMakeup", "ApplyLipstick", "Archery", "BabyCrawling",
    "BandMarching", "BaseballPitch", "Basketball", "BasketballDunk", "BenchPress",
    "Biking", "Billiards", "BlowDryHair", "BlowingCandles", "BodyWeightSquats",
    "Bowling", "BoxingPunchingBag", "BoxingSpeedBag", "BrushingTeeth", "CliffDiving", "CricketBowling", "CricketShot", "CuttingInKitchen",
    "Diving", "Drumming",
    "FrontCrawl", "GolfSwing", "Haircut", "Hammering",
    "HammerThrow",  "HandstandWalking", "HeadMassage", "HighJump",
    "HorseRace", "HulaHoop", "IceDancing", "JavelinThrow",
    "JugglingBalls", "Kayaking", "Knitting",
    "LongJump",  "MilitaryParade", "Mixing", "MoppingFloor",
    "Nunchucks", "ParallelBars", "PizzaTossing", "PlayingCello", "PlayingGuitar", "PlayingPiano",
    "PlayingSitar", "PlayingTabla", "PlayingViolin", "PommelHorse",
    "PullUps", "Punch", "PushUps", "Rafting",
    "Rowing",
    "SkateBoarding", "Skiing", "Skijet", "SkyDiving", "SoccerJuggling",
    "SoccerPenalty", "StillRings", "SumoWrestling", "Swing",
    "TableTennisShot", "TaiChi", "TennisSwing", "ThrowDiscus", "TrampolineJumping",
    "Typing", "VolleyballSpiking", "WalkingWithDog", "WallPushups",
    "WritingOnBoard", "YoYo"
]
# Đường dẫn cố định đến mô hình 
MODEL_PATH = "D:\DATN\CLIP_LRCN_model_Date_Time_2025_04_10_20_10_18_Loss_0_0899_Accuracy.h5"

# Khởi tạo ứng dụng Flask
app = Flask(__name__)

# Tải mô hình CLIP
def load_clip_model():
    device = "cuda" if torch.cuda.is_available() else "cpu"
    print(f"CLIP sẽ sử dụng thiết bị: {device}")
    model, preprocess = clip.load("ViT-B/32", device=device)
    return model, preprocess, device

# Tải CLIP model
clip_model, clip_preprocess, device = load_clip_model()

def extract_frames(video_path):
    """
    Trích xuất các khung hình từ video để dự đoán
    """
    frames_list = []
    # Đọc video file
    video_reader = cv2.VideoCapture(video_path)
    # Lấy tổng số frame
    video_frames_count = int(video_reader.get(cv2.CAP_PROP_FRAME_COUNT))
    # Tính khoảng cách giữa các frame để lấy
    skip_frames_window = max(int(video_frames_count / SEQUENCE_LENGTH), 1)
    
    # Trích xuất các frame
    for frame_counter in range(SEQUENCE_LENGTH):
        # Đặt vị trí của frame cần đọc
        video_reader.set(cv2.CAP_PROP_POS_FRAMES, frame_counter * skip_frames_window)
        # Đọc frame
        success, frame = video_reader.read()
        if not success:
            break
        # Thay đổi kích thước và chuyển sang RGB
        resized_frame = cv2.resize(frame, (IMAGE_HEIGHT, IMAGE_WIDTH))
        rgb_frame = cv2.cvtColor(resized_frame, cv2.COLOR_BGR2RGB)
        # Chuẩn hóa frame
        normalized_frame = rgb_frame / 255.0
        # Thêm frame vào danh sách
        frames_list.append(normalized_frame)
    
    # Đảm bảo có đủ số lượng frame
    if len(frames_list) < SEQUENCE_LENGTH:
        frames_list += [np.zeros((IMAGE_HEIGHT, IMAGE_WIDTH, 3))] * (SEQUENCE_LENGTH - len(frames_list))
    
    # Giải phóng video reader
    video_reader.release()
    
    # Chuyển danh sách frames thành mảng numpy
    frames_array = np.array(frames_list)
    return frames_array

def extract_clip_features(frames_array):
    """
    Trích xuất đặc trưng CLIP từ các khung hình
    """
    sequence_length = frames_array.shape[0]
    embedding_dim = 512  # Kích thước embedding của CLIP (ViT-B/32)
    
    # Khởi tạo mảng để lưu các đặc trưng CLIP
    clip_features = np.zeros((sequence_length, embedding_dim))
    
    # Trích xuất đặc trưng cho từng khung hình
    with torch.no_grad():
        for i in range(sequence_length):
            # Chuyển đổi sang định dạng PIL Image
            frame = frames_array[i] * 255  # Rescale về khoảng [0, 255]
            frame = np.uint8(frame)
            pil_image = Image.fromarray(frame)
            
            # Áp dụng tiền xử lý CLIP
            preprocessed_image = clip_preprocess(pil_image).unsqueeze(0).to(device)
            
            # Trích xuất đặc trưng
            features = clip_model.encode_image(preprocessed_image)
            
            # Lưu đặc trưng
            clip_features[i] = features.cpu().numpy()
    
    return clip_features

def predict_video_action(video_path):
    """
    Dự đoán hành động trong video sử dụng mô hình CLIP-LRCN
    """
    global model
    # Trích xuất các frames
    frames = extract_frames(video_path)
    
    # Trích xuất đặc trưng CLIP
    clip_features = extract_clip_features(frames)
    
    # Thêm chiều batch
    clip_features = np.expand_dims(clip_features, axis=0)
    
    # Dự đoán
    predictions = model.predict(clip_features)[0]
    predicted_class_index = np.argmax(predictions)
    predicted_class_name = CLASSES_LIST[predicted_class_index]
    confidence = float(predictions[predicted_class_index] * 100)
    
    # Trả về tên lớp dự đoán và độ tin cậy
    return predicted_class_name, confidence

# Tải mô hình trước khi chạy ứng dụng
print(f"Đang tải mô hình từ {MODEL_PATH}...")
model = load_model(MODEL_PATH)
print("Đã tải mô hình thành công!")

@app.route('/predict', methods=['POST'])
def predict():
    # Kiểm tra xem yêu cầu có chứa file video không
    if 'video' not in request.files:
        return jsonify({'error': 'Không tìm thấy file video'}), 400
    
    video_file = request.files['video']
    
    # Nếu người dùng không chọn file, trình duyệt cũng gửi một phần rỗng không có tên file
    if video_file.filename == '':
        return jsonify({'error': 'Không có video nào được chọn'}), 400
    
    try:
        # Lưu file tải lên tạm thời
        temp_file = tempfile.NamedTemporaryFile(delete=False, suffix='.mp4')
        video_path = temp_file.name
        video_file.save(video_path)
        temp_file.close()
        
        # Sử dụng mô hình để dự đoán
        action, confidence = predict_video_action(video_path)
        
        # Xóa file tạm
        os.unlink(video_path)
        
        # Trả về kết quả dự đoán
        return jsonify({
            'predicted_action': action,
            'confidence': confidence
        })
        
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/', methods=['GET'])
def index():
    return """
    <html>
        <head>
            <title>API Nhận Dạng Hành Động Video (CLIP-LRCN)</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 0; padding: 20px; line-height: 1.6; }
                h1 { color: #333; }
                form { margin: 20px 0; padding: 20px; border: 1px solid #ddd; border-radius: 5px; }
                button { background: #4CAF50; color: white; padding: 10px 15px; border: none; cursor: pointer; }
                #result { margin-top: 20px; padding: 10px; border: 1px solid #eee; border-radius: 5px; }
            </style>
        </head>
        <body>
            <h1>API Nhận Dạng Hành Động Video (CLIP-LRCN)</h1>
            <form id="upload-form" enctype="multipart/form-data">
                <h3>Tải lên video để nhận dạng hành động:</h3>
                <input type="file" name="video" accept="video/*" required>
                <button type="submit">Phân Tích</button>
            </form>
            <div id="result"></div>
            
            <script>
                document.getElementById('upload-form').addEventListener('submit', function(e) {
                    e.preventDefault();
                    const formData = new FormData(this);
                    const resultDiv = document.getElementById('result');
                    
                    resultDiv.innerHTML = 'Đang xử lý...';
                    
                    fetch('/predict', {
                        method: 'POST',
                        body: formData
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.error) {
                            resultDiv.innerHTML = `<p>Lỗi: ${data.error}</p>`;
                        } else {
                            resultDiv.innerHTML = `
                                <h3>Kết quả phân tích:</h3>
                                <p><strong>Hành động dự đoán:</strong> ${data.predicted_action}</p>
                                <p><strong>Độ tin cậy:</strong> ${data.confidence.toFixed(2)}%</p>
                            `;
                        }
                    })
                    .catch(error => {
                        resultDiv.innerHTML = `<p>Lỗi: ${error.message}</p>`;
                    });
                });
            </script>
        </body>
    </html>
    """

if __name__ == '__main__':
    # Chỉ dùng cho phát triển - sử dụng máy chủ WSGI phù hợp trong sản xuất
    port = int(os.environ.get('PORT', 4000))
    app.run(host='0.0.0.0', port=port, debug=True)