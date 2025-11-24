import type { Gender, UserStatus } from './enums';
import type { components } from './generated/api';

export interface User {
    isProfileCompleted: boolean;
    authId: string;
    userId?: string | null;
    userName: string;
    email: string;
    firstName?: string | null;
    middleName?: string | null;
    lastName?: string | null;
    status?: UserStatus | null;
    contactNumber?: string | null;
    isContactNumberVerified?: boolean | null;
    gender?: Gender | null;
    dob?: string | null;
    roles: {
        id: string;
        name: string;
    }[];
}

export type CreateUserCommandCorrected = Omit<
    components['schemas']['CreateUserProfileCommand'],
    'firstName' | 'lastName' | 'dob' | 'contactNumber'
> & {
    firstName: string;
    dob: string;
    lastName: string;
    contactNumber: string;
};

export type RegisterUserCommandCorrected = Omit<
    components['schemas']['RegisterUserCommand'],
    'userName' | 'email' | 'password'
> & {
    userName: string;
    email: string;
    password: string;
};

export type LoginUserCommandCorrected = Omit<
    components['schemas']['LoginUserCommand'],
    'usernameOrEmail' | 'password'
> & {
    usernameOrEmail: string;
    password: string;
};

export type EditUserRolesCommandCorrected = Omit<
    components['schemas']['EditUserRolesCommand'],
    'userId' | 'roles'
> & {
    userId: string;
    roles: (Omit<
        components['schemas']['UserRolesDTO'],
        'roleId' | 'assignedBy'
    > & {
        roleId: string;
        assignedBy?: string | null;
    })[];
};
