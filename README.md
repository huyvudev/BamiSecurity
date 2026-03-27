# DATN — Hệ thống lưu trữ phân tán tích hợp nhận diện hành động video

Dự án này kết hợp 2 phần chính:

1) **Hệ thống lưu trữ phân tán dựa theo kiến trúc HDFS** được xây dựng bằng **Node.js** (NameNode và các DataNode), có cơ chế **chia nhỏ file (chunk)** và **nhân bản (replication)**.
2) **Dịch vụ nhận diện hành động trong video** bằng mô hình **CLIP + LRCN** chạy trên **Flask**, có thể **tự động lưu video đã phân tích** vào cụm lưu trữ phân tán.

Mục tiêu: triển khai trên các máy cấu hình nhẹ (mini PC/edge) thay vì máy chủ lớn, phục vụ bài toán lưu trữ + phân tích video cho camera an ninh.

---

## Kiến trúc tổng quan

- **NameNode** (BigData API):
  - Quyết định số chunk dựa theo kích thước file.
  - Chọn DataNode ghi (primary) và 2 DataNode nhân bản cho từng chunk.
  - Lưu metadata (file → danh sách chunk → vị trí DataNode/replica) vào MongoDB.
  - Theo dõi DataNode sống/chết thông qua heartbeat.

- **DataNode (1..n)**:
  - Nhận upload từng chunk.
  - Lưu chunk xuống ổ đĩa (thư mục `src/store/`).
  - Tự động **replicate** chunk sang 2 DataNode khác theo địa chỉ được gửi kèm trong request.

- **Action Recognition API (Flask)**:
  - Endpoint `/predict` nhận video, trích xuất frame → đặc trưng CLIP → LSTM dự đoán hành động.
  - Tuỳ chọn `save_to_bigdata=true` để upload video vào cụm BigData (NameNode/DataNode) kèm nhãn hành động.

- **Client demo (HTML)**:
  - `FE/newClient.html`: demo upload file (ảnh) theo cơ chế chunk & download ghép lại.
  - Trang index của Flask (`/`): form upload video để nhận diện và (tuỳ chọn) lưu vào BigData.

### Luồng upload (HDFS-like)

1. Client gửi metadata file → **NameNode**: `POST /api/namenode/uploadFile`.
2. NameNode trả về `numberChunk` và `metaDatas[]` (mỗi metaData chứa datanode + 2 replica).
3. Client chia file thành N chunk và upload từng chunk tới DataNode tương ứng: `POST {datanode}/api/datanode/upload`.
4. DataNode lưu chunk và replicate sang 2 DataNode replica.

### Luồng read file

1. Client hỏi NameNode: `GET /api/namenode/readFile?name=...`.
2. NameNode trả metadata đã “lọc” theo DataNode còn sống (ưu tiên primary, fallback sang replica).
3. Client tải từng chunk từ các DataNode: `GET {datanode}/api/datanode/read?name=...&index=...`.
4. Client ghép chunk → file hoàn chỉnh.

---

## Cấu trúc thư mục

- `BigDataSystem/NameNode/` — API NameNode (metadata, heartbeat, phân phối chunk)
- `BigDataSystem/DataNode1/` — API DataNode 1 (lưu chunk, replicate)
- `BigDataSystem/DataNode2/` — API DataNode 2
- `API/API_CLIP_LRCN_ver2.py` — Flask server nhận diện hành động (CLIP + LRCN) + upload BigData
- `API/captureUpload.py` — script quay webcam và upload theo cơ chế chunk (demo)
- `CLIP+LRCN/` — notebook/model huấn luyện & file `.h5`
- `FE/newClient.html` — client demo upload/read (chunk)

---

## API chính (BigDataSystem)

### NameNode

Base path: `/api/namenode`

- `POST /uploadFile`
  - Body form fields: `size`, `type`, `name`, `fileName` (và có thể thêm `category` nếu client gửi)
  - Response: `{ numberChunk, metaDatas: [{ index, datanode, datanodeReplication1, datanodeReplication2, ... }] }`

- `GET /readFile?name=...`
  - Response: `{ metadata: [{ name, index, datanode, type }] }` (đã chọn node còn sống)

- `GET /listFile?name=...&page=...&limit=...&sortBy=...`
  - Trả danh sách file (MongoDB) với phân trang + tìm kiếm gần đúng theo `name`.

### DataNode

Base path: `/api/datanode`

- `POST /upload` (multipart/form-data)
  - Fields: `name`, `index`, `datanodeReplication1`, `datanodeReplication2`, file field `file`
  - Hành vi: lưu chunk vào `src/store/` và replicate sang 2 URL replica nếu có.

- `GET /read?name=...&index=...`
  - Trả về `{ index, name, file }` (buffer).

- `POST /heartbeat`
  - Trả heartbeat chứa `address`, `datanode`, `time`.

---

## Action Recognition API (Flask)

- `POST /predict` (multipart/form-data)
  - file field: `video`
  - optional: `save_to_bigdata=true|false` (mặc định false)
  - Response: `{ predicted_action, confidence, upload_result? }`

- `GET /status`
  - Trả trạng thái server + (best-effort) kiểm tra kết nối BigData.

---

## Cài đặt & chạy (development)

> Lưu ý: mỗi node (NameNode, DataNode1, DataNode2, …) là **một project Node.js riêng**.

### 1) Chuẩn bị MongoDB

- Có thể dùng MongoDB local hoặc MongoDB Atlas.
- NameNode/DataNode đều lưu dữ liệu vào MongoDB (metadata, manager, handleFile...).

### 2) Cấu hình biến môi trường

Mỗi node cần file `.env` tại **root** của node (cùng cấp với `package.json`). Bạn có thể copy từ `.env.example`.

#### NameNode — gợi ý `.env`

File: `BigDataSystem/NameNode/.env`

```env
NODE_ENV=Namenode
HOST=localhost
PORT_NAMENODE=3000

# Ngưỡng chia chunk (đơn vị: bytes). Ví dụ ~150MB, ~200MB
MB150=157286400
MB200=209715200

MONGODB_URL=mongodb://127.0.0.1:27017/node-boilerplate
JWT_SECRET=thisisasamplesecret

# Heartbeat endpoints của DataNode (trỏ đúng host/port)
DATANODE1=http://<IP_DATANODE_1>:3001/api/datanode/heartbeat
DATANODE2=http://<IP_DATANODE_2>:3002/api/datanode/heartbeat
DATANODE3=http://<IP_DATANODE_3>:3003/api/datanode/heartbeat
```

#### DataNode — gợi ý `.env`

Ví dụ DataNode1:

File: `BigDataSystem/DataNode1/.env`

```env
NODE_ENV=Datanode1
HOST=<IP_DATANODE_1>
PORT_NAMENODE=3001
DATANODE=1

MONGODB_URL=mongodb://127.0.0.1:27017/node-boilerplate
JWT_SECRET=thisisasamplesecret
```

DataNode2 tương tự, đổi `NODE_ENV` (nếu code yêu cầu), `PORT_NAMENODE=3002`, `DATANODE=2`...

### 3) Khởi tạo ManagerDatanode (bắt buộc cho NameNode)

NameNode đọc cấu hình DataNode từ collection `ManagerDatanode` với `namenodeId = "123qwe"`.
Bạn cần tạo sẵn 1 document trong MongoDB, ví dụ:

```json
{
  "namenodeId": "123qwe",
  "datanode1": { "address": "http://<IP_DATANODE_1>:3001", "alive": true,  "size": 0 },
  "datanode2": { "address": "http://<IP_DATANODE_2>:3002", "alive": true,  "size": 0 },
  "datanode3": { "address": "http://<IP_DATANODE_3>:3003", "alive": true,  "size": 0 }
}
```

> Nếu chưa tạo, NameNode sẽ không có danh sách DataNode để phân phối chunk.

### 4) Chạy NameNode và các DataNode

Chạy **mỗi node trong 1 terminal riêng**.

**NameNode**

```bash
cd BigDataSystem/NameNode
yarn install
yarn dev
```

**DataNode1**

```bash
cd BigDataSystem/DataNode1
yarn install
yarn dev
```

**DataNode2**

```bash
cd BigDataSystem/DataNode2
yarn install
yarn dev
```

> Lưu ý: các project hiện có `docker-compose.yml` nhưng mặc định đều map `3000:3000`. Muốn chạy nhiều node trên 1 máy bằng Docker, cần sửa port mapping và `.env` tương ứng.

---

## Chạy dịch vụ nhận diện hành động (Flask)

File: `API/API_CLIP_LRCN_ver2.py`

### 1) Cài dependencies Python

Script đang import: `flask`, `opencv-python`, `numpy`, `tensorflow`, `torch`, `Pillow`, `requests`, `clip`.
Cài đặt (tham khảo):

```bash
pip install flask opencv-python numpy pillow requests tensorflow torch
# CLIP (OpenAI) tuỳ môi trường cài đặt, có thể cần:
# pip install git+https://github.com/openai/CLIP.git
```

### 2) Chỉnh đường dẫn model và BigData URL

- Trong `API/API_CLIP_LRCN_ver2.py`:
  - `MODEL_PATH` đang là đường dẫn tuyệt đối (Windows). Hãy sửa về file model thực tế trong repo, ví dụ: `CLIP+LRCN/CLIP_LRCN_model_...h5`.
  - `BIGDATA_API_URL` mặc định `http://localhost:3000/api` (NameNode). Nếu chạy NameNode ở host/port khác, cần sửa tương ứng.

### 3) Run

```bash
python API/API_CLIP_LRCN_ver2.py
```

Mặc định Flask chạy port `4000`. Mở trình duyệt: `http://localhost:4000/`.

---

## Demo nhanh

### 1) Upload/Read file (chunk) bằng HTML client

- Mở `FE/newClient.html` bằng trình duyệt.
- **Write**: nhập `name` → chọn file → Submit.
- **Read**: nhập `name` → Read → trình duyệt sẽ download file đã ghép.

### 2) Nhận diện hành động + lưu vào BigData

- Mở `http://localhost:4000/`
- Upload video và tick “Lưu video lên server BigData…”
- Kết quả trả về: hành động dự đoán + độ tin cậy + trạng thái upload.

---

## Ghi chú kỹ thuật

- **Chia chunk**: NameNode chọn `numberChunk` theo kích thước file và 2 ngưỡng `MB150`, `MB200`.
- **Replication**: DataNode replicate bằng cách gọi HTTP tới `{datanodeReplication1}/api/datanode/upload` và `{datanodeReplication2}/api/datanode/upload`.
- **Lưu trữ chunk**: DataNode ghi file vào `src/store/` và lưu `urlFile` trong MongoDB collection `handleFile`.
- **Metadata**: NameNode lưu mapping chunk ở collection `metaData` và danh sách file ở `ManagerFile`.

---

## Tài liệu/Báo cáo

- Báo cáo LaTeX nằm ở `BAOCAO/latex/`.

