# ste.com



## Getting started

To make it easy for you to get started with GitLab, here's a list of recommended next steps.

Already a pro? Just edit this README.md and make it your own. Want to make it easy? [Use the template at the bottom](#editing-this-readme)!

## Add your files

- [ ] [Create](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#create-a-file) or [upload](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#upload-a-file) files
- [ ] [Add files using the command line](https://docs.gitlab.com/ee/gitlab-basics/add-file.html#add-a-file-using-the-command-line) or push an existing Git repository with the following command:

```
cd existing_repo
git branch -M main
git push -uf origin main
```

## Collaborate with your team

- [ ] [Invite team members and collaborators](https://docs.gitlab.com/ee/user/project/members/)
- [ ] [Create a new merge request](https://docs.gitlab.com/ee/user/project/merge_requests/creating_merge_requests.html)
- [ ] [Automatically close issues from merge requests](https://docs.gitlab.com/ee/user/project/issues/managing_issues.html#closing-issues-automatically)
- [ ] [Enable merge request approvals](https://docs.gitlab.com/ee/user/project/merge_requests/approvals/)
- [ ] [Automatically merge when pipeline succeeds](https://docs.gitlab.com/ee/user/project/merge_requests/merge_when_pipeline_succeeds.html)

## Test and Deploy

Use the built-in continuous integration in GitLab.

- [ ] [Get started with GitLab CI/CD](https://docs.gitlab.com/ee/ci/quick_start/index.html)
- [ ] [Analyze your code for known vulnerabilities with Static Application Security Testing(SAST)](https://docs.gitlab.com/ee/user/application_security/sast/)
- [ ] [Deploy to Kubernetes, Amazon EC2, or Amazon ECS using Auto Deploy](https://docs.gitlab.com/ee/topics/autodevops/requirements.html)
- [ ] [Use pull-based deployments for improved Kubernetes management](https://docs.gitlab.com/ee/user/clusters/agent/)
- [ ] [Set up protected environments](https://docs.gitlab.com/ee/ci/environments/protected_environments.html)

***

# Editing this README

When you're ready to make this README your own, just edit this file and use the handy template below (or feel free to structure it however you want - this is just a starting point!).  Thank you to [makeareadme.com](https://www.makeareadme.com/) for this template.

## Suggestions for a good README
Every project is different, so consider which of these sections apply to yours. The sections used in the template are suggestions for most open source projects. Also keep in mind that while a README can be too long and detailed, too long is better than too short. If you think your README is too long, consider utilizing another form of documentation rather than cutting out information.

## Name
Choose a self-explaining name for your project.

## Description
Let people know what your project can do specifically. Provide context and add a link to any reference visitors might be unfamiliar with. A list of Features or a Background subsection can also be added here. If there are alternatives to your project, this is a good place to list differentiating factors.

## Badges
On some READMEs, you may see small images that convey metadata, such as whether or not all the tests are passing for the project. You can use Shields to add some to your README. Many services also have instructions for adding a badge.

## Visuals
Depending on what you are making, it can be a good idea to include screenshots or even a video (you'll frequently see GIFs rather than actual videos). Tools like ttygif can help, but check out Asciinema for a more sophisticated method.

## Installation
Within a particular ecosystem, there may be a common way of installing things, such as using Yarn, NuGet, or Homebrew. However, consider the possibility that whoever is reading your README is a novice and would like more guidance. Listing specific steps helps remove ambiguity and gets people to using your project as quickly as possible. If it only runs in a specific context like a particular programming language version or operating system or has dependencies that have to be installed manually, also add a Requirements subsection.

## Usage
Use examples liberally, and show the expected output if you can. It's helpful to have inline the smallest example of usage that you can demonstrate, while providing links to more sophisticated examples if they are too long to reasonably include in the README.

## Support
Tell people where they can go to for help. It can be any combination of an issue tracker, a chat room, an email address, etc.

## Roadmap
If you have ideas for releases in the future, it is a good idea to list them in the README.

## Contributing
State if you are open to contributions and what your requirements are for accepting them.

For people who want to make changes to your project, it's helpful to have some documentation on how to get started. Perhaps there is a script that they should run or some environment variables that they need to set. Make these steps explicit. These instructions could also be useful to your future self.

You can also document commands to lint the code or run tests. These steps help to ensure high code quality and reduce the likelihood that the changes inadvertently break something. Having instructions for running tests is especially helpful if it requires external setup, such as starting a Selenium server for testing in a browser.

## Authors and acknowledgment
Show your appreciation to those who have contributed to the project.

## License
For open source projects, say how it is licensed.

## Project status
If you have run out of energy or time for your project, put a note at the top of the README saying that development has slowed down or stopped completely. Someone may choose to fork your project or volunteer to step in as a maintainer or owner, allowing your project to keep going. You can also make an explicit request for maintainers.
# MB

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

## Coding convetion C#
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
            _logger.LogInformation($"{nameof(MethodName)}: input = {JsonSerializer.Serialize(input)}, idOrther = {idOrther}");
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
    - test hub: https://gourav-d.github.io/SignalR-Web-Client/dist/
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

## Tìm code
1. regex
    `_logger\.LogError\(.+\,\s{0,1}\$.+\;`
    
## Coding convetion Angular
1. Tổng quan
    - Tạo các component hợp lý không copy lặp lại code
