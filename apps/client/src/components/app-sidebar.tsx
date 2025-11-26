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
import { NavMain } from '@/components/nav-main';
import { NavUser } from '@/components/nav-user';
import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarRail,
} from '@/components/ui/sidebar';
import { NavGroup } from './nav-group';

const data = {
    user: {
        name: 'loading...',
        email: 'loading...',
        avatar: '/avatars/shadcn.jpg',
    },
    dashboard: [
        {
            name: 'Dashboard',
            url: '#',
            icon: LayoutDashboard,
        },
    ],
    navMain: [
        {
            title: 'Configurations',
            url: '#',
            icon: SlidersHorizontal,
            isActive: false,
            items: [
                {
                    title: 'Skills',
                    url: '#',
                },
                {
                    title: 'Designations',
                    url: '#',
                },
                {
                    title: 'Document Types',
                    url: '#',
                },
            ],
        },
        {
            title: 'Admin',
            url: '#',
            icon: Settings2,
            isActive: false,
            items: [
                {
                    title: 'Users',
                    url: '#',
                },
                {
                    title: 'Roles',
                    url: '#',
                },
                {
                    title: 'Employees',
                    url: '#',
                },
            ],
        },
    ],
    coreWorkflows: [
        {
            name: 'Job Openings',
            url: '#',
            icon: Briefcase,
        },
        {
            name: 'Candidates',
            url: '#',
            icon: UsersRound,
        },
        {
            name: 'Job Applications',
            url: '#',
            icon: ClipboardList,
        },
        {
            name: 'Interviews',
            url: '#',
            icon: CalendarCheck,
        },
    ],
    supportingWorkflows: [
        {
            name: 'Positions',
            url: '#',
            icon: Layers,
        },
        {
            name: 'Events',
            url: '#',
            icon: CalendarDays,
        },
        {
            name: 'Reports & Analytics',
            url: '#',
            icon: BarChart3,
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
            <SidebarContent className="pt-10">
                <NavGroup Workflows={data.dashboard} />
                <NavGroup
                    Workflows={data.coreWorkflows}
                    title="Core Workflows"
                />
                <NavGroup
                    Workflows={data.supportingWorkflows}
                    title="Supporting Workflows"
                />
                <NavMain items={data.navMain} />
            </SidebarContent>
            <SidebarFooter>
                <NavUser user={data.user} />
            </SidebarFooter>
            <SidebarRail />
        </Sidebar>
    );
}
