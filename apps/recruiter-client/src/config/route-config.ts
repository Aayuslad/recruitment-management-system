type RouteConfigType = {
    path: string;
    breadcrumb: string | ((params: string) => string);
    children?: RouteConfigType[];
};

export const routes: RouteConfigType[] = [
    {
        breadcrumb: 'Dashboaerd',
        path: 'dashboard',
    },
    {
        breadcrumb: 'Jobs',
        path: 'jobs',
        children: [
            {
                path: 'openings',
                breadcrumb: 'Openings',
            },
            {
                path: 'positions',
                breadcrumb: 'Positions',
            },
            {
                path: 'designations',
                breadcrumb: 'Designations',
            },
            {
                path: 'skills',
                breadcrumb: 'Skills',
            },
        ],
    },
    {
        breadcrumb: 'Candidates',
        path: 'candidates',
        // all sbling pages
        children: [
            {
                path: 'candidate-sub-page-1',
                breadcrumb: 'Candidate Sub Page 1',
            },
        ],
    },
    {
        breadcrumb: 'Screening & Shortlisting',
        path: 'screening-and-shortlisting',
    },
    {
        breadcrumb: 'Interview Scheduling',
        path: 'interview-scheduling',
    },
    {
        breadcrumb: 'Feedback & Evaluation',
        path: 'feedback-and-evaluation',
    },
    {
        breadcrumb: 'Document Verification',
        path: 'document-verification',
    },
    {
        breadcrumb: 'Notifications & Tasks',
        path: 'notification-and-tasks',
    },
    {
        breadcrumb: 'Reports & Analytics',
        path: 'reports-and-analytics',
    },
    {
        breadcrumb: 'Admin / Settings',
        path: 'admin-settings',
    },
];
