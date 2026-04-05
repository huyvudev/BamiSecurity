namespace CR.Constants.ErrorCodes
{
    public class EventErrorCode : ErrorCode
    {
        protected EventErrorCode() : base() { }

        //public const int <Tên Error> = <giá trị>;
        public const int EvtEventNotFound = 8000;
        public const int EvtEventTypeNotFound = 8001;
        public const int EvtEventTypeDuplicated = 8002;
        public const int EvtEventAttended = 8003;
        public const int EvtEventOutOfPeriod = 8004;
        public const int EvtEventTypeHasEvent = 8005;
        public const int EvtEventAttendanceNotFound = 8006;
        public const int EvtEventAssignUserFailed = 8007;
        public const int EvtEventTypeColorNotFound = 8008;
    }

}
