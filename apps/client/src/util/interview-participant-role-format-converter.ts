import type { InterviewParticipantRole } from '@/types/enums';

export function interviewParticipantRoleFormatConverter(
    role: InterviewParticipantRole,
    isPlural: boolean = false
) {
    switch (role) {
        case 'TechnicalInterviewer':
            return isPlural
                ? 'Technical Interviewers'
                : 'Technical Interviewer';
        case 'HRInterviewer':
            return isPlural ? 'HR Interviewers' : 'HR Interviewer';
        case 'NoteTaker':
            return isPlural ? 'Note Takers' : 'Note Taker';
        case 'Observer':
            return isPlural ? 'Observers' : 'Observer';
    }
}
