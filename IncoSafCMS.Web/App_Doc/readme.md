

hãy viết tài liệu SRS có đầy đủ các phần theo định dạng tài liệu SRS chuẩn, trong đó mô tả kỹ giải pháp nhằm giải quyết các vấn đề 
A. cho người dùng như sau
1. đối với role quản trị
- quản lý user theo chuẩn, cho phép thêm/sửa/đóng, đặt trạng thái
- quản lý ngân hàng câu hỏi, trong đó các câu hỏi được phân chia thành các catergory dạng như cây thư mục, không giới hạn số lượng cấp (nhưng thường thì tối đa có 5 cấp), các câu hỏi nằm ở cấp cuối cùng trong cây thư mục. mỗi câu hỏi có số lượng đáp án khác nhau (thường là >4 đáp án) trong đó chỉ có 1 đáp án đúng.
- quản lý khóa học, trong đó bài học có thể là file pdf, powerpoint hoặc html5, các khóa học ở mức công khai để mọi người có thể học
- quản lý đợt thi, cho phép tạo đợt thi, add cụ thể người dùng sẽ tham gia đợt thi đó. Lựa chọn catergory, số lượng câu hỏi từ mỗi catergory để tự định sinh ra đề thi với random câu hỏi, random câu trả lời chỗ mỗi user, mỗi user sẽ có đề thì hoàn toàn khác nhau, cho lựa chọn số lượng câu đáp án. Giả sử trong ngân hàng câu hỏi có những câu có 7 đáp án (trong đó chỉ 1 đáp án đúng), khi người tạo đề thì chọn số lượng đáp án là 4, thì sẽ lấy ra 1 đáp án đúng và random 3 đáp án từ trong các đáp án sai
- quản lý xem được tình hình học tập và quá trình học của các user theo phòng ban

2. đối với user
- đăng nhập để tham gia khóa học
- thực hiện làm bài thi bắt buộc khi admin có mở các đợt thi, yêu cầu vào thi
- chủ động tạo ra các bài thi thử, với cấu trúc và số lượng câu hỏi, chuyên mục (catergory) lựa chọn tương tự như bài thi thật
- toàn bộ quá trình học và thi thử/thật đều được lưu lại thành hoạt động học tập

B. với nhu cầu nghiệp vụ
1. lưu trữ và quản lý quá trình học tập
2. lưu trữ và quản lý các khóa học, link tới các nguồn tài liệu
3. lưu trữ và quản lý đợt thi/đề thi (thật và thử)
4. lưu trữ và quản lý ngân hàng câu hỏi



# Tài Liệu Đặc Tả Yêu Cầu Phần Mềm (SRS)

**Tên Dự Án:** Hệ Thống Quản Lý Học Tập Và Thi Trực Tuyến (Online Learning and Examination Management System - OLEMS)

**Phiên Bản:** 1.0

**Ngày Soạn Thảo:** 23/02/2026

**Tác Giả:** Grok 4 (Dựa trên yêu cầu người dùng)

**Mục Đích Tài Liệu:** Tài liệu này mô tả chi tiết các yêu cầu chức năng và phi chức năng của hệ thống OLEMS, nhằm giải quyết các vấn đề liên quan đến quản lý người dùng, khóa học, ngân hàng câu hỏi, đợt thi, và lưu trữ dữ liệu học tập. Tài liệu tuân theo định dạng SRS chuẩn dựa trên IEEE Std 830-1998.

---

## 1. Giới Thiệu (Introduction)

### 1.1 Mục Đích (Purpose)
Tài liệu SRS này định nghĩa các yêu cầu cho việc phát triển hệ thống OLEMS, một nền tảng trực tuyến hỗ trợ quản lý học tập và thi cử. Hệ thống nhằm giải quyết các vấn đề:
- Quản lý người dùng với các vai trò khác nhau (quản trị và người dùng thông thường).
- Quản lý ngân hàng câu hỏi theo cấu trúc cây thư mục.
- Quản lý khóa học công khai với các định dạng tài liệu đa dạng.
- Quản lý đợt thi, bao gồm thi thật và thi thử, với cơ chế sinh đề random.
- Lưu trữ và quản lý toàn bộ quá trình học tập, thi cử, và dữ liệu liên quan.
Hệ thống đảm bảo tính bảo mật, dễ sử dụng, và hỗ trợ theo dõi tiến độ học tập theo phòng ban.

### 1.2 Phạm Vi (Scope)
Hệ thống OLEMS bao gồm:
- Module quản lý người dùng.
- Module quản lý ngân hàng câu hỏi.
- Module quản lý khóa học.
- Module quản lý đợt thi và đề thi.
- Module theo dõi và lưu trữ hoạt động học tập.
Hệ thống không bao gồm: Tích hợp thanh toán, hỗ trợ video streaming thời gian thực, hoặc tích hợp với hệ thống bên thứ ba ngoài lưu trữ tài liệu (PDF, PowerPoint, HTML5).

### 1.3 Định Nghĩa, Thuật Ngữ Và Viết Tắt (Definitions, Acronyms, and Abbreviations)
- **SRS**: Software Requirements Specification (Đặc Tả Yêu Cầu Phần Mềm).
- **OLEMS**: Online Learning and Examination Management System.
- **Admin**: Quản trị viên (role quản trị).
- **User**: Người dùng thông thường.
- **Category**: Danh mục (cấu trúc cây thư mục cho câu hỏi).
- **Đợt Thi**: Một sự kiện thi cử cụ thể, có thể là thi thật hoặc thi thử.
- **Ngân Hàng Câu Hỏi**: Kho lưu trữ câu hỏi trắc nghiệm.
- **Khóa Học**: Bộ sưu tập bài học công khai.
- **Random**: Ngẫu nhiên (áp dụng cho sinh đề thi).

### 1.4 Tài Liệu Tham Khảo (References)
- IEEE Std 830-1998: Recommended Practice for Software Requirements Specifications.
- Các tiêu chuẩn bảo mật dữ liệu (GDPR, ISO 27001) cho quản lý thông tin người dùng.

### 1.5 Tổng Quan (Overview)
Phần 2 mô tả tổng quan hệ thống. Phần 3 chi tiết các yêu cầu cụ thể. Phần 4 là thông tin hỗ trợ.

---

## 2. Mô Tả Tổng Quan (Overall Description)

### 2.1 Quan Điểm Sản Phẩm (Product Perspective)
OLEMS là hệ thống mới, nhằm cải thiện quy trình học tập và thi cử truyền thống bằng cách số hóa. Nó giải quyết vấn đề thiếu tính linh hoạt trong quản lý câu hỏi, sinh đề thi ngẫu nhiên, và theo dõi tiến độ học tập. Hệ thống tương tự các nền tảng như Moodle hoặc Google Classroom nhưng tập trung vào thi trắc nghiệm random và quản lý theo phòng ban.

### 2.2 Chức Năng Sản Phẩm (Product Functions)
- **Quản Lý Người Dùng:** Thêm, sửa, đóng tài khoản, đặt trạng thái (active/inactive).
- **Quản Lý Ngân Hàng Câu Hỏi:** Tạo cấu trúc cây category (không giới hạn cấp, thường ≤5), thêm câu hỏi với ≥4 đáp án (1 đúng).
- **Quản Lý Khóa Học:** Tạo khóa học công khai với bài học PDF, PowerPoint, HTML5.
- **Quản Lý Đợt Thi:** Tạo đợt thi, chỉ định user tham gia, chọn category và số lượng câu hỏi để sinh đề random (random câu hỏi, đáp án, và số lượng đáp án).
- **Theo Dõi Học Tập:** Xem tiến độ học tập theo phòng ban.
- **Chức Năng Người Dùng:** Đăng nhập, học khóa học, thi thật/thử, lưu trữ hoạt động.
- **Lưu Trữ Nghiệp Vụ:** Quản lý dữ liệu học tập, khóa học, đợt thi, ngân hàng câu hỏi.

### 2.3 Lớp Người Dùng Và Đặc Điểm (User Classes and Characteristics)
- **Admin:** Người quản lý hệ thống, cần kỹ năng CNTT cơ bản, truy cập đầy đủ.
- **User:** Học viên, có thể là nhân viên công ty, cần giao diện thân thiện, không yêu cầu kỹ năng cao.

### 2.4 Môi Trường Hoạt Động (Operating Environment)
- **Phần Cứng:** Máy chủ web (cloud như AWS), thiết bị người dùng (PC, mobile).
- **Phần Mềm:** Web-based (HTML5, JavaScript), backend (Node.js/Python), database (SQL như PostgreSQL).
- **Hệ Điều Hành:** Cross-platform (Windows, Linux, macOS).
- **Mạng:** Yêu cầu kết nối internet ổn định.

### 2.5 Ràng Buộc Thiết Kế Và Triển Khai (Design and Implementation Constraints)
- Sử dụng framework web chuẩn (React cho frontend, Django/Express cho backend).
- Database phải hỗ trợ cấu trúc cây (hierarchical data) cho category.
- Tích hợp random generator an toàn (cryptographically secure random).

### 2.6 Giả Định Và Phụ Thuộc (Assumptions and Dependencies)
- Người dùng có tài khoản email hợp lệ để đăng ký.
- Tài liệu khóa học được upload từ nguồn đáng tin cậy.
- Không phụ thuộc vào phần cứng cụ thể, nhưng yêu cầu browser hiện đại (Chrome, Firefox).

---

## 3. Yêu Cầu Cụ Thể (Specific Requirements)

### 3.1 Giao Diện Bên Ngoài (External Interfaces)
- **Giao Diện Người Dùng:** Web UI responsive, hỗ trợ desktop và mobile.
- **Giao Diện Phần Mềm:** API RESTful cho tích hợp (nếu cần).
- **Giao Diện Phần Cứng:** Không áp dụng.
- **Giao Diện Truyền Thông:** HTTPS cho bảo mật.

### 3.2 Chức Năng (Functions)
Dưới đây là các yêu cầu chức năng chi tiết, phân loại theo vai trò và nhu cầu nghiệp vụ.

#### 3.2.1 Chức Năng Cho Role Quản Trị (Admin)
- **REQ-ADMIN-1: Quản Lý Người Dùng**
  - Hệ thống cho phép admin thêm, sửa, đóng tài khoản user.
  - Đặt trạng thái (active, inactive, suspended).
  - Gán user vào phòng ban để theo dõi.
  - Giải Pháp: Sử dụng form web để nhập dữ liệu, lưu vào database với unique ID (GUID-like).

- **REQ-ADMIN-2: Quản Lý Ngân Hàng Câu Hỏi**
  - Tạo cấu trúc category dạng cây thư mục (không giới hạn cấp, khuyến nghị ≤5).
  - Câu hỏi chỉ đặt ở cấp lá (cấp cuối).
  - Mỗi câu hỏi có ≥4 đáp án (thường >4), chỉ 1 đúng.
  - Chức năng: Thêm/sửa/xóa category và câu hỏi.
  - Giải Pháp: Sử dụng mô hình database hierarchical (adjacency list hoặc nested set) để lưu cây category. Đáp án lưu dưới dạng array JSON hoặc bảng riêng.

- **REQ-ADMIN-3: Quản Lý Khóa Học**
  - Tạo khóa học công khai, thêm bài học (PDF, PowerPoint, HTML5).
  - Link tới tài liệu (upload hoặc URL external).
  - Chức năng: Thêm/sửa/xóa khóa học.
  - Giải Pháp: Lưu metadata khóa học trong database, tài liệu lưu trên cloud storage (S3-like), hiển thị qua iframe hoặc download.

- **REQ-ADMIN-4: Quản Lý Đợt Thi**
  - Tạo đợt thi, chỉ định user tham gia (add cụ thể hoặc theo phòng ban).
  - Chọn category, số lượng câu hỏi từ mỗi category để sinh đề thi random.
  - Mỗi user có đề khác nhau: Random câu hỏi, thứ tự đáp án.
  - Chọn số lượng đáp án: Nếu câu gốc có 7 đáp án (1 đúng), chọn 4 thì lấy 1 đúng + random 3 sai.
  - Thời gian thi, điểm số tự động chấm.
  - Giải Pháp: Sử dụng algorithm random (seed per user) để sinh đề. Lưu đề thi cá nhân hóa trong session hoặc database tạm thời.

- **REQ-ADMIN-5: Theo Dõi Học Tập**
  - Xem tình hình học tập và quá trình học của user theo phòng ban.
  - Báo cáo: Tiến độ khóa học, kết quả thi, hoạt động log.
  - Giải Pháp: Query database log hoạt động, hiển thị dưới dạng dashboard với chart (using Chart.js).

#### 3.2.2 Chức Năng Cho Người Dùng (User)
- **REQ-USER-1: Đăng Nhập Và Học Khóa Học**
  - Đăng nhập bằng username/password hoặc OAuth.
  - Truy cập khóa học công khai, xem/tải bài học.
  - Giải Pháp: Authentication JWT, hiển thị danh sách khóa học.

- **REQ-USER-2: Tham Gia Thi Bắt Buộc**
  - Thông báo và yêu cầu thi khi admin mở đợt thi.
  - Làm bài thi với đề random, thời gian giới hạn.
  - Giải Pháp: Push notification (email hoặc in-app), timer client-side.

- **REQ-USER-3: Tạo Bài Thi Thử**
  - Chủ động chọn category, số lượng câu hỏi, số lượng đáp án tương tự thi thật.
  - Sinh đề random cá nhân hóa.
  - Giải Pháp: Tương tự REQ-ADMIN-4 nhưng user tự khởi tạo.

- **REQ-USER-4: Lưu Trữ Hoạt Động**
  - Toàn bộ quá trình học/thi lưu thành log (timestamp, action, result).
  - Giải Pháp: Bảng log database với foreign key liên kết user.

#### 3.2.3 Nhu Cầu Nghiệp Vụ
- **REQ-BUS-1: Lưu Trữ Quá Trình Học Tập**
  - Lưu log chi tiết: Thời gian học, tiến độ, kết quả thi.
  - Giải Pháp: Database schema với bảng ActivityLog.

- **REQ-BUS-2: Lưu Trữ Khóa Học**
  - Lưu metadata và link tài liệu (PDF, PowerPoint, HTML5).
  - Giải Pháp: Bảng Courses với fields: title, public_status, materials (JSON array).

- **REQ-BUS-3: Lưu Trữ Đợt Thi/Đề Thi**
  - Lưu đợt thi (thật/thử), đề thi cá nhân hóa, kết quả.
  - Giải Pháp: Bảng Exams (metadata đợt thi), ExamInstances (đề per user), Results.

- **REQ-BUS-4: Lưu Trữ Ngân Hàng Câu Hỏi**
  - Lưu category cây, câu hỏi, đáp án.
  - Giải Pháp: Bảng Categories (hierarchical), Questions, Answers (with is_correct flag).

### 3.3 Yêu Cầu Hiệu Suất (Performance Requirements)
- Thời gian phản hồi: <2 giây cho query thông thường.
- Hỗ trợ 1000 user đồng thời.
- Sinh đề thi: <1 giây per user.

### 3.4 Yêu Cầu Cơ Sở Dữ Liệu Logic (Logical Database Requirements)
- Sử dụng relational database (e.g., PostgreSQL).
- Schema: Users, Departments, Categories, Questions, Answers, Courses, Materials, Exams, ExamInstances, Results, ActivityLogs.
- Đảm bảo tính toàn vẹn: Foreign keys, unique constraints.

### 3.5 Ràng Buộc Thiết Kế (Design Constraints)
- Giao diện phải tuân thủ WCAG cho accessibility.
- Sử dụng responsive design.

### 3.6 Tuân Thủ Tiêu Chuẩn (Standards Compliance)
- Bảo mật: OWASP Top 10.
- Dữ liệu: Tuân thủ luật bảo vệ dữ liệu Việt Nam.

### 3.7 Thuộc Tính Hệ Thống Phần Mềm (Software System Attributes)
- **Độ Tin Cậy (Reliability):** 99% uptime, backup daily.
- **Khả Năng Sẵn Sàng (Availability):** 24/7.
- **Bảo Mật (Security):** Encryption dữ liệu, role-based access control (RBAC).
- **Khả Năng Bảo Trì (Maintainability):** Code modular, documentation.
- **Khả Năng Chuyển Đổi (Portability):** Cloud-agnostic.

---

## 4. Thông Tin Hỗ Trợ (Supporting Information)
- **Phụ Lục A: Use Case Diagrams** (Mô tả textual: Use cases cho từng REQ).
- **Phụ Lục B: Data Flow Diagrams** (Mô tả flow dữ liệu từ user đến database).
- **Phụ Lục C: Glossary** (Thuật ngữ bổ sung nếu cần).

Tài liệu này có thể được cập nhật dựa trên phản hồi. Nếu cần thêm chi tiết hoặc diagram, vui lòng chỉ định.