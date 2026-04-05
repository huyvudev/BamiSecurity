# Printing factory

## Run project

1. Docker
    - `cd src`
    - `docker-compose up -d --build`
2. Chạy môi trường local
    1. Dùng dòng lệnh
        - Vào từng project có file `Program.cs` bằng cmd hoặc wt
        - Chạy lệnh `dotnet run Program.cs`
    2. Dùng visual studio
        - Chạy multiple project

## Commit

1. Tổng quan
    - Tách riêng các commit fix bug và commit tính năng mới
    - Nếu cần `pull` code mới về thì sử dụng lệnh `git stash` để cất code đi sau khi pull code từ _branch develop_ về thì dùng lệnh `git stash apply` để đưa code đang làm dở ra lại không mỗi lần `pull code` lại `commit` một lần tạo rất nhiều `commit` thừa
        - Với lệnh `git stash apply` code thể dùng trong giao diện của visual studio
        - Với trường hợp cần bỏ code đã cất đi dùng lênh `git stash drop` lưu ý cẩn thận khi dùng lệnh này sẽ xoá code đã cất đi _**Không thể khôi phục**_
    - Không tự ý merge code phải tạo `merge request` để check lại
    - `message commit` viết có ý nghĩa theo convetion sau:
        - Bug: `[Bug] nguồn bug từ đâu - nội dung bug được fix`
        - Tính năng mới: `Mô tả tính năng`

## Comment code

1. Tổng quan
    - comment càng chi tiết càng tốt
    - các method giải thích đầu vào đầu ra, logic phức tạp
2. Repositories
    - comment tại tất cả các method trong repo giải thích đầu vào đầu ra xử lý bên trong nếu join nhiều bảng
3. Package trong db
    - các procedure mô tả dùng để làm gì càng chi tiết càng tốt, các tham số truyền vào xử lý như nào nếu truyền thì làm gì nếu truyền null thì làm gì (vd: trong trường hợp một số procedure truyền trading_provider_id = số hoặc = null)

## Coding convetion C #

1. Tổng quan
    - Tuân theo convetion **PascalCase** tức viết hoa chữ cái đầu tiên
    - Các biến sẽ viết theo convetion **CamelCase** viết thường chữ cái đầu
2. Dtos
    - Đặt tên Dto nếu là model tương tự với Entity (_các trường dữ liệu giống hệt_) thì đặt tên theo dạng `EntityDto` nếu là một class mở rộng thêm các trường thì đặt tên theo dạng `EntityWith` `GiDo` `Dto`
    - Không dùng chung 1 class Dto ở 2 hàm mà không cùng trả ra các trường giống nhau
    - Các class Dto thêm mới từ giờ không thêm tiếp vào project `EPIC.Entities` mà phải để ở các project `Entities` của chính microservice đó, tương lại các Dto đặt không đúng microservice sẽ phải được chuyển về đúng chỗ.
3. Services
    - Viết trong project Domain của từng _microservice_
4. Repositories
    - Các method `GetById` thực sự chỉ đơn giản là `GetById` không xử lý thêm các trường bên ngoài entity nếu có đặt tên hàm khác, không `throw Exception` tại các method `GetById` nếu không tìm thấy chỉ trả ra `null`
    - Mỗi một bảng 1 repository trường hợp đã xử lý nhiều bảng theo nghiệp vụ thì comment summary trên class repository là tương tác đến những bảng nào theo dạng `<summary>` `Xử lý bảng:` `EP_ABC, EP_DEF` `</summary>`
5. Cách viết log
    - log information cho các tham số truyền vào trong hàm dạng như sau:

    ```cs
        public PagingResult<ResultDto> MethodName(InputDto input, int? idOrther = null)
        {
            _logger.LogInformation($"{nameof(MethodName)}: input = {@input}, idOrther = {idOrther}");
        }
    ```

6. Cách mô tả swagger
    - Chèn thêm attribute `ProducesResponseType` theo mẫu như bên dưới
    - Thêm các comment summery vào các trường trong class Dto trả ra

    ```cs
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppDistributionGetAll(string keyword) 
        {
            //nội dung api
        }
    ```

    - Config swagger như sau để sinh comment trên swagger

    ```cs
        // Set the comments path for the Swagger JSON and UI.**
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"EPIC.GarnerEntities.xml"));
    ```

7. SignalR
    - test hub: <https://gourav-d.github.io/SignalR-Web-Client/dist/>
8. API Test
    - api thử nghiệm chỉ nên chạy ở local đặt attribute sau để không thấy api để chạy

    ```cs
    [ApiExplorerSettings(IgnoreApi = true)]
    ```

9. Header
    - Content-Security-Policy

    ```
    Content-Security-Policy: default-src 'self'; script-src 'self' http://auth.demo.abc http://localhost; style-src 'self' http://auth.demo.abc http://localhost https://fonts.googleapis.com 'unsafe-inline'; img-src 'self' data:; font-src 'self' http://auth.demo.abc https://fonts.gstatic.com 'unsafe-inline'; connect-src 'self' http://localhost wss://localhost http://auth.demo.abc 'unsafe-inline' 'unsafe-eval'; media-src 'self'; object-src 'none'"
    ```

## Coding convetion DB

1. Tổng quan
    - Đặt tên các bảng theo đúng ý nghĩa và liên quan đến mảng nghiệp vụ nào
2. Error Code
    - Số mã lỗi đặt tương ứng với nghiệp vụ đang xử lý vd: bond 3xxx không đặt nhảy cách số phải liên tiếp. các mã lỗi nếu cần xử lý trong C# thì viết tiếp vào class ErrorCodes theo đúng thứ tự tăng dần không đặt linh tinh.
3. Migrations
    - Tạo migrations trong project HostConsole

    ```
        dotnet ef migrations add <MigrationName>
    ```

    - Dùng lệnh sau để tạo script thay vì chạy database update

    ```
        dotnet ef migrations script -o Scripts/changedb.sql <MigrationTruoc> <MigrationHienTai>
    ```

    - Scaffold db

    ```
        dotnet ef dbcontext scaffold Name=ConnectionStrings:EPIC Oracle.EntityFrameworkCore -o Models -f
    ```

    - Lỗi OracleException: ORA-01950: no privileges on tablespace 'USERS' -> gõ lệnh sau vào script trong db:
        GRANT UNLIMITED TABLESPACE TO <Schema Name>;

## RabbitMQ

1. Consumer
    1. ExchangeDeclare
    - `durable`: Tham số này xác định xem sàn có tồn tại sau khi RabbitMQ bị đóng và khởi động lại hay không. Nếu bạn đặt giá trị của `durable` là `true`, sàn sẽ tồn tại và không bị mất dữ liệu khi RabbitMQ khởi động lại. Nếu bạn đặt `durable` là `false`, sàn chỉ tồn tại trong bộ nhớ và sẽ bị xóa khi RabbitMQ khởi động lại. Để có sàn không bền, bạn có thể sử dụng `false` cho tham số durable
    - `autoDelete`: Tham số này xác định xem sàn có tự động bị xóa khi không có bất kỳ bản tin nào được gửi đến nó hay không. Nếu bạn đặt giá trị của `autoDelete` là `true`, sàn sẽ tự động bị xóa khi không có bản tin nào được gửi đến nó. Nếu autoDelete là false, sàn sẽ không tự động bị xóa. Thông thường, `autoDelete` được sử dụng khi bạn muốn tạo các sàn tạm thời
    2. QueueDeclare
    - `durable`: Được sử dụng để xác định xem hàng đợi có tồn tại sau khi RabbitMQ bị đóng và khởi động lại hay không. Nếu bạn đặt giá trị của durable là true, hàng đợi sẽ tồn tại và không bị mất dữ liệu khi RabbitMQ khởi động lại. Ngược lại, nếu bạn đặt giá trị là `false`, hàng đợi chỉ tồn tại trong bộ nhớ và sẽ bị xóa khi RabbitMQ khởi động lại
    - `exclusive`: Nếu bạn đặt giá trị của `exclusive` là `true`, hàng đợi sẽ chỉ được sử dụng bởi một kết nối duy nhất và sẽ bị xóa khi kết nối đó đóng. Nếu bạn đặt `exclusive` là `false`, hàng đợi có thể được sử dụng bởi nhiều kết nối. Thông thường, `exclusive` thường được sử dụng để tạo hàng đợi tạm thời mà chỉ một kết nối duy nhất có quyền truy cập
    - `autoDelete`: Nếu bạn đặt giá trị của `autoDelete` là `true`, hàng đợi sẽ tự động bị xóa khi không có người tiêu dùng (consumer) nào kết nối đến nó. Nếu `autoDelete` là `false`, hàng đợi sẽ không tự động bị xóa khi không có consumer nào. Thông thường, `autoDelete` được sử dụng khi bạn muốn tạo các hàng đợi tạm thời

2. Producer
    1. Publish
    - Đẩy message bằng `BasicPublish`
    - Đảm bảo message được push thành công: <https://www.rabbitmq.com/reliability.html#routing>

## Tìm code

1. regex
    `_logger\.LogError\(.+\,\s{0,1}\$.+\;`
2. xoá toàn bộ thư mục `bin` và `obj`

    ```sh
    find . -iname "bin" -o -iname "obj" | xargs rm -rf
    ```

## Coding convetion Angular

1. Tổng quan
    - Tạo các component hợp lý không copy lặp lại code

## Mã lỗi của các service

    - Core 3xxx
    - Learn 4xxx
    - Exam 5xxx
    - File 11xxx

## Prompt Copilot

1. Ví dụ viết API thuộc Exam module

    ```txt
    đọc config quan hệ các bảng từ file #LearnDbContext.cs và file #ExamDbContext.cs.
    Có các bảng học viên #LearnLearner.cs, khoá học #LearnCourse.cs với customerUserId lấy từ #LearnLearner join với #User.cs, bài kiểm tra trong khoá học #ExCourseExam.cs kế thừa #ExExamBase.cs, câu hỏi trong bài kiểm tra #ExCourseExamQuestion.cs #ExExamQuestionBase.cs #ExCourseExamAnswer.cs #ExCourseExamAnswerBase.cs, kết quả bài làm nằm trong các bảng #ExCourseExamResultQuestion.cs #ExCourseExamResultAnswer.cs, các loại câu hỏi nằm trong enum #QuestionTypes.cs.
    ```

    ```txt
    với các chức năng liên quan đến bài kiểm tra sẽ viết logic tại project CR.Exam.ApplicationServices tuỳ theo module là gì mà viết trong thư mục đấy, các class Dto viết ở project CR.Exam.Dtos, khai báo DI tại file #ExamConfigStartUp.cs, gọi api tại project CR.Core.API tuỳ theo module là gì mà viết trong thư mục đấy. Các logic xử lý cần comment đầy đủ, các trường trong Dto cần comment summary các trường hợp xảy ra lỗi cần viết thêm mã lỗi vào file #ExamErrorCode và throw ra #ExamExcepion.cs sau đó thêm localization message lỗi tại file #vi.xml và #en.xml với mã là "error_{nameof(ExamErrorCode.ExamTenLoi)}" ví dụ error_ExamCourseNotFound.
    ```

    ```txt
    viết cho tôi API ... (kế thừa #ApiControllerBase) với đầu vào ... xử lý .... Với API có dạng:
    /// <summary>
    /// Lấy khoá học theo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("find/{id}")]
    [ProducesResponseType(typeof(ApiResponse<DetailCourseDto>), StatusCodes.Status200OK)]
    public ApiResponse Find(int id)
    {
        try
        {
            return new(_courseService.Find(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }
    ```
