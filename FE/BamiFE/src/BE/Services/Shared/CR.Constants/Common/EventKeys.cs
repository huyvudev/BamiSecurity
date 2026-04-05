using CR.Constants.Core.Users;
using System.Reflection;

namespace CR.Constants.Common
{
    public static class EventKeys
    {
        #region đăng ký tenant
        /// <summary>
        /// Gửi otp đăng ký tài khoản tenant (<see cref="UserTypes.TENANT_ADMIN"/>)
        /// </summary>
        public const string TenantSendOtp = "TenantSendOtp";

        /// <summary>
        /// Đăng ký tài khoản tenant thành công (<see cref="UserTypes.TENANT_ADMIN"/>)
        /// </summary>
        public const string TenantRegisterSuccess = "TenantRegisterSuccess";
        #endregion

        #region tài khoản trong tenant
        /// <summary>
        /// Gửi otp đăng ký tài khoản (<see cref="UserTypes.CUSTOMER"/>)
        /// </summary>
        public const string CustomerSendOtp = "CustomerSendOtp";

        /// <summary>
        /// Đăng ký tài khoản thành công (<see cref="UserTypes.CUSTOMER"/>)
        /// </summary>
        public const string CustomerRegisterSuccess = "CustomerRegisterSuccess";

        /// <summary>
        /// Gửi email quên mật khẩu (<see cref="UserTypes.CUSTOMER"/>)
        /// </summary>
        public const string CustomerForgotPassword = "CustomerForgotPassword";
        #endregion

        #region khoá học
        #region content
        public const string CourseNewContentAdded = "CourseNewContentAdded";
        #endregion

        #region people
        /// <summary>
        /// Thêm trainer vào course
        /// </summary>
        public const string CourseTrainerAdded = "CourseTrainerAdded";
        /// <summary>
        /// Xoá trainer khỏi course
        /// </summary>
        public const string CourseTrainerRemoved = "CourseTrainerRemoved";
        #endregion

        #region enrollment
        /// <summary>
        /// Cần phải tham gia lại khóa học
        /// </summary>
        [Obsolete]
        public const string CourseReEnrollmentRequired = "CourseReEnrollmentRequired";

        /// <summary>
        /// Xác nhận ghi danh khóa học
        /// </summary>
        public const string CourseEnrollmentConfirmation = "CourseEnrollmentConfirmation";

        /// <summary>
        /// Leaner hủy ghi danh khóa học
        /// </summary>
        public const string CourseEnrollmentCancellation = "CourseEnrollmentCancellation";

        /// <summary>
        /// Đang chờ phê duyệt ghi danh khóa học cho learner
        /// </summary>
        public const string EnrollmentApprovalPendingForCourse =
            "EnrollmentApprovalPendingForCourse";

        /// <summary>
        /// Yêu cầu ghi danh khóa học cho quản trị
        /// </summary>
        public const string CourseEnrollmentRequest = "CourseEnrollmentRequest";

        /// <summary>
        /// Thư nhắc phê duyệt cho quản trị (sau 2 ngày mà chưa phê duyệt sẽ được nhắc)
        /// </summary>
        public const string CourseEnrollmentApprovalReminder = "CourseEnrollmentApprovalReminder";

        /// <summary>
        /// Từ chối ghi danh khóa học (quản trị viên từ chối duyệt ghi danh thông báo cho học viên)
        /// </summary>
        public const string CourseEnrollmentRejection = "CourseEnrollmentRejection";

        /// <summary>
        /// Khóa học mà học viên đã ghi danh nhưng chưa bắt đầu
        /// </summary>
        public const string CourseEnrollmentButNotStarted = "CourseEnrollmentButNotStarted";
        #endregion

        #region publish
        /// <summary>
        /// Khóa học đã được đăng lên,
        /// Admin và Trainer sẽ nhận được thông báo này
        /// </summary>
        public const string CoursePublished = "CoursePublished";

        /// <summary>
        /// Khóa học đã được gỡ xuống (khoá học bỏ publish),
        /// Admin và Trainer sẽ nhận được thông báo này
        /// </summary>
        public const string CourseUnpublished = "CourseUnpublished";

        /// <summary>
        /// Khóa học đã bắt đầu (lấy ngày bắt đầu publish/ngày bắt đầu cho đăng ký)
        /// </summary>
        public const string CourseStarted = "CourseStarted";

        /// <summary>
        /// Khóa học đã kết thúc (lấy ngày DueDate)
        /// </summary>
        public const string CourseEnded = "CourseEnded";

        /// <summary>
        /// Nhắc thời hạn của khóa học (sắp tới DueDate)
        /// </summary>
        public const string CourseDueDateAlert = "CourseDueDateAlert";

        /// <summary>
        /// Đã qua thời hạn của khóa học
        /// </summary>
        [Obsolete]
        public const string CourseExpiredAlert = "CourseExpiredAlert";
        #endregion

        #region progress
        /// <summary>
        /// Đặt lại tiến độ khóa học
        /// </summary>
        public const string CourseProgressReset = "CourseProgressReset";

        /// <summary>
        /// Xác nhận đã hoàn thành khóa học
        /// </summary>
        public const string CourseCompletionConfirmation = "CourseCompletionConfirmation";
        #endregion

        #region discussion
        /// <summary>
        /// Thông báo khi có thảo luận mới trong khóa học cho mọi trainer và learner
        /// trong course trừ người tạo comment, gửi tới mọi người trong course
        /// </summary>
        public const string CourseDiscussionAdd = "CourseDiscussionAdd";

        /// <summary>
        /// Thông báo khi có trả lời trong thảo luận của khóa học 
        /// chỉ thông báo cho người được reply, gửi tới user được reply
        /// </summary>
        public const string CourseDiscussionReply = "CourseDiscussionReply";
        #endregion

        #region exam & assessment
        public const string CourseExamSubmitted = "CourseExamSubmitted";
        public const string CourseAssessmentSubmitted = "CourseAssessmentSubmitted";
        public const string CourseExamEndDateAlert = "CourseExamEndDateAlert";
        public const string CourseAssessmentEndDateAlert = "CourseAssessmentEndDateAlert";
        #endregion
        #endregion

        #region Chứng chỉ, kỹ năng
        /// <summary>
        /// Chứng chỉ đã hết hạn
        /// </summary>
        public const string CertificateExpiryNoti = "CertificateExpiryNoti";

        /// <summary>
        /// Nhắc hết hạn chứng chỉ
        /// </summary>
        public const string CertificateExpiryAlert = "CertificateExpiryAlert";

        /// <summary>
        /// Yêu cầu kỹ năng
        /// </summary>
        public const string SkillRequest = "SkillRequest";

        /// <summary>
        /// Yêu cầu kỹ năng đã bị từ chối
        /// </summary>
        public const string SkillRequestRejected = "SkillRequestRejected";

        /// <summary>
        /// Yêu cầu kỹ năng đã được phê duyệt
        /// </summary>
        public const string SkillRequestApproved = "SkillRequestApproved";

        /// <summary>
        /// Đã cập nhật kỹ năng
        /// </summary>
        public const string SkillUpdated = "SkillUpdated";

        /// <summary>
        /// Kỹ năng được cấp
        /// </summary>
        public const string SkillAwarded = "SkillAwarded";
        #endregion

        #region Key cũ, tạm giữ lại nếu cần
        /// <summary>
        /// Đã lên lịch khóa đào tạo có giáo viên hướng dẫn - thư mời tham dự khóa học
        /// </summary>
        [Obsolete]
        public const string InvitationToScheduledInstructorLedTraining =
            "InvitationToScheduledInstructorLedTraining";

        /// <summary>
        /// Đã thêm người hướng dẫn buổi học
        /// </summary>
        [Obsolete]
        public const string InstructorAssignedToTrainingSession =
            "InstructorAssignedToTrainingSession";

        /// <summary>
        /// Buổi học bị hủy (chỉ gửi qua ứng dụng di động)
        /// </summary>
        [Obsolete]
        public const string TrainingSessionCancelledOnlyApp = "TrainingSessionCancelledOnlyApp";

        /// <summary>
        /// Buổi học đã được cập nhật (chỉ gửi qua ứng dụng di động)
        /// </summary>
        [Obsolete]
        public const string TrainingSessionUpdatedOnlyApp = "TrainingSessionUpdatedOnlyApp";

        /// <summary>
        /// Thư nhắc phê duyệt ghi danh phiên học
        /// </summary>
        [Obsolete]
        public const string EnrollmentApprovalReminderForSession =
            "EnrollmentApprovalReminderForSession";

        /// <summary>
        /// Xác nhận ghi danh khóa đào tạo có giáo viên hướng dẫn
        /// </summary>
        [Obsolete]
        public const string EnrollmentConfirmationForInstructorLedTrainingCourse =
            "EnrollmentConfirmationForInstructorLedTrainingCourse";

        /// <summary>
        /// Yêu cầu đăng ký phiên học
        /// </summary>
        [Obsolete]
        public const string CourseSessionSelectionRequest = "CourseSessionSelectionRequest";

        /// <summary>
        /// Yêu cầu mới cho buổi học
        /// </summary>
        [Obsolete]
        public const string NewSessionRequestForCourse = "NewSessionRequestForCourse";

        /// <summary>
        /// Yêu cầu phê duyệt ghi danh phiên học
        /// </summary>
        [Obsolete]
        public const string EnrollmentApprovalRequestForCourseSession =
            "EnrollmentApprovalRequestForCourseSession";

        /// <summary>
        /// Xoá giảng viên khỏi buổi hướng dẫn
        /// </summary>
        [Obsolete]
        public const string InstructorRemovedFromTrainingSession =
            "InstructorRemovedFromTrainingSession";

        /// <summary>
        /// Thêm vào xác nhận danh sách chờ
        /// </summary>
        [Obsolete]
        public const string AddedToWaitlistConfirmation = "AddedToWaitlistConfirmation";

        /// <summary>
        /// Cần phải tham gia lại kế hoạch đào tạo
        /// </summary>
        [Obsolete]
        public const string ReJoinTheTrainingPlan = "ReJoinTheTrainingPlan";

        /// <summary>
        /// Cần phải tham gia lại kế hoạch đào tạo có CC
        /// </summary>
        [Obsolete]
        public const string ReJoinTheTrainingPlanHasCarbonCopy =
            "ReJoinTheTrainingPlanHasCarbonCopy";

        /// <summary>
        /// Chứng chỉ của kế hoạch đào tạo đã hết hạn
        /// </summary>
        [Obsolete]
        public const string TrainingPlanCertificateExpired = "TrainingPlanCertificateExpired";

        /// <summary>
        /// Đặt lại tiến trình của khóa đào tạo
        /// </summary>
        [Obsolete]
        public const string TrainingProgressReset = "TrainingProgressReset";

        /// <summary>
        /// Hủy Đăng ký Kế hoạch Đào tạo
        /// </summary>
        [Obsolete]
        public const string TrainingPlanEnrollmentCancellation =
            "TrainingPlanEnrollmentCancellation";

        /// <summary>
        /// Cần phải tham gia lại khóa đào tạo có giáo viên hướng dẫn
        /// </summary>
        [Obsolete]
        public const string NeedToRejoinInstructorLedTraining = "NeedToRejoinInstructorLedTraining";

        /// <summary>
        /// Hủy ghi danh khóa đào tạo có giáo viên hướng dẫn
        /// </summary>
        [Obsolete]
        public const string CancellationOfEnrollmentInInstructorLedTrainingCourse =
            "CancellationOfEnrollmentInInstructorLedTrainingCourse";

        /// <summary>
        /// Nhắc nhở bắt đầu khóa đào tạo có giáo viên hướng dẫn
        /// </summary>
        [Obsolete]
        public const string ReminderUpcomingInstructorLedTrainingCourse =
            "ReminderUpcomingInstructorLedTrainingCourse";

        /// <summary>
        /// Đánh giá đã được xem xét
        /// </summary>
        [Obsolete]
        public const string AssessmentReviewCompleted = "AssessmentReviewCompleted";
        #endregion

        public static List<string> GetAllEventKeys()
        {
            return typeof(EventKeys)
                .GetFields(
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                )
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue())
                .Where(x => x is string)
                .Select(x => (string)x!)
                .ToList();
        }

        public static HashSet<string> GetAllEventKeysInTenant()
        {
            return typeof(EventKeys)
                .GetFields(
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                )
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue())
                .Where(x => x is string)
                .Select(x => (string)x!)
                .Where(x => !x.StartsWith("Tenant"))
                .Distinct()
                .ToHashSet();
        }

        public static HashSet<string> GetAllExceptStoreEventKeys()
        {
            return typeof(EventKeys)
                .GetFields(
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                )
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue())
                .Where(x =>
                    x is string cst && !cst.StartsWith("Tenant") && !cst.StartsWith("Customer")
                )
                .Select(x => (string)x!)
                .Distinct()
                .ToHashSet();
        }
    }
}
