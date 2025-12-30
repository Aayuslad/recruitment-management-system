import { INTERVIEW_STATUS, type InterviewStatus } from '@/types/enums';

export function interviewStatusFormatConverter(status: InterviewStatus) {
    switch (status) {
        case INTERVIEW_STATUS.NOT_SCHEDULED:
            return 'Not Scheduled';
        default:
            return status;
    }
}
