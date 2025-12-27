type RouteConfigType = {
    path: string;
    breadcrumb: string | ((params: string) => string);
    children?: RouteConfigType[];
};

export const routes: RouteConfigType[] = [
    {
        breadcrumb: 'Dashboard',
        path: 'dashboard',
    },
    {
        breadcrumb: 'Job Openings',
        path: 'job-openings',
        children: [
            {
                breadcrumb: 'Opening',
                path: 'opening/:id',
            },
        ],
    },
    {
        breadcrumb: 'Candidates',
        path: 'candidates',
        children: [
            {
                breadcrumb: 'Candidate',
                path: 'candidate/:id',
            },
        ],
    },
    {
        breadcrumb: 'Job Applications',
        path: 'job-applications',
        children: [
            {
                breadcrumb: 'Application',
                path: 'application/:id',
            },
        ],
    },
    {
        breadcrumb: 'Interviews',
        path: 'interviews',
        children: [
            {
                breadcrumb: 'Interview',
                path: 'interview/:id',
            },
        ],
    },
    {
        breadcrumb: 'Screenings',
        path: 'screenings',
        children: [
            {
                breadcrumb: 'Job Application',
                path: 'application/:id',
            },
        ],
    },
    {
        breadcrumb: 'Positions',
        path: 'positions',
        children: [
            {
                breadcrumb: 'Batch',
                path: 'batch/:id',
            },
        ],
    },
    {
        breadcrumb: 'Reports & Analytics',
        path: 'reports-and-analytics',
    },
    {
        breadcrumb: 'Events',
        path: 'events',
    },
    {
        breadcrumb: 'Configurations',
        path: 'configuration',
        children: [
            {
                breadcrumb: 'Skills',
                path: 'skills',
            },
            {
                breadcrumb: 'Designations',
                path: 'designations',
            },
            {
                breadcrumb: 'Document Types',
                path: 'document-types',
            },
        ],
    },
    {
        breadcrumb: 'Admin',
        path: 'admin',
        children: [
            {
                breadcrumb: 'Users',
                path: 'users',
            },
            {
                breadcrumb: 'Roles',
                path: 'roles',
            },
            {
                breadcrumb: 'Employees',
                path: 'employees',
            },
        ],
    },
];
