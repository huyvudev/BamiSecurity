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
import requests
import time
import shutil
import logging
import traceback

# Thiết lập logging
logging.basicConfig(level=logging.INFO, 
                    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
                    handlers=[logging.FileHandler("server.log"), logging.StreamHandler()])
logger = logging.getLogger("action-recognition-server")

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

# Địa chỉ API server BigData
BIGDATA_API_URL = "http://localhost:3000/api"
# Số lần thử lại tối đa khi gặp lỗi kết nối
MAX_RETRIES = 3
# Thời gian chờ giữa các lần thử (giây)
RETRY_DELAY = 2

# Khởi tạo ứng dụng Flask
app = Flask(__name__)

# Tải mô hình CLIP
def load_clip_model():
    device = "cuda" if torch.cuda.is_available() else "cpu"
    logger.info(f"CLIP sẽ sử dụng thiết bị: {device}")
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
    
    logger.info(f"Video có {video_frames_count} frames, sẽ lấy mẫu mỗi {skip_frames_window} frames")
    
    # Trích xuất các frame
    for frame_counter in range(SEQUENCE_LENGTH):
        # Đặt vị trí của frame cần đọc
        video_reader.set(cv2.CAP_PROP_POS_FRAMES, frame_counter * skip_frames_window)
        # Đọc frame
        success, frame = video_reader.read()
        if not success:
            logger.warning(f"Không thể đọc frame thứ {frame_counter * skip_frames_window}")
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
        logger.warning(f"Chỉ trích xuất được {len(frames_list)}/{SEQUENCE_LENGTH} frames, sẽ thêm frames trống")
        frames_list += [np.zeros((IMAGE_HEIGHT, IMAGE_WIDTH, 3))] * (SEQUENCE_LENGTH - len(frames_list))
    
    # Giải phóng video reader
    video_reader.release()
    
    # Chuyển danh sách frames thành mảng numpy
    frames_array = np.array(frames_list)
    logger.info(f"Đã trích xuất {frames_array.shape[0]} frames từ video")
    return frames_array

def extract_clip_features(frames_array):
    """
    Trích xuất đặc trưng CLIP từ các khung hình
    """
    sequence_length = frames_array.shape[0]
    embedding_dim = 512  # Kích thước embedding của CLIP (ViT-B/32)
    
    # Khởi tạo mảng để lưu các đặc trưng CLIP
    clip_features = np.zeros((sequence_length, embedding_dim))
    
    logger.info(f"Bắt đầu trích xuất đặc trưng CLIP cho {sequence_length} frames")
    
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
    
    logger.info(f"Đã trích xuất xong đặc trưng CLIP, shape: {clip_features.shape}")
    return clip_features

def predict_video_action(video_path):
    """
    Dự đoán hành động trong video sử dụng mô hình CLIP-LRCN
    """
    global model
    logger.info(f"Bắt đầu dự đoán hành động cho video: {video_path}")
    
    # Trích xuất các frames
    frames = extract_frames(video_path)
    
    # Trích xuất đặc trưng CLIP
    clip_features = extract_clip_features(frames)
    
    # Thêm chiều batch
    clip_features = np.expand_dims(clip_features, axis=0)
    
    # Dự đoán
    logger.info("Đang thực hiện dự đoán với mô hình...")
    predictions = model.predict(clip_features)[0]
    predicted_class_index = np.argmax(predictions)
    predicted_class_name = CLASSES_LIST[predicted_class_index]
    confidence = float(predictions[predicted_class_index] * 100)
    
    logger.info(f"Kết quả dự đoán: {predicted_class_name} với độ tin cậy {confidence:.2f}%")
    
    # Trả về tên lớp dự đoán và độ tin cậy
    return predicted_class_name, confidence

# Hàm mới để tải video lên server BigData
def split_file(file_path, num_chunks):
    """
    Chia file thành các phần nhỏ (chunks).
    """
    chunks = []
    file_size = os.path.getsize(file_path)
    chunk_size = (file_size + num_chunks - 1) // num_chunks  # Tính kích thước mỗi chunk

    logger.info(f"Chia file {file_path} ({file_size} bytes) thành {num_chunks} chunks, mỗi chunk khoảng {chunk_size} bytes")

    with open(file_path, 'rb') as f:
        for i in range(num_chunks):
            start = i * chunk_size
            end = min(start + chunk_size, file_size)  # Đảm bảo không vượt quá kích thước file
            f.seek(start)
            chunk_data = f.read(end - start)
            chunks.append(chunk_data)
            logger.debug(f"Đã đọc chunk {i+1}/{num_chunks}, kích thước: {len(chunk_data)} bytes")

    return chunks

def upload_video_to_bigdata(file_path, action_name, confidence):
    """
    Upload video lên server BigData, chia thành các chunk và gửi metadata.
    Sử dụng tên hành động làm nhãn dán cho video khi lưu trữ.
    """
    logger.info(f"Bắt đầu upload video lên BigData: {file_path}, hành động: {action_name}")
    
    retries = 0
    while retries < MAX_RETRIES:
        try:
            # Lấy thông tin metadata của file
            file_size = os.path.getsize(file_path)
            file_type = "video/mp4"  # MIME type của video, có thể thay đổi nếu cần
            video_name = os.path.basename(file_path)

            # Sử dụng định dạng datetime dễ đọc
            current_time = time.strftime("%Y%m%d_%H%M%S")
            video_name_with_info = f"{action_name}_{current_time}"

            # Gửi metadata đến API namenode để lấy thông tin về chunk
            form_data = {
                "size": file_size,
                "type": file_type,
                "name": f"{action_name}_{current_time}",  # Tên với định dạng datetime rõ ràng
                "fileName": video_name,
                "category": action_name,  # Vẫn giữ trường category để phân loại
            }
            
            logger.info(f"Gửi metadata đến namenode: {BIGDATA_API_URL}/namenode/uploadFile")
            logger.debug(f"Form data: {form_data}")
            
            response = requests.post(f"{BIGDATA_API_URL}/namenode/uploadFile", data=form_data, timeout=30)
            
            # Log thông tin response
            logger.info(f"Namenode response status: {response.status_code}")
            logger.debug(f"Namenode response body: {response.text}")
            
            response.raise_for_status()  # Kiểm tra lỗi từ response

            response_data = response.json()
            number_chunk = response_data["numberChunk"]
            meta_datas = response_data["metaDatas"]

            logger.info(f"Namenode yêu cầu chia thành {number_chunk} chunks")

            # Chia file thành các chunk
            chunks = split_file(file_path, number_chunk)

            logger.info(f"Đã chia file thành {len(chunks)} chunks")

            # Upload từng chunk đến các datanode
            upload_promises = []
            success_count = 0
            for i, meta_data in enumerate(meta_datas):
                chunk_form_data = {
                    'name': f"{action_name}_{current_time}",  # Sử dụng tên với định dạng datetime
                    'index': meta_data['index'],
                    'datanodeReplication1': meta_data['datanodeReplication1'],
                    'datanodeReplication2': meta_data['datanodeReplication2'],
                    'category': action_name,  # Vẫn giữ category để dễ dàng phân loại
                }
                files = {'file': (f"{action_name}_{current_time}", chunks[i], file_type)}

                # Upload chunk đến datanode
                datanode_url = f"{meta_data['datanode']}/api/datanode/upload"
                logger.info(f"Uploading chunk {i + 1}/{number_chunk} to {datanode_url}...")
                logger.debug(f"Chunk form data: {chunk_form_data}")

                try:
                    upload_response = requests.post(datanode_url, files=files, data=chunk_form_data, timeout=30)
                    
                    # Log thông tin response từ datanode
                    logger.info(f"Datanode {i+1} response status: {upload_response.status_code}")
                    logger.debug(f"Datanode {i+1} response body: {upload_response.text}")
                    
                    upload_response.raise_for_status()
                    success_count += 1
                    logger.info(f"Upload thành công đến Datanode {i + 1}")
                except Exception as e:
                    logger.error(f"Upload thất bại đến Datanode {i + 1}: {str(e)}")
                    logger.debug(traceback.format_exc())
            
            # Trả về kết quả upload
            upload_result = {
                "success": success_count == number_chunk,
                "message": f"Đã upload {success_count}/{number_chunk} chunks thành công",
                "video_name": f"{action_name}_{current_time}",
                "category": action_name
            }
            logger.info(f"Kết quả upload: {upload_result}")
            return upload_result

        except requests.exceptions.RequestException as e:
            retries += 1
            logger.error(f"Lỗi kết nối khi upload video (lần thử {retries}/{MAX_RETRIES}): {str(e)}")
            if retries < MAX_RETRIES:
                logger.info(f"Thử lại sau {RETRY_DELAY} giây...")
                time.sleep(RETRY_DELAY)
            else:
                logger.error(f"Đã thử {MAX_RETRIES} lần nhưng vẫn thất bại")
                return {
                    "success": False,
                    "message": f"Lỗi kết nối sau {MAX_RETRIES} lần thử: {str(e)}",
                    "video_name": None
                }
        except Exception as e:
            logger.error(f"Lỗi không xác định khi upload video: {str(e)}")
            logger.debug(traceback.format_exc())
            return {
                "success": False,
                "message": f"Lỗi không xác định: {str(e)}",
                "video_name": None
            }

# Tải mô hình trước khi chạy ứng dụng
logger.info(f"Đang tải mô hình từ {MODEL_PATH}...")
try:
    model = load_model(MODEL_PATH)
    logger.info("Đã tải mô hình thành công!")
except Exception as e:
    logger.error(f"Lỗi khi tải mô hình: {str(e)}")
    logger.debug(traceback.format_exc())

@app.route('/predict', methods=['POST'])
def predict():
    logger.info("Nhận request dự đoán mới")
    
    # Kiểm tra xem yêu cầu có chứa file video không
    if 'video' not in request.files:
        logger.warning("Không tìm thấy file video trong request")
        return jsonify({'error': 'Không tìm thấy file video'}), 400
    
    video_file = request.files['video']
    
    # Nếu người dùng không chọn file, trình duyệt cũng gửi một phần rỗng không có tên file
    if video_file.filename == '':
        logger.warning("Tên file video trống")
        return jsonify({'error': 'Không có video nào được chọn'}), 400
    
    # Kiểm tra xem có muốn lưu vào BigData hay không
    save_to_bigdata = request.form.get('save_to_bigdata', 'false').lower() == 'true'
    logger.info(f"Tham số save_to_bigdata: {save_to_bigdata}")
    
    try:
        # Lưu file tải lên tạm thời
        temp_file = tempfile.NamedTemporaryFile(delete=False, suffix='.mp4')
        video_path = temp_file.name
        video_file.save(video_path)
        temp_file.close()
        
        logger.info(f"Đã lưu video tạm thời tại: {video_path}")
        logger.info(f"Kích thước video: {os.path.getsize(video_path)} bytes")
        
        # Sử dụng mô hình để dự đoán
        action, confidence = predict_video_action(video_path)
        
        upload_result = None
        # Nếu được yêu cầu, tải video lên server BigData
        if save_to_bigdata:
            logger.info("Chuẩn bị upload video lên server BigData")
            
            # Tạo bản sao của video để đảm bảo có đuôi file đúng
            video_copy_path = f"temp_{int(time.time())}.mp4"
            shutil.copy2(video_path, video_copy_path)
            logger.info(f"Đã tạo bản sao video tại: {video_copy_path}")
            
            # Tải lên server BigData với thông tin nhận dạng
            upload_result = upload_video_to_bigdata(video_copy_path, action, confidence)
            
            # Xóa bản sao sau khi tải lên
            os.unlink(video_copy_path)
            logger.info(f"Đã xóa bản sao video: {video_copy_path}")
        
        # Xóa file tạm
        os.unlink(video_path)
        logger.info(f"Đã xóa file video tạm thời: {video_path}")
        
        # Trả về kết quả dự đoán và upload (nếu có)
        result = {
            'predicted_action': action,
            'confidence': confidence
        }
        
        if upload_result:
            result['upload_result'] = upload_result
        
        logger.info(f"Trả về kết quả: {result}")
        return jsonify(result)
        
    except Exception as e:
        logger.error(f"Lỗi trong quá trình xử lý: {str(e)}")
        logger.debug(traceback.format_exc())
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
                .checkbox-container { margin: 15px 0; }
                .debug-log { margin-top: 20px; padding: 10px; background: #f8f8f8; border: 1px solid #ddd; height: 200px; overflow-y: auto; font-family: monospace; font-size: 12px; }
                .status { padding: 10px; margin: 10px 0; border-radius: 5px; }
                .success { background-color: #e6ffe6; border: 1px solid #99ff99; }
                .error { background-color: #ffe6e6; border: 1px solid #ff9999; }
            </style>
        </head>
        <body>
            <h1>API Nhận Dạng Hành Động Video (CLIP-LRCN)</h1>
            <div class="status success">
                Server đang hoạt động ở cổng: 4000
            </div>
            <form id="upload-form" enctype="multipart/form-data">
                <h3>Tải lên video để nhận dạng hành động:</h3>
                <input type="file" name="video" accept="video/*" required>
                <div class="checkbox-container">
                    <input type="checkbox" id="save_to_bigdata" name="save_to_bigdata" value="true">
                    <label for="save_to_bigdata">Lưu video lên server BigData sau khi phân tích</label>
                </div>
                <button type="submit">Phân Tích</button>
            </form>
            <div id="result"></div>
            
            <h3>Thông tin Debug:</h3>
            <div class="debug-log" id="debug-log">Server sẵn sàng nhận request...</div>
            
            <script>
                document.getElementById('upload-form').addEventListener('submit', function(e) {
                    e.preventDefault();
                    const formData = new FormData(this);
                    const resultDiv = document.getElementById('result');
                    const debugLog = document.getElementById('debug-log');
                    
                    resultDiv.innerHTML = 'Đang xử lý...';
                    debugLog.innerHTML += '<br/>Đang gửi video đến server...';
                    
                    fetch('/predict', {
                        method: 'POST',
                        body: formData
                    })
                    .then(response => {
                        debugLog.innerHTML += '<br/>Đã nhận phản hồi từ server, status: ' + response.status;
                        return response.json();
                    })
                    .then(data => {
                        if (data.error) {
                            resultDiv.innerHTML = `<p>Lỗi: ${data.error}</p>`;
                            debugLog.innerHTML += '<br/>Lỗi: ' + data.error;
                        } else {
                            debugLog.innerHTML += '<br/>Phân tích thành công: ' + data.predicted_action;
                            
                            let resultHtml = `
                                <h3>Kết quả phân tích:</h3>
                                <p><strong>Hành động dự đoán:</strong> ${data.predicted_action}</p>
                                <p><strong>Độ tin cậy:</strong> ${data.confidence.toFixed(2)}%</p>
                            `;
                            
                            // Nếu có kết quả upload, hiển thị thêm thông tin
                            if (data.upload_result) {
                                debugLog.innerHTML += '<br/>Kết quả upload: ' + (data.upload_result.success ? 'Thành công' : 'Thất bại');
                                resultHtml += `
                                    <h3>Kết quả lưu trữ:</h3>
                                    <div class="${data.upload_result.success ? 'status success' : 'status error'}">
                                        <p><strong>Trạng thái:</strong> ${data.upload_result.success ? 'Thành công' : 'Thất bại'}</p>
                                        <p><strong>Thông tin:</strong> ${data.upload_result.message}</p>
                                        ${data.upload_result.video_name ? `<p><strong>Đã lưu với tên:</strong> ${data.upload_result.video_name}</p>` : ''}
                                    </div>
                                `;
                            } else {
                                debugLog.innerHTML += '<br/>Video không được lưu vào BigData';
                            }
                            
                            resultDiv.innerHTML = resultHtml;
                        }
                    })
                    .catch(error => {
                        resultDiv.innerHTML = `<p>Lỗi: ${error.message}</p>`;
                        debugLog.innerHTML += '<br/>Lỗi kết nối: ' + error.message;
                    });
                });
            </script>
        </body>
    </html>
    """

@app.route('/status', methods=['GET'])
def status():
    """Kiểm tra trạng thái của server và kết nối đến BigData"""
    try:
        # Kiểm tra trạng thái mô hình
        model_status = "OK" if model else "Không khả dụng"
        
        # Kiểm tra kết nối đến BigData server
        bigdata_status = "Không thể kết nối"
        try:
            response = requests.get(f"{BIGDATA_API_URL}/namenode/status", timeout=5)
            if response.status_code == 200:
                bigdata_status = "Kết nối được"
        except:
            pass
        
        return jsonify({
            "server": "Đang chạy",
            "model": model_status,
            "bigdata_server": bigdata_status
        })
    except Exception as e:
        return jsonify({
            "server": "Đang chạy nhưng có lỗi",
            "error": str(e)
        })

if __name__ == '__main__':
    # Chỉ dùng cho phát triển - sử dụng máy chủ WSGI phù hợp trong sản xuất
    port = int(os.environ.get('PORT', 4000))
    logger.info(f"Khởi động server trên cổng {port}...")
    app.run(host='0.0.0.0', port=port, debug=True)