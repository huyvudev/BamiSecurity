import cv2
import time
import os
import requests
import tempfile
import numpy as np

# Các tham số cấu hình
API_URL = "http://192.168.1.9:4000/predict"  # URL của API dự đoán
RECORDING_SECONDS = 20  # Thời gian ghi hình mỗi chu kỳ
FRAME_WIDTH = 640
FRAME_HEIGHT = 480
FPS = 30  # Frames per second cho video ghi
SAVE_TO_BIGDATA = True  # Mặc định lưu video lên BigData sau khi phân tích

# Tham số phát hiện chuyển động
MOTION_THRESHOLD = 2500  # Ngưỡng phát hiện chuyển động (số pixel thay đổi)
MIN_MOTION_FRAMES = 3    # Số khung hình liên tiếp phải có chuyển động
MOTION_COOLDOWN = 5      # Thời gian chờ (giây) trước khi phát hiện chuyển động tiếp theo

def capture_and_predict():
    """
    Chức năng chính: bắt hình ảnh từ webcam, phát hiện chuyển động,
    ghi thành video, gửi đến API và hiển thị kết quả dự đoán
    """
    # Mở webcam
    cap = cv2.VideoCapture(0)
    if not cap.isOpened():
        print("Không thể mở webcam!")
        return
    
    # Thiết lập webcam
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, FRAME_WIDTH)
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT)
    
    # Biến để theo dõi trạng thái ghi hình
    is_recording = False
    start_time = 0
    video_writer = None
    temp_video_path = None
    
    # Biến cho phát hiện chuyển động
    prev_frame = None
    motion_detected_count = 0
    last_motion_time = 0
    motion_detection_enabled = True  # Biến để bật/tắt tính năng phát hiện chuyển động
    save_to_bigdata_enabled = SAVE_TO_BIGDATA  # Biến để bật/tắt tính năng lưu vào BigData
    
    try:
        running = True
        while running:
            # Đọc frame từ webcam
            ret, frame = cap.read()
            if not ret:
                print("Không thể đọc frame từ webcam!")
                break
            
            # Phát hiện chuyển động nếu không đang ghi và chức năng được bật
            motion_status = "Dang theo doi chuyen dong" if motion_detection_enabled else "Phát hiện chuyển động: TẮT"
            bigdata_status = "Dang luu vao CSDL" if save_to_bigdata_enabled else "Lưu vào BigData: TẮT"
            
            if not is_recording and motion_detection_enabled:
                # Xử lý frame để phát hiện chuyển động
                gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
                gray = cv2.GaussianBlur(gray, (21, 21), 0)
                
                # Nếu đây là khung hình đầu tiên, khởi tạo
                if prev_frame is None:
                    prev_frame = gray
                    continue
                
                # Tính toán sự khác biệt giữa frame hiện tại và trước đó
                frame_delta = cv2.absdiff(prev_frame, gray)
                thresh = cv2.threshold(frame_delta, 25, 255, cv2.THRESH_BINARY)[1]
                
                # Mở rộng ngưỡng để làm đầy các lỗ, sau đó tìm các contour
                thresh = cv2.dilate(thresh, None, iterations=2)
                contours, _ = cv2.findContours(thresh.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
                
                # Tính tổng diện tích của các contour
                total_area = sum(cv2.contourArea(c) for c in contours)
                
                # Hiển thị thông tin phát hiện chuyển động
                cv2.putText(frame, f"Mức chuyển động: {total_area:.1f}", (10, 60), 
                           cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 1)
                
                # Vẽ contour
                for c in contours:
                    if cv2.contourArea(c) > 100:  # Lọc ra các contour nhỏ
                        (x, y, w, h) = cv2.boundingRect(c)
                        cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)
                
                # Kiểm tra xem có chuyển động đáng kể không
                if total_area > MOTION_THRESHOLD:
                    motion_detected_count += 1
                    cv2.putText(frame, "PHAT HIEN CHUYEN DONG!", (10, 30), 
                               cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
                else:
                    motion_detected_count = 0
                
                # Nếu phát hiện chuyển động liên tục đủ số khung hình
                current_time = time.time()
                if (motion_detected_count >= MIN_MOTION_FRAMES and 
                    current_time - last_motion_time > MOTION_COOLDOWN):
                    print("Phát hiện chuyển động! Bắt đầu ghi hình...")
                    
                    # Tạo file video tạm thời
                    temp_video = tempfile.NamedTemporaryFile(delete=False, suffix='.mp4')
                    temp_video_path = temp_video.name
                    temp_video.close()
                    
                    # Tạo VideoWriter để ghi video
                    fourcc = cv2.VideoWriter_fourcc(*'mp4v')
                    video_writer = cv2.VideoWriter(temp_video_path, fourcc, FPS, (FRAME_WIDTH, FRAME_HEIGHT))
                    
                    is_recording = True
                    start_time = current_time
                    motion_detected_count = 0
                    last_motion_time = current_time
                
                # Cập nhật frame trước đó
                prev_frame = gray
            
            # Tạo nút bắt đầu quay thủ công
            button_color = (0, 0, 255) if is_recording else (0, 255, 0)
            button_text = "DANG GHI HINH" if is_recording else "BAT DAU GHI HINH"
            
            # Vẽ nút quay thủ công
            cv2.rectangle(frame, (10, FRAME_HEIGHT - 60), (250, FRAME_HEIGHT - 10), button_color, -1)
            cv2.putText(frame, button_text, (20, FRAME_HEIGHT - 30), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 255, 255), 2)
            
            # Vẽ nút bật/tắt phát hiện chuyển động
            motion_button_color = (0, 255, 0) if motion_detection_enabled else (128, 128, 128)
            motion_button_text = "TAT PHAT HIEN" if motion_detection_enabled else "BAT PHAT HIEN"
            
            cv2.rectangle(frame, (270, FRAME_HEIGHT - 60), (470, FRAME_HEIGHT - 10), motion_button_color, -1)
            cv2.putText(frame, motion_button_text, (280, FRAME_HEIGHT - 30), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 255, 255), 2)
            
            # Vẽ nút bật/tắt lưu vào BigData
            bigdata_button_color = (0, 255, 0) if save_to_bigdata_enabled else (128, 128, 128)
            bigdata_button_text = "TAT LUU BIGDATA" if save_to_bigdata_enabled else "BAT LUU BIGDATA"
            
            cv2.rectangle(frame, (490, FRAME_HEIGHT - 60), (690, FRAME_HEIGHT - 10), bigdata_button_color, -1)
            cv2.putText(frame, bigdata_button_text, (500, FRAME_HEIGHT - 30), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 255, 255), 2)
            
            # Hiển thị trạng thái phát hiện chuyển động và lưu BigData
            cv2.putText(frame, motion_status, (10, 90), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 255), 1)
            cv2.putText(frame, bigdata_status, (10, 120), 
                       cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 255), 1)
            
            # Nếu đang ghi hình, hiển thị thời gian còn lại
            if is_recording:
                seconds_left = RECORDING_SECONDS - int(time.time() - start_time)
                if seconds_left >= 0:
                    cv2.putText(frame, f"Thoi gian con lai: {seconds_left}s", (10, 30), 
                               cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
                    
                    # Ghi frame vào video
                    video_writer.write(frame)
                    
                # Nếu hết thời gian ghi hình
                if time.time() - start_time >= RECORDING_SECONDS:
                    is_recording = False
                    
                    # Đóng video writer
                    if video_writer is not None:
                        video_writer.release()
                        video_writer = None
                    
                    # Kiểm tra xem file video có tồn tại và có kích thước
                    if os.path.exists(temp_video_path) and os.path.getsize(temp_video_path) > 0:
                        print(f"Đã ghi video thành công: {temp_video_path}")
                        print("Đang gửi đến API để phân tích...")
                        
                        # Gửi video đến API
                        try:
                            with open(temp_video_path, 'rb') as video_file:
                                # Thêm tham số save_to_bigdata
                                files = {'video': (os.path.basename(temp_video_path), video_file, 'video/mp4')}
                                data = {'save_to_bigdata': 'true' if save_to_bigdata_enabled else 'false'}
                                
                                # Log thông tin gửi
                                print(f"Gửi request đến API: {API_URL}")
                                print(f"Với tham số: {data}")
                                
                                # Gửi request với timeout dài hơn (30 giây) và in thêm thông tin debug
                                response = requests.post(API_URL, files=files, data=data, timeout=30)
                                
                                print(f"Nhận response với status code: {response.status_code}")
                            
                            # Kiểm tra response
                            if response.status_code == 200:
                                result = response.json()
                                
                                # Hiển thị kết quả
                                print("\n" + "="*50)
                                print(f"Hành động được dự đoán: {result['predicted_action']}")
                                print(f"Độ tin cậy: {result['confidence']:.2f}%")
                                
                                # Hiển thị thông tin upload nếu có
                                if 'upload_result' in result:
                                    print("\nKết quả lưu vào BigData:")
                                    print(f"Trạng thái: {'Thành công' if result['upload_result']['success'] else 'Thất bại'}")
                                    print(f"Thông báo: {result['upload_result']['message']}")
                                    if result['upload_result']['video_name']:
                                        print(f"Tên video đã lưu: {result['upload_result']['video_name']}")
                                else:
                                    print("\nVideo không được lưu vào BigData.")
                                    
                                print("="*50 + "\n")
                            else:
                                print(f"Lỗi từ API: {response.status_code}")
                                print(response.text)
                        
                        except Exception as e:
                            print(f"Lỗi khi gửi video đến API: {str(e)}")
                            import traceback
                            traceback.print_exc()
                    else:
                        print("Không thể ghi video hoặc video trống!")
                    
                    # Xóa file video tạm thời
                    try:
                        os.unlink(temp_video_path)
                        print(f"Đã xóa file tạm: {temp_video_path}")
                    except Exception as e:
                        print(f"Không thể xóa file tạm: {str(e)}")
            
            # Hiển thị frame
            cv2.imshow('Webcam', frame)
            
            # Xử lý sự kiện bàn phím và chuột
            key = cv2.waitKey(1) & 0xFF
            
            # Thoát nếu nhấn 'q'
            if key == ord('q'):
                running = False
                break
                
            # Xử lý sự kiện nhấp chuột
            def mouse_callback(event, x, y, flags, param):
                nonlocal is_recording, start_time, video_writer, temp_video_path
                nonlocal motion_detection_enabled, save_to_bigdata_enabled
                
                # Kiểm tra xem có nhấp vào nút hay không
                if event == cv2.EVENT_LBUTTONDOWN:
                    # Kiểm tra nút ghi hình thủ công
                    if 10 <= x <= 250 and FRAME_HEIGHT - 60 <= y <= FRAME_HEIGHT - 10:
                        # Nếu chưa ghi hình, bắt đầu ghi hình
                        if not is_recording:
                            # Tạo file video tạm thời
                            temp_video = tempfile.NamedTemporaryFile(delete=False, suffix='.mp4')
                            temp_video_path = temp_video.name
                            temp_video.close()
                            
                            # Tạo VideoWriter để ghi video
                            fourcc = cv2.VideoWriter_fourcc(*'mp4v')
                            video_writer = cv2.VideoWriter(temp_video_path, fourcc, FPS, (FRAME_WIDTH, FRAME_HEIGHT))
                            
                            is_recording = True
                            start_time = time.time()
                            print(f"Bắt đầu ghi hình trong {RECORDING_SECONDS} giây...")
                    
                    # Kiểm tra nút bật/tắt phát hiện chuyển động
                    elif 270 <= x <= 470 and FRAME_HEIGHT - 60 <= y <= FRAME_HEIGHT - 10:
                        motion_detection_enabled = not motion_detection_enabled
                        status = "BẬT" if motion_detection_enabled else "TẮT"
                        print(f"Phát hiện chuyển động: {status}")
                    
                    # Kiểm tra nút bật/tắt lưu vào BigData
                    elif 490 <= x <= 690 and FRAME_HEIGHT - 60 <= y <= FRAME_HEIGHT - 10:
                        save_to_bigdata_enabled = not save_to_bigdata_enabled
                        status = "BẬT" if save_to_bigdata_enabled else "TẮT"
                        print(f"Lưu vào BigData: {status}")
            
            # Thiết lập callback cho cửa sổ hiển thị
            cv2.setMouseCallback('Webcam', mouse_callback)
                
    finally:
        # Giải phóng tài nguyên
        if video_writer is not None:
            video_writer.release()
        cap.release()
        cv2.destroyAllWindows()
        print("Đã đóng webcam và kết thúc chương trình.")

if __name__ == "__main__":
    capture_and_predict()