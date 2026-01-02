import { GENDER, type Gender } from '@/types/enums';

export function genderFormatConverter(gender: Gender) {
    switch (gender) {
        case GENDER.PREFER_NOT_TO_SAY:
            return 'Prefer not to say';
        default:
            return gender;
    }
}
