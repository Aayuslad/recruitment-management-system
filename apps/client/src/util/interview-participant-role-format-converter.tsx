import type { InterviewParticipantRole } from '@/types/enums';

export function interviewParticipantRoleFormatConverter(
    role: InterviewParticipantRole,
    count: number
) {
    switch (role) {
        case 'Interviewer':
            return count > 1 ? `${count} Interviewers` : `${count} Interviewer`;
        case 'HiringManager':
            return count > 1
                ? `${count} Hiring Managers`
                : `${count} Hiring Manager`;
        case 'HRRepresentative':
            return count > 1
                ? `${count} HR Representatives`
                : `${count} HR Representative`;
        case 'NoteTaker':
            return count > 1 ? `${count} Note Takers` : `${count} Note Taker`;
        case 'Observer':
            return count > 1 ? `${count} Observers` : `${count} Observer`;
    }
}
