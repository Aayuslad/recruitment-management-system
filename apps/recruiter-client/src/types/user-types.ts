import type { components } from './generated/api';

export interface UserDTO {
    authId: string;
    userId: string;
    userName: string;
    email: string;
    firstName: string;
    middleName?: string;
    lastName: string;
    status?: UserStatus;
    contactNumber: string;
    IsContactNumberVerified?: boolean;
    Gender?: components['schemas']['Gender'];
    dob: string;
}

type UserStatus = 'OnHold' | 'Active' | 'Inactive' | 'Pending';

export type CreateUserCommandCorrected = Omit<
    components['schemas']['CreateUserCommand'],
    'firstName' | 'lastName' | 'dob'
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
