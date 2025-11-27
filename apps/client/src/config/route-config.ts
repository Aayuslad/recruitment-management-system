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
        breadcrumb: 'Job Applications',
        path: 'job-applications',
    },
    {
        breadcrumb: 'Candidates',
        path: 'candidates',
    },
    {
        breadcrumb: 'Job Applications',
        path: 'job-applications',
    },
    {
        breadcrumb: 'Interviews',
        path: 'interviews',
    },
    {
        breadcrumb: 'Positions',
        path: 'positions',
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
