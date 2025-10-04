'use client';

import {
    BarChart3,
    BellRing,
    Briefcase,
    CalendarClock,
    ClipboardCheck,
    FileCheck2,
    LayoutDashboard,
    SearchCheck,
    UserCog,
    Users,
} from 'lucide-react';
import * as React from 'react';

import { useGetUser } from '@/api/user';
import { NavMain } from '@/components/nav-main';
import { NavUser } from '@/components/nav-user';
import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarHeader,
    SidebarRail,
} from '@/components/ui/sidebar';

const data = {
    user: {
        name: 'loading...',
        email: 'loading...',
        avatar: '/avatars/shadcn.jpg',
    },
    // teams: [
    //   {
    //     name: "Acme Inc",
    //     logo: GalleryVerticalEnd,
    //     plan: "Enterprise",
    //   },
    //   {
    //     name: "Acme Corp.",
    //     logo: AudioWaveform,
    //     plan: "Startup",
    //   },
    //   {
    //     name: "Evil Corp.",
    //     logo: Command,
    //     plan: "Free",
    //   },
    // ],
    navMain: [
        {
            title: 'Dashboard',
            url: '/dashboard',
            icon: LayoutDashboard,
            isActive: false,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Jobs',
            url: '/jobs',
            icon: Briefcase,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Candidates',
            url: '/candidates',
            icon: Users,
            items: [
                {
                    title: 'sub page 1 (Working)',
                    url: '/candidates/candidate-sub-page-1',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Screening & Shortlisting',
            url: 'screening-and-shortlisting',
            icon: SearchCheck,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Interview Scheduling',
            url: 'interview-scheduling',
            icon: CalendarClock,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Feedback & Evaluation',
            url: 'feedback-and-evaluation',
            icon: ClipboardCheck,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Document Verification',
            url: 'document-verification',
            icon: FileCheck2,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Notifications & Tasks',
            url: 'notification-and-tasks',
            icon: BellRing,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Reports & Analytics',
            url: 'reports-and-analytics',
            icon: BarChart3,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
        {
            title: 'Admin / Settings',
            url: 'admin-settings',
            icon: UserCog,
            items: [
                {
                    title: 'sub page 1',
                    url: '#',
                },
                {
                    title: 'sub page 2',
                    url: '#',
                },
                {
                    title: 'sub page 3',
                    url: '#',
                },
            ],
        },
    ],
    // projects: [
    //     {
    //         name: 'Design Engineering',
    //         url: '#',
    //         icon: Frame as LucideIcon,
    //     },
    //     {
    //         name: 'Sales & Marketing',
    //         url: '#',
    //         icon: PieChart as LucideIcon,
    //     },
    //     {
    //         name: 'Travel',
    //         url: '#',
    //         // @ts-ignore
    //         icon: Map as LucideIcon,
    //     },
    // ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
    const { data: user } = useGetUser();

    data.user.name = `${user?.firstName} ${user?.lastName}`;
    // eslint-disable-next-line
    data.user.email = user?.email! ?? 'unknown@example.com';

    return (
        <Sidebar collapsible="icon" {...props}>
            <SidebarHeader>
                {/* <TeamSwitcher teams={data.teams} /> */}
            </SidebarHeader>
            <SidebarContent>
                <NavMain items={data.navMain} />
                {/* <NavProjects projects={data.projects} /> */}
            </SidebarContent>
            <SidebarFooter>
                <NavUser user={data.user} />
            </SidebarFooter>
            <SidebarRail />
        </Sidebar>
    );
}
