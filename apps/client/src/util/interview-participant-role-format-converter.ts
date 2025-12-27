import type { InterviewParticipantRole } from '@/types/enums';

export function interviewParticipantRoleFormatConverter(
    role: InterviewParticipantRole,
    count: number
) {
    switch (role) {
        case 'TechnicalInterviewer':
            return count > 1
                ? `${count} Technical Interviewers`
                : `${count} Technical Interviewer`;
        case 'HRInterviewer':
            return count > 1
                ? `${count} HR Interviewers`
                : `${count} HR Interviewer`;
        case 'NoteTaker':
            return count > 1 ? `${count} Note Takers` : `${count} Note Taker`;
        case 'Observer':
            return count > 1 ? `${count} Observers` : `${count} Observer`;
    }
}
