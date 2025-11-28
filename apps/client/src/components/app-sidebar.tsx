'use client';

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
import * as React from 'react';

import { useGetUser } from '@/api/user-api';
import { CollapsibleNavGroup } from '@/components/collapsible-nav-group';
import { NavUser } from '@/components/nav-user';
import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarRail,
} from '@/components/ui/sidebar';
import { SimpleNavGroup } from './simple-nav-group';

const data = {
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
        },
        {
            name: 'Candidates',
            url: '/candidates',
            icon: UsersRound,
        },
        {
            name: 'Job Applications',
            url: 'job-applications',
            icon: ClipboardList,
        },
        {
            name: 'Interviews',
            url: 'interviews',
            icon: CalendarCheck,
        },
    ],
    supportingWorkflows: [
        {
            name: 'Positions',
            url: 'positions',
            icon: Layers,
        },
        {
            name: 'Events',
            url: 'events',
            icon: CalendarDays,
        },
        {
            name: 'Reports & Analytics',
            url: 'reports-and-analytics',
            icon: BarChart3,
        },
    ],
    other: [
        {
            title: 'Configurations',
            url: 'configuration',
            icon: SlidersHorizontal,
            isActive: false,
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
        {
            title: 'Admin',
            url: 'admin',
            icon: Settings2,
            isActive: false,
            items: [
                {
                    title: 'Users',
                    url: 'admin/users',
                },
                {
                    title: 'Roles',
                    url: 'admin/roles',
                },
                {
                    title: 'Employees',
                    url: 'admin/employees',
                },
            ],
        },
    ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
    const { data: user } = useGetUser();

    data.user.name = `${user?.firstName} ${user?.lastName}`;
    // eslint-disable-next-line
    data.user.email = user?.email! ?? 'unknown@example.com';

    return (
        <Sidebar collapsible="icon" {...props}>
            <SidebarContent className="pt-5">
                <SimpleNavGroup Workflows={data.dashboard} />
                <SimpleNavGroup
                    Workflows={data.coreWorkflows}
                    title="Core Workflows"
                />
                <SimpleNavGroup
                    Workflows={data.supportingWorkflows}
                    title="Supporting Workflows"
                />
                <CollapsibleNavGroup items={data.other} title="Other" />
            </SidebarContent>
            <SidebarFooter>
                <NavUser user={data.user} />
            </SidebarFooter>
            <SidebarRail />
        </Sidebar>
    );
}
