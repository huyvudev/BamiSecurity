namespace CR.Constants.ErrorCodes
{
    public class LearnErrorCode : ErrorCode
    {
        protected LearnErrorCode()
            : base() { }

        //public const int <Tên Error> = <giá trị>;
        public const int LearnClassroomNotFound = 4000;
        public const int LearnClassroomExisted = 4001;
        public const int LearnSubjectNotExisted = 4002;
        public const int LearnSubjectNotFound = 4003;
        public const int LearnSubjectExisted = 4004;
        public const int LearnCategoryNotFound = 4005;
        public const int LearnCertificateNotFound = 4006;
        public const int LearnTagNotFound = 4007;
        public const int LearnCourseNotFound = 4008;
        public const int LearnSectionNotFound = 4009;
        public const int LearnSectionDetailNotFound = 4010;
        public const int LearnSectionExamMustHaveAtLeastOneExam = 4011;
        public const int LearnLearnerExisted = 4012;
        public const int LearnTrainerExisted = 4013;
        public const int LearnTrainerNotFound = 4014;
        public const int LearnLearnerNotFound = 4015;
        public const int LearnCourseEnrolled = 4016;
        public const int OutOfEnrollPeriod = 4017;
        public const int UnenrollNotAllowed = 4018;
        public const int EnrollmentNotFound = 4019;   
        public const int UserDoesNotHavePermission = 4020;
        public const int LearnCourseDocumentNotFound = 4021;
        public const int LearnLearnerHaveNotBeenApproved = 4022;
        public const int LearnLearnerIsNotActiveInCourse = 4023;
        public const int LearnCourseSectionIsPrerequisite = 4024;
        public const int LearnCourseEnrollRequestNotFound = 4025;
        public const int LearnExamNotFoundInSection = 4026;
        public const int LearnSkillLevelSetNotFound = 4027;
        public const int LearnSkillNotFound = 4028;
        public const int LearnSkillUserNotFound = 4029;
        public const int LearnCourseAssessmentNotFound = 4030;
        public const int LearnCourseAssessmentLearnerExists = 4031;
        public const int LearnSkillLevelNameDuplicate = 4032;
        public const int LearnCourseAssessmentLearnerNotFound = 4033;
        public const int LearnAssessmentLearnerAttachmentNotFound = 4034;
        public const int LearnCourseAssessmentLearnerSubmitted = 4035;
        public const int LearnCourseAssessmentNotStarted = 4036;
        public const int LearnCourseAssessmentEnded = 4037;
        public const int LearnCourseAssessmentLearnerSubmitLimit = 4037;
        public const int LearnCourseContentIsRequired = 4038;
        [Obsolete("Use LLmErrorCode instead")]
        public const int LearnGenerateLlmError = 4039;
        public const int LearnDiscussNotFound = 4040;
        public const int LearnSkillLevelSetIsDefault = 4041;
        public const int LearnSkillLevelNameNotFound = 4042;
        public const int LearnParentCategoryNotFound = 4043;
        public const int LearnPublicExamMustHaveExam = 4044;
        public const int LearnPublicExamNotFound = 4045;
    }
}
