import {
    BarChart3,
    Briefcase,
    CalendarCheck,
    CalendarDays,
    ClipboardList,
    Layers,
    LayoutDashboard,
    Settings2,
    SlidersHorizontal,
    UsersRound,
} from 'lucide-react';

export const sidebarNavConfig = {
    user: {
        name: 'loading...',
        email: 'loading...',
        avatar: '/avatars/shadcn.jpg',
    },
    dashboard: [
        {
            name: 'Dashboard',
            url: '/dashboard',
            icon: LayoutDashboard,
        },
    ],
    coreWorkflows: [
        {
            name: 'Job Openings',
            url: '/job-openings',
            icon: Briefcase,
            roles: ['Admin', 'Recruiter', 'Viewer'],
        },
        {
            name: 'Candidates',
            url: '/candidates',
            icon: UsersRound,
            roles: [
                'Admin',
                'Recruiter',
                'Interviewer',
                'Reviewer',
                'HR',
                'Viewer',
            ],
        },
        {
            name: 'Job Applications',
            url: 'job-applications',
            icon: ClipboardList,
            roles: [
                'Admin',
                'Recruiter',
                'Interviewer',
                'Reviewer',
                'HR',
                'Viewer',
            ],
        },
    ],
    myWork: [
        {
            name: 'Screenings',
            url: 'screenings',
            icon: CalendarCheck,
            roles: ['Reviewer'],
        },
        {
            name: 'Interviews',
            url: 'interviews',
            icon: CalendarCheck,
            roles: ['Interviewer', 'Admin', 'Recruiter', 'HR'],
        },
    ],
    supportingWorkflows: [
        {
            name: 'Positions',
            url: 'positions',
            icon: Layers,
            roles: ['Admin', 'Recruiter', 'Viewer'],
        },
        {
            name: 'Events',
            url: 'events',
            icon: CalendarDays,
            roles: ['Admin', 'Recruiter', 'Viewer'],
        },
    ],
    admin: [
        {
            name: 'Users',
            url: 'admin/users',
            icon: Settings2,
            roles: ['Admin'],
        },
        // TODO: show roles in batter place, a page to show 5 roles is non-sense
        // {
        //     name: 'Roles',
        //     url: 'admin/roles',
        //     icon: Settings2,
        //     roles: ['Admin'],
        // },
    ],
    other: [
        {
            name: 'Employees',
            url: 'admin/employees',
            icon: Settings2,
            roles: ['Admin', 'HR', 'Viewer'],
        },
        {
            name: 'Reports & Analytics',
            url: 'reports-and-analytics',
            icon: BarChart3,
            roles: ['Admin', 'Recruiter', 'Viewer'],
        },
    ],
    collapsibleGrop: [
        {
            title: 'Configurations',
            url: 'configuration',
            icon: SlidersHorizontal,
            isActive: false,
            roles: ['Admin', 'Recruiter', 'Viewer'],
            items: [
                {
                    title: 'Skills',
                    url: 'configuration/skills',
                },
                {
                    title: 'Designations',
                    url: 'configuration/designations',
                },
                {
                    title: 'Document Types',
                    url: 'configuration/document-types',
                },
            ],
        },
    ],
};
