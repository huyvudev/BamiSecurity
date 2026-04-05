namespace CR.Constants.ErrorCodes
{
    public class ExamErrorCode : ErrorCode
    {
        protected ExamErrorCode()
            : base() { }

        //public const int <Tên Error> = <giá trị>;
        public const int ExamQuestionNotFound = 5001;
        public const int ExamAnswerNotFound = 5002;
        public const int ExamAnswerIsRequired = 5003;
        public const int ExamQuestionPropertyIsRequired = 5004;
        public const int ExamAnswerPropertyIsRequired = 5005;
        public const int ExamQuestionPoolNotFound = 5006;
        public const int ExamExamNotFound = 5007;
        public const int ExamResultNotFound = 5008;
        public const int ExamQuestionContentIsRequired = 5009;
        public const int ExamUserNotExistInCourse = 5010;
        public const int ExamUserAttemptIsLimitExpired = 5011;
        public const int ExamExamIsExpired = 5012;
        public const int ExamLearnerIsInActiveInCourse = 5013;
        public const int ExamUnfinishedExam = 5014;
        public const int ExamNotAllowReview = 5015;
        public const int ExamCourseExamNotFound = 5016;
        public const int ExamCourseExamNotTimeToTakeExam = 5017;
        public const int ExamCourseExamOverdueDoingExam = 5018;
        public const int ExamCourseExamResultQuestionNotFound = 5019;
    }
}
