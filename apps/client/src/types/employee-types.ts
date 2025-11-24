export interface Employee {
    id: string;
    designationId: string;
    designationName: string;
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    contactNumber: string;
    dob: string;
}
