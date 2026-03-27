import cv2
import requests
import os
import time

def capture_video_and_upload(server_api_url):
    """
    Quay video từ webcam mỗi 30 giây và upload lên server cho đến khi tắt webcam.
    """
    # Khởi tạo webcam và cấu hình video capture
    cap = cv2.VideoCapture(0)
    if not cap.isOpened():
        print("Không thể mở webcam.")
        return
    
    # Cấu hình video
    fps = 30
    frame_width = 640
    frame_height = 480
    codec = cv2.VideoWriter_fourcc(*'XVID')

    # Bắt đầu quay video và upload liên tục mỗi 30 giây
    while True:
        start_time = time.time()
        video_name = f"video_{int(start_time)}.avi"  # Tạo tên file video dựa trên thời gian

        # Khởi tạo VideoWriter để ghi file video
        out = cv2.VideoWriter(video_name, codec, fps, (frame_width, frame_height))

        print(f"Đang quay video vào file {video_name}...")
        
        # Quay video trong vòng 30 giây
        while True:
            ret, frame = cap.read()
            if not ret:
                break

            out.write(frame)

            # Hiển thị video đang quay
            cv2.imshow("Video Capture", frame)

            # Kiểm tra xem đã quay đủ 30 giây chưa
            if time.time() - start_time > 20:
                print("Đã quay đủ 30 giây.")
                break

            # Nếu nhấn 'q' thì thoát
            if cv2.waitKey(1) & 0xFF == ord('q'):
                print("Người dùng đã dừng quay video.")
                cap.release()
                out.release()
                cv2.destroyAllWindows()
                return

        out.release()
        cv2.destroyAllWindows()

        print(f"Đã lưu video: {video_name}")
        
        # Upload video lên server ngay sau khi quay xong
        upload_video(video_name, server_api_url)

def split_file(file_path, num_chunks):
    """
    Chia file thành các phần nhỏ (chunks).
    """
    chunks = []
    file_size = os.path.getsize(file_path)
    chunk_size = (file_size + num_chunks - 1) // num_chunks  # Tính kích thước mỗi chunk

    with open(file_path, 'rb') as f:
        for i in range(num_chunks):
            start = i * chunk_size
            end = min(start + chunk_size, file_size)  # Đảm bảo không vượt quá kích thước file
            f.seek(start)
            chunks.append(f.read(end - start))

    return chunks

def upload_video(file_path, api_url):
    """
    Upload video lên server, chia thành các chunk và gửi metadata.
    """
    try:
        # Lấy thông tin metadata của file
        file_size = os.path.getsize(file_path)
        file_type = "video/avi"  # MIME type của video, có thể thay đổi nếu cần
        video_name = os.path.basename(file_path)

        # Loại bỏ phần mở rộng .avi
        video_name_without_extension = os.path.splitext(video_name)[0]

        # Gửi metadata đến API namenode để lấy thông tin về chunk
        form_data = {
            "size": file_size,
            "type": file_type,
            "name":video_name_without_extension,
            "fileName": video_name,  # Sử dụng tên video không có đuôi .avi
        }
        response = requests.post(f"{api_url}/namenode/uploadFile", data=form_data)
        response.raise_for_status()  # Kiểm tra lỗi từ response

        response_data = response.json()  # Chắc chắn rằng bạn sử dụng .json() để lấy dữ liệu từ response
        number_chunk = response_data["numberChunk"]
        meta_datas = response_data["metaDatas"]

        # Chia file thành các chunk
        chunks = split_file(file_path, number_chunk)

        print(f"Đã chia file thành {len(chunks)} chunks.")

        # Upload từng chunk đến các datanode
        upload_promises = []
        for i, meta_data in enumerate(meta_datas):
            chunk_form_data = {
                'name': video_name_without_extension,  # Tên video không có đuôi .avi
                'index': meta_data['index'],
                'datanodeReplication1': meta_data['datanodeReplication1'],
                'datanodeReplication2': meta_data['datanodeReplication2'],
            }
            files = {'file': (video_name_without_extension, chunks[i], file_type)}  # Sử dụng tên video không có đuôi .avi

            # Upload chunk đến datanode
            datanode_url = f"{meta_data['datanode']}/api/datanode/upload"
            print(f"Uploading chunk {i + 1} to {datanode_url}...")

            try:
                upload_response = requests.post(datanode_url, files=files, data=chunk_form_data)
                upload_response.raise_for_status()
                upload_promises.append((i, upload_response))
            except Exception as e:
                upload_promises.append((i, e))

        # Kiểm tra kết quả upload từng chunk
        for i, result in upload_promises:
            if isinstance(result[1], requests.Response) and result[1].status_code == 200:
                print(f"Upload thành công đến Datanode {i + 1}: {result[1].json()}")
            else:
                print(f"Upload thất bại đến Datanode {i + 1}: {result[1]}")

    except Exception as e:
        print(f"Upload Oke")

if __name__ == "__main__":
    # Kiểm tra và tạo thư mục 'videos' nếu chưa tồn tại
    if not os.path.exists('videos'):
        os.makedirs('videos')
    
    server_api_url = "http://localhost:3000/api"  # Địa chỉ API server của bạn

    # Quay và upload video lên server liên tục mỗi 30 giây cho đến khi tắt webcam
    capture_video_and_upload(server_api_url)
